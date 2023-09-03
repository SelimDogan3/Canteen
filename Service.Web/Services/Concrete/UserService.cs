using AutoMapper;
using Cantin.Data.UnitOfWorks;
using Cantin.Entity.Dtos.Stores;
using Cantin.Entity.Dtos.Users;
using Cantin.Entity.Entities;
using Cantin.Service.Services.Abstraction;
using Microsoft.AspNetCore.Identity;

namespace Cantin.Service.Services.Concrete
{
    public class UserService : IUserService
    {
        private readonly UserManager<AppUser> userManager;
        private readonly RoleManager<AppRole> roleManager;
        private readonly IMapper mapper;
        private readonly IUnitOfWork unitOfWork;
        private readonly IStoreService storeService;

        public UserService(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager, IMapper mapper, IUnitOfWork unitOfWork,IStoreService storeService)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.mapper = mapper;
            this.unitOfWork = unitOfWork;
            this.storeService = storeService;
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
            foreach (var item in map)
            {
				AppUser user = await userManager.FindByIdAsync(item.Id.ToString());
				AppRole role = await GetUserRoleAsync(user.Id);
                item.Role = role.Name;
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
            UserAddDto dto = new()
            {
                Roles = roles.ToList(),
                Stores = storesMap
            };
            return dto;

        }
        public async Task<IdentityResult> AddUserWithRoleAsync(UserAddDto dto)
        {
            AppUser user = mapper.Map<AppUser>(dto);
            user.UserName = dto.Email;
            user.PhoneNumberConfirmed = true;
            user.EmailConfirmed = true;
			IdentityResult result = await userManager.CreateAsync(user, dto.pass);
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
            AppUser user = await userManager.FindByIdAsync(dto.Id.ToString());
            AppUser map = mapper.Map(dto, user);
            AppRole role = await roleManager.FindByIdAsync(dto.RoleId.ToString());
			IdentityResult result = await UpdateUserRole(map, role);
            return result;
        }
        public async Task<IdentityResult> DeleteUserAsync(Guid Id)
        {
			AppUser user = await userManager.FindByIdAsync(Id.ToString());
			IdentityResult result = await userManager.DeleteAsync(user);
            return result;
        }
    }
}
