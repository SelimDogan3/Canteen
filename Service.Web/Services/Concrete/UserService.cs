using AutoMapper;
using Cantin.Data.Identity;
using Cantin.Data.UnitOfWorks;
using Cantin.Entity.Dtos.Stores;
using Cantin.Entity.Dtos.Users;
using Cantin.Entity.Entities;
using Cantin.Service.Extensions;
using Cantin.Service.Services.Abstraction;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Security.Claims;

namespace Cantin.Service.Services.Concrete
{
    public class UserService : IUserService
    {
        private readonly UserManager<AppUser> userManager;
        private readonly RoleManager<AppRole> roleManager;
        private readonly IMapper mapper;
        private readonly IUnitOfWork unitOfWork;
        private readonly IStoreService storeService;
        private readonly IdentityErrorDescriber errorDescriber;
        private readonly IValidator<AppUser> validator;
        private readonly ClaimsPrincipal _user;
        private string? userRole => _user.GetRole();
        private string? userMail => _user.GetLoggedInUserEmail();

        public UserService(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager, IMapper mapper, IUnitOfWork unitOfWork, IStoreService storeService, IdentityErrorDescriber errorDescriber, IHttpContextAccessor contextAccessor, IValidator<AppUser> validator)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.mapper = mapper;
            this.unitOfWork = unitOfWork;
            this.storeService = storeService;
            this.errorDescriber = errorDescriber;
            this.validator = validator;
            _user = contextAccessor.HttpContext.User;
        }
        public List<UserDto> GetAllUsers()
        {
            List<UserDto> dto = mapper.Map<List<UserDto>>(userManager.Users.ToList());
            return dto;
        }
        public async Task<List<UserDto>> GetAllUsersWithRolesAsync()
        {

            IQueryable<AppUser> users = userManager.Users;
            List<UserDto> map = mapper.Map<List<UserDto>>(users);
            foreach (var item in map)
            {
                AppUser user = await userManager.FindByIdAsync(item.Id.ToString());
                AppRole role = await GetUserRoleAsync(user.Id);
                item.Role = role.Name;
            }
            return map;
        }
        public async Task<List<UserDto>> GetAllUsersWithRolesAndStoresAsync()
        {
            IQueryable<AppUser> users = userManager.Users;
            List<UserDto> map = mapper.Map<List<UserDto>>(users);
            List<UserDto> removeItem = new();
            foreach (var item in map)
            {
                AppUser user = await userManager.FindByIdAsync(item.Id.ToString());
                AppRole role = await GetUserRoleAsync(user.Id);
                if (userRole != "Superadmin" && role.Name == "Superadmin")
                {
                    removeItem.Add(item);
                }
                    item.Role = role.Name;
            }
            if (removeItem.Count > 0)
            {
                map.RemoveAll(x => removeItem.Contains(x));
            }
            foreach (var item in map)
            {
                item.Store = await storeService.GetStoreById(item.StoreId);
            }
            return map;
        }
        public async Task<AppRole> GetUserRoleAsync(Guid id)
        {
            IList<string> roles = await userManager.GetRolesAsync(await userManager.FindByIdAsync(id.ToString()));
            AppRole role = await roleManager.FindByNameAsync(String.Join("", roles));
            return role;

        }
        public async Task<IdentityResult> UpdateUserRole(AppUser user, string roleName)
        {
            AppRole oldRole = await GetUserRoleAsync(user.Id);
            IdentityResult result = await userManager.RemoveFromRoleAsync(user, oldRole.Name);
            if (!result.Succeeded)
                return result;
            result = await userManager.AddToRoleAsync(user, roleName);
            return result;
        }
        public async Task<IdentityResult> UpdateUserRole(AppUser user, AppRole newRole)
        {
            AppRole oldRole = await GetUserRoleAsync(user.Id);
            IdentityResult result = await userManager.RemoveFromRoleAsync(user, oldRole.Name);
            if (!result.Succeeded)
                return result;
            result = await userManager.AddToRoleAsync(user, newRole.Name);
            return result;

        }
        public async Task<IdentityResult> UpdateUserRole(AppUser user, Guid roleId)
        {
            AppRole oldRole = await GetUserRoleAsync(user.Id);
            AppRole newRole = await roleManager.FindByIdAsync(roleId.ToString());
            IdentityResult result = await userManager.RemoveFromRoleAsync(user, oldRole.Name);
            if (!result.Succeeded)
                return result;
            result = await userManager.AddToRoleAsync(user, newRole.Name);
            return result;

        }
        public async Task<UserAddDto> GetUserAddDto()
        {

            List<Store> stores = await unitOfWork.GetRepository<Store>().GetAllAsync(x => !x.IsDeleted);
            List<StoreDto> storesMap = mapper.Map<List<StoreDto>>(stores);
            IQueryable<AppRole> roles = roleManager.Roles;
            roles = roles.Where(x => x.Name != "Superadmin");
            UserAddDto dto = new()
            {
                Roles = roles.ToList(),
                Stores = storesMap
            };
            return dto;

        }
        public async Task<IdentityResult> AddUserWithRoleAsync(UserAddDto dto)
        {
            AppUser loggedInUser = await userManager.FindByEmailAsync(_user.GetLoggedInUserEmail());
            IdentityResult result = await CheckIfPasswordMatch(loggedInUser, dto.Password);
            if (!result.Succeeded)
                return result;
            AppUser user = mapper.Map<AppUser>(dto);
            user.UserName = dto.Email;
            user.PhoneNumberConfirmed = true;
            user.EmailConfirmed = true;
            result = await userManager.CreateAsync(user, dto.Pass);
            if (result.Succeeded)
            {
                var role = await roleManager.FindByIdAsync(dto.RoleId.ToString());
                result = await userManager.AddToRoleAsync(user, role.Name);
            }
            return result;

        }
        public async Task<UserUpdateDto> GetUpdateUserDto(Guid id)
        {
            AppUser user = await userManager.FindByIdAsync(id.ToString());
            UserUpdateDto dto = mapper.Map<UserUpdateDto>(user);
            List<StoreDto> stores = await storeService.GetAllStoreDtosNonDeleted();
            AppRole userRole = await GetUserRoleAsync(user.Id);
            dto.RoleId = userRole.Id;
            dto.Roles = roleManager.Roles.ToList();
            dto.Stores = stores;
            return dto;
        }
        public async Task<IdentityResult> UpdateUserWithRoleAsync(UserUpdateDto dto)
        {
            AppUser loggedInUser = await userManager.FindByEmailAsync(_user.GetLoggedInUserEmail());
            AppUser user = await userManager.FindByIdAsync(dto.Id.ToString());
            IdentityResult result = await CheckIfPasswordMatch(loggedInUser, dto.Password);
            if (!result.Succeeded)
                return result;
            if (dto.NewPassword != null)
            {
                result = await ChangeUserPasswordWithoutConfig(user, dto.NewPassword);
                if (!result.Succeeded)
                    result.Errors.Cast<ModelAddIdentityError>().First().key = "NewPassword";
                return result;
            }
            AppUser map = mapper.Map(dto, user);
            AppRole role = await roleManager.FindByIdAsync(dto.RoleId.ToString());
            result = await UpdateUserRole(map, role);
            return result;
        }
        public async Task<IdentityResult> DeleteUserAsync(Guid Id, string password)
        {
            AppUser loggedInUser = await userManager.FindByEmailAsync(_user.GetLoggedInUserEmail());
            IdentityResult result = await CheckIfPasswordMatch(loggedInUser, password);
            if (!result.Succeeded)
                return result;
            AppUser user = await userManager.FindByIdAsync(Id.ToString());
            result = await userManager.DeleteAsync(user);
            return result;
        }
        public async Task<IdentityResult> CheckIfPasswordMatch(AppUser user, string password)
        {
            List<IdentityError> errors = new List<IdentityError>();
            bool isMatch = await userManager.CheckPasswordAsync(user, password);
            if (isMatch)
            {
                return IdentityResult.Success;
            }
            IdentityError error = errorDescriber.PasswordMismatch();
            errors.Add(error);
            return IdentityResult.Failed(errors.ToArray());
        }

        public async Task ValidateUserAsync(UserAddDto dto, ModelStateDictionary modelState)
        {
            if (dto.Password == null)
            {
                modelState.AddModelError("Password", "Yönetici Şifresi boş olamaz");
            }
            AppUser user = mapper.Map<AppUser>(dto);
            ValidationResult result = await validator.ValidateAsync(user);
            if (!result.IsValid)
            {
                result.AddErrorsToModelState(modelState);
            }
        }

        public async Task ValidateUserAsync(UserUpdateDto dto, ModelStateDictionary modelState)
        {
            if (dto.Password == null)
            {
                modelState.AddModelError("Password", "Yönetici Şifresi boş olamaz");
            }
            AppUser user = mapper.Map<AppUser>(dto);
            ValidationResult result = await validator.ValidateAsync(user);
            if (!result.IsValid)
            {
                result.AddErrorsToModelState(modelState);
            }
        }

        public async Task<AppUser> GetUserByEmail(string email)
        {
            var user = await userManager.FindByEmailAsync(email);
            return user;
        }

        public async Task<IdentityResult> ChangeUserPasswordWithoutConfig(AppUser user, string newPassword)
        {
            var token = await userManager.GeneratePasswordResetTokenAsync(user);
            IdentityResult result = await userManager.ResetPasswordAsync(user, token, newPassword);
            return result;
        }
    }
}
