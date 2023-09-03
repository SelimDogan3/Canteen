using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cantin.Web.Controllers
{
	[Authorize]
	public class HomeController : Controller
	{
		
		public IActionResult Index()
		{
			var s = ";"
			return View();
		}
	}
}
