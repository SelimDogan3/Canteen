using Cantin.Entity.Dtos.Products;
using Cantin.Entity.Dtos.Users;
using Cantin.Entity.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Cantin.Service.Services.Abstraction
{
    public interface IUserService
    {
        List<UserDto> GetAllUsers();
        Task<List<UserDto>> GetAllUsersWithRolesAsync();
        Task<List<UserDto>> GetAllUsersWithRolesAndStoresAsync();
        Task<AppUser> GetUserByEmail(string email);
        Task<AppRole> GetUserRoleAsync(Guid id);
        Task<IdentityResult> UpdateUserRole(AppUser user, string roleName);
        Task<IdentityResult> UpdateUserRole(AppUser user, AppRole role);
        Task<IdentityResult> UpdateUserRole(AppUser user, Guid roleId);
        Task<UserAddDto> GetUserAddDto();
        Task<IdentityResult> AddUserWithRoleAsync(UserAddDto dto);
        Task<UserUpdateDto> GetUpdateUserDto(Guid id);
        Task<IdentityResult> UpdateUserWithRoleAsync(UserUpdateDto dto);
        Task<IdentityResult> DeleteUserAsync(Guid Id,string password);
        Task<IdentityResult> CheckIfPasswordMatch(AppUser user, string password);
		Task ValidateUserAsync(UserAddDto dto, ModelStateDictionary modelState);
		Task ValidateUserAsync(UserUpdateDto dto, ModelStateDictionary modelState);
        Task<IdentityResult> ChangeUserPasswordWithoutConfig(AppUser user,string newPassword);




	}
}
