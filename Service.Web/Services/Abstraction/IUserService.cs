using Cantin.Entity.Dtos.Users;
using Cantin.Entity.Entities;
using Microsoft.AspNetCore.Identity;

namespace Cantin.Service.Services.Abstraction
{
    public interface IUserService
    {
        List<UserDto> GetAllUsers();
        Task<List<UserDto>> GetAllUsersWithRolesAsync();
        Task<List<UserDto>> GetAllUsersWithRolesAndStoresAsync();
        Task<AppRole> GetUserRoleAsync(Guid id);
        Task<IdentityResult> UpdateUserRole(AppUser user, string roleName);
        Task<IdentityResult> UpdateUserRole(AppUser user, AppRole role);
        Task<IdentityResult> UpdateUserRole(AppUser user, Guid roleId);
        Task<UserAddDto> GetUserAddDto();
        Task<IdentityResult> AddUserWithRoleAsync(UserAddDto dto);
        Task<UserUpdateDto> GetUpdateUserDto(Guid id);
        Task<IdentityResult> UpdateUserWithRoleAsync(UserUpdateDto dto);
        Task<IdentityResult> DeleteUserAsync(Guid Id);


    }
}
