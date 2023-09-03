using AutoMapper;
using Cantin.Data.Identity;
using Cantin.Entity.Dtos.Users;
using Cantin.Entity.Entities;
using Cantin.Service.Services.Abstraction;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;

namespace Cantin.Web.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private readonly UserManager<AppUser> userManager;
        private readonly ILogger<UserController> logger;
        private readonly IUserService userService;
        private readonly IMapper mapper;
        private readonly IToastNotification toastNotification;
        private readonly string type = "kullanıcı";

        public UserController(ILogger<UserController> logger, IUserService userService, UserManager<AppUser> userManager, IMapper mapper,IToastNotification toastNotification)
        {
            this.userManager = userManager;
            this.logger = logger;
            this.userService = userService;
            this.mapper = mapper;
            this.toastNotification = toastNotification;
        }


        public async Task<IActionResult> Index()
        {
            var users = await userService.GetAllUsersWithRolesAndStoresAsync();
            return View(users);
        }
        [HttpGet]
        public async Task<IActionResult> Add()
        {
            var dto = await userService.GetUserAddDto();
            return View(dto);
        }
        [HttpPost]
        public async Task<IActionResult> Add(UserAddDto dto)
        {
            if (ModelState.IsValid)
            {
                var result = await userService.AddUserWithRoleAsync(dto);
                if (result.Succeeded) {
                    toastNotification.AddSuccessToastMessage(Messages.Messages.Add(dto.FullName,type));
                    return RedirectToAction("Index");
                }
                else { 
                foreach (ModelAddIdentityError error in result.Errors.Cast<ModelAddIdentityError>())
                {
                    ModelState.AddModelError(error.key,error.Description);

                }
                    toastNotification.AddErrorToastMessage(Messages.Messages.AddError(dto.FullName, type));

                    dto = await userService.GetUserAddDto();
                return View(dto);
                }
            }
            toastNotification.AddErrorToastMessage(Messages.Messages.AddError(dto.FullName, type));
            dto = await userService.GetUserAddDto();
            return View(dto);
        }
        [HttpGet]
        public async Task<IActionResult> Update(Guid id) {
            var dto = await userService.GetUpdateUserDto(id);
            return View(dto);
        }
        [HttpPost]
        public async Task<IActionResult> Update(UserUpdateDto dto)
        {
            if (ModelState.IsValid) {
                IdentityResult result = await userService.UpdateUserWithRoleAsync(dto);
                if (result.Succeeded)
                {
                    toastNotification.AddSuccessToastMessage(Messages.Messages.Update(dto.FullName, type));
                    return RedirectToAction("Index");
                }
                else {
					foreach (ModelAddIdentityError error in result.Errors.Cast<ModelAddIdentityError>())
					{
						ModelState.AddModelError(error.key, error.Description);

					}
					toastNotification.AddErrorToastMessage(Messages.Messages.UpdateError(dto.FullName, type));
                    dto = await userService.GetUpdateUserDto(dto.Id);
                        return View(dto);

                }
                
            }
            toastNotification.AddErrorToastMessage(Messages.Messages.UpdateError(dto.FullName, type));
            dto = await userService.GetUpdateUserDto(dto.Id);
            return View(dto);
        }
        public async Task<IActionResult> Delete(Guid Id) {
            var name =(await userManager.FindByIdAsync(Id.ToString())).FullName;
            IdentityResult result = await userService.DeleteUserAsync(Id);
            if (result.Succeeded)
            {
                toastNotification.AddSuccessToastMessage(Messages.Messages.Delete(name, type));


            }
            else {
                toastNotification.AddErrorToastMessage(Messages.Messages.DeleteError(name, type));

            }
            return RedirectToAction("Index");
        }
    }
}
