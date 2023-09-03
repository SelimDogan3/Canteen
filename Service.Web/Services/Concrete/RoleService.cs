using AutoMapper;
using Cantin.Entity.Dtos.Roles;
using Cantin.Entity.Entities;
using Cantin.Service.Services.Abstraction;
using Microsoft.AspNetCore.Identity;

namespace Cantin.Service.Services.Concrete
{

    public class RoleService : IRoleService
    {
        private readonly RoleManager<AppRole> roleManager;
        private readonly IMapper mapper;
        public RoleService(RoleManager<AppRole> roleManager, IMapper mapper)
        {
            this.roleManager = roleManager;
            this.mapper = mapper;
        }
		public List<RoleDto> GetAllRoles()
		{
			List<AppRole> roles = roleManager.Roles.ToList();
			List<RoleDto> map = mapper.Map<List<RoleDto>>(roles);
			return map;
		}
		public RoleAddDto GetRoleAddDto()
		{
			RoleAddDto dto = new RoleAddDto();
			return dto;
		}
		public async Task<IdentityResult> AddRoleAsync(RoleAddDto addDto)
        {
            AppRole role = mapper.Map<AppRole>(addDto);
            IdentityResult result = await roleManager.CreateAsync(role);
            return result;
        }
		public async Task<RoleUpdateDto> GetRoleUpdateDtoAsync(Guid Id)
		{
			AppRole role = await roleManager.FindByIdAsync(Id.ToString());
			RoleUpdateDto dto = mapper.Map<RoleUpdateDto>(role);
			return dto;
		}

		public async Task<IdentityResult> UpdateRoleAsync(RoleUpdateDto updateDto)
        {
			AppRole role = await roleManager.FindByIdAsync(updateDto.Id.ToString());
			role = mapper.Map(updateDto,role);
            IdentityResult result = await roleManager.UpdateAsync(role);
            return result;
        }
		public async Task<IdentityResult> DeleteRoleAsync(Guid Id)
		{
			AppRole role = await roleManager.FindByIdAsync(Id.ToString());
			IdentityResult result = await roleManager.DeleteAsync(role);
			return result;
		}
	}
}
