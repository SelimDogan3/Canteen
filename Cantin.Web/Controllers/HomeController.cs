using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cantin.Web.Controllers
{
    [Authorize()]
    public class HomeController : Controller
	{
        private readonly ILogger<HomeController> logger;

        public HomeController(ILogger<HomeController> logger)
        {
            this.logger = logger;
        }
        [HttpGet]
		public IActionResult Index()
		{
			return View();
		}
	}
}
