using Cantin.Entity.Dtos.Users;
using Cantin.Entity.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;

namespace Cantin.Web.Controllers
{
    public class AuthController : Controller
    {
        private readonly ILogger<AuthController> logger;
        private readonly IToastNotification toast;
        private readonly UserManager<AppUser> userManager;
        private readonly SignInManager<AppUser> signInManager;
		public AuthController(ILogger<AuthController> logger, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IToastNotification toast)
		{
			this.logger = logger;
			this.userManager = userManager;
			this.signInManager = signInManager;
			this.toast = toast;
		}
		[HttpGet]
        public IActionResult Login(string? returnUrl) {
            TempData["returnUrl"] = returnUrl;
            return View(); 
        }
        [HttpPost]
        public async Task<IActionResult> Login(UserLoginDto dto)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByEmailAsync(dto.Email);
                if (user != null)
                {
                    var result = await signInManager.PasswordSignInAsync(user, dto.Password, dto.RememberMe, true);
                    if (result.Succeeded)
                    {
                        await userManager.ResetAccessFailedCountAsync(user);
                        toast.AddInfoToastMessage(user.FullName+" adlı kullanıcı başarıyla giriş yaptı",new ToastrOptions{Title="Oturum Bilgisi"});
                        return Redirect(TempData["returnUrl"] == null || TempData["returnUrl"]?.ToString() == String.Empty ? "/Home/Index" : TempData["returnUrl"]!.ToString()!);
                    }
                }
                
            }
            ModelState.AddModelError("","Mailiniz veye şifreniz yanlış tekrar kontrol ediniz");
			toast.AddInfoToastMessage("Giriş Yapmaya çalışırken bir hata oluştu", new ToastrOptions { Title = "Oturum Bilgisi" });
			return View();
		}
        [Authorize]
        public async Task<IActionResult> Logout() {
            await signInManager.SignOutAsync();
            return RedirectToAction("Login");
        }
        [Authorize]
        public IActionResult AccessDenied(string ReturnUrl) {
            toast.AddAlertToastMessage("Bu sayfaya erişme yetkiniz yoktur");
            return RedirectToAction("Index","Home");
        }
    }
}
