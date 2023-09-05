using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cantin.Web.Controllers
{
    [Authorize()]
    public class HomeController : Controller
	{
		[HttpGet]
		public IActionResult Index()
		{
			return View();
		}
	}
}
