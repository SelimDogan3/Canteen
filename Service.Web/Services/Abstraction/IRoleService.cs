using Cantin.Entity.Dtos.Roles;
using Microsoft.AspNetCore.Identity;

namespace Cantin.Service.Services.Abstraction
{
    public interface IRoleService
    {
        List<RoleDto> GetAllRoles();
        RoleAddDto GetRoleAddDto();
        Task<IdentityResult> AddRoleAsync(RoleAddDto addDto);
        Task<RoleUpdateDto> GetRoleUpdateDtoAsync(Guid Id);
        Task<IdentityResult> UpdateRoleAsync(RoleUpdateDto updateDto);
        Task<IdentityResult> DeleteRoleAsync(Guid Id);

    }
}
