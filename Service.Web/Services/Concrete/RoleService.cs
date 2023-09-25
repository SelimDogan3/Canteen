using AutoMapper;
using Cantin.Entity.Dtos.Roles;
using Cantin.Entity.Dtos.Supplies;
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

	public class RoleService : IRoleService
    {
        private readonly RoleManager<AppRole> roleManager;
        private readonly IMapper mapper;
		private readonly IValidator<AppRole> validator;
        private readonly ClaimsPrincipal _user;
        private string? userRole => _user.GetRole();
        private string? userMail => _user.GetLoggedInUserEmail();

        public RoleService(RoleManager<AppRole> roleManager, IMapper mapper, IValidator<AppRole> validator,IHttpContextAccessor contextAccessor)
        {
            this.roleManager = roleManager;
            this.mapper = mapper;
			this.validator = validator;
			_user = contextAccessor.HttpContext.User;
        }
		public async Task<List<RoleDto>> GetAllRoles()
		{
			List<AppRole> roles = roleManager.Roles.ToList();
			if (userRole != "Superadmin") {
                var role = await roleManager.FindByNameAsync("Superadmin");
                roles.Remove(role);
			}
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

		public async Task ValidateRoleAsync(RoleAddDto dto, ModelStateDictionary modelState)
		{
			AppRole role = mapper.Map<AppRole>(dto);
			ValidationResult result = await validator.ValidateAsync(role);
			if (!result.IsValid) {
				result.AddErrorsToModelState(modelState);
			}
		}

		public async Task ValidateRoleAsync(RoleUpdateDto dto, ModelStateDictionary modelState)
		{
			AppRole role = mapper.Map<AppRole>(dto);
			ValidationResult result = await validator.ValidateAsync(role);
			if (!result.IsValid)
			{
				result.AddErrorsToModelState(modelState);
			}
		}
	}
}
