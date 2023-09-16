using Cantin.Data.Identity;
using Cantin.Entity.Dtos.Roles;
using Cantin.Entity.Entities;
using Cantin.Service.Services.Abstraction;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;

namespace Cantin.Web.Areas.Admin.Controllers
{
    [Authorize(
		Policy = "AdminOnly"
		)]


    public class RoleController : Controller
	{
		private readonly ILogger<RoleController> logger;
		private readonly RoleManager<AppRole> roleManager;
		private readonly IRoleService roleService;
		private readonly IToastNotification toast;
		private readonly string type = "rol";

		public RoleController(ILogger<RoleController> logger, RoleManager<AppRole> roleManager, IRoleService roleService, IToastNotification toast)
		{
			this.logger = logger;
			this.roleManager = roleManager;
			this.roleService = roleService;
			this.toast = toast;
		}
		public IActionResult Index()
		{
			var roles = roleService.GetAllRoles();
			return View(roles);
		}
		[HttpGet]
		public IActionResult Add()
		{
			var dto = roleService.GetRoleAddDto();
			return View(dto);
		}
		[HttpPost]
		public async Task<IActionResult> Add(RoleAddDto addDto)
		{

			await roleService.ValidateRoleAsync(addDto, ModelState);
			if (ModelState.IsValid)
			{
				var result = await roleService.AddRoleAsync(addDto);

				if (!result.Succeeded)
				{
					foreach (ModelAddIdentityError error in result.Errors.Cast<ModelAddIdentityError>())
					{
						ModelState.AddModelError(error.key, error.Description);

					}
					toast.AddErrorToastMessage(Messages.Messages.AddError(addDto.Name, type), new ToastrOptions { Title = "Rol Ekleme" });
					addDto = roleService.GetRoleAddDto();
					return View(addDto);
				}
				toast.AddSuccessToastMessage(Messages.Messages.Add(addDto.Name, type), new ToastrOptions { Title = "Rol Ekleme" });
				return RedirectToAction("Index");
			}
			toast.AddErrorToastMessage(Messages.Messages.AddError(addDto.Name, type), new ToastrOptions { Title = "Rol Ekleme" });
			var dto = roleService.GetRoleAddDto();
			return View(dto);
		}
		[HttpGet]
		public async Task<IActionResult> Update(Guid Id)
		{
			var dto = await roleService.GetRoleUpdateDtoAsync(Id);
			return View(dto);
		}
		[HttpPost]
		public async Task<IActionResult> Update(RoleUpdateDto updateDto)
		{
			await roleService.ValidateRoleAsync(updateDto, ModelState);
			if (ModelState.IsValid)
			{
				var result = await roleService.UpdateRoleAsync(updateDto);
				if (!result.Succeeded)
				{
					foreach (ModelAddIdentityError error in result.Errors.Cast<ModelAddIdentityError>())
					{
						ModelState.AddModelError(error.key, error.Description);

					}
					toast.AddErrorToastMessage(Messages.Messages.UpdateError(updateDto.Name, type), new ToastrOptions { Title = "Rol Güncelleme" });
					return View(updateDto);
				}
				else
				{
					toast.AddSuccessToastMessage(Messages.Messages.Update(updateDto.Name, type), new ToastrOptions { Title = "Rol Güncelleme" });
					return RedirectToAction("Index");
				}
			}
			toast.AddErrorToastMessage(Messages.Messages.UpdateError(updateDto.Name, type), new ToastrOptions { Title = "Rol Güncelleme" });
			return View(updateDto);
		}
		public async Task<IActionResult> Delete(Guid Id)
		{
			var name = (await roleManager.FindByIdAsync(Id.ToString())).Name;
			var result = await roleService.DeleteRoleAsync(Id);
			if (result.Succeeded)
			{
				toast.AddSuccessToastMessage(Messages.Messages.Delete(name, type), new ToastrOptions { Title = "Rol Silme" });
			}
			else
			{
				toast.AddErrorToastMessage(Messages.Messages.DeleteError(name, type),new ToastrOptions { Title = "Rol Silme" });

			}
			return RedirectToAction("Index");
		}
	}
}
