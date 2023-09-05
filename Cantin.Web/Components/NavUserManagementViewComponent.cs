using Cantin.Entity.Entities;
using Cantin.Service.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Xml.Linq;

namespace Cantin.Web.Components
{
	public class NavUserManagementViewComponent : ViewComponent
	{
		private readonly UserManager<AppUser> userManager;

		public NavUserManagementViewComponent(UserManager<AppUser> userManager)
		{
			this.userManager = userManager;
		}
		public async Task<IViewComponentResult> InvokeAsync()
		{
			AppUser? user = await userManager.FindByEmailAsync(HttpContext.User?.GetLoggedInUserEmail());
			return View("_NavUserManagement", user); 
		}
	}
}
