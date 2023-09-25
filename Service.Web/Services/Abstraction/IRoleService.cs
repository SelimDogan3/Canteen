using Cantin.Entity.Dtos.Roles;
using Cantin.Entity.Dtos.Supplies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Cantin.Service.Services.Abstraction
{
	public interface IRoleService
    {
        Task<List<RoleDto>> GetAllRoles();
        RoleAddDto GetRoleAddDto();
        Task<IdentityResult> AddRoleAsync(RoleAddDto addDto);
        Task<RoleUpdateDto> GetRoleUpdateDtoAsync(Guid Id);
        Task<IdentityResult> UpdateRoleAsync(RoleUpdateDto updateDto);
        Task<IdentityResult> DeleteRoleAsync(Guid Id);
		Task ValidateRoleAsync(RoleAddDto dto, ModelStateDictionary modelState);
		Task ValidateRoleAsync(RoleUpdateDto dto, ModelStateDictionary modelState);


	}
}
