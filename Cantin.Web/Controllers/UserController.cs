using AutoMapper;
using Cantin.Data.Identity;
using Cantin.Entity.Dtos.Users;
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


    public class UserController : Controller
    {
        private readonly UserManager<AppUser> userManager;
        private readonly ILogger<UserController> logger;
        private readonly IUserService userService;
        private readonly IMapper mapper;
        private readonly IToastNotification toastNotification;
        private readonly string type = "kullanıcı";

        public UserController(ILogger<UserController> logger, IUserService userService, UserManager<AppUser> userManager, IMapper mapper, IToastNotification toastNotification)
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
        public async Task<IActionResult> Add(UserAddDto addDto)
        {
            await userService.ValidateUserAsync(addDto, ModelState);
            if (ModelState.IsValid)
            {
                var result = await userService.AddUserWithRoleAsync(addDto);
                if (result.Succeeded)
                {
                    toastNotification.AddSuccessToastMessage(Messages.Messages.Add(addDto.FullName, type), new ToastrOptions { Title = "Kullanıcı Ekleme" });
                    return RedirectToAction("Index");
                }
                else
                {
                    foreach (ModelAddIdentityError error in result.Errors.Cast<ModelAddIdentityError>())
                    {
                        ModelState.AddModelError(error.key, error.Description);

                    }
                    toastNotification.AddErrorToastMessage(Messages.Messages.AddError(addDto.FullName, type), new ToastrOptions { Title = "Kullanıcı Ekleme" });

                    addDto = await userService.GetUserAddDto();
                    return View(addDto);
                }
            }
            toastNotification.AddErrorToastMessage(Messages.Messages.AddError(addDto.FullName, type), new ToastrOptions { Title = "Kullanıcı Ekleme" });
            addDto = await userService.GetUserAddDto();
            return View(addDto);
        }
        [HttpGet]
        public async Task<IActionResult> Update(Guid id)
        {
            var dto = await userService.GetUpdateUserDto(id);
            return View(dto);
        }
        [HttpPost]
        public async Task<IActionResult> Update(UserUpdateDto updateDto)
        {
            await userService.ValidateUserAsync(updateDto, ModelState);
            if (ModelState.IsValid)
            {
                IdentityResult result = await userService.UpdateUserWithRoleAsync(updateDto);
                if (result.Succeeded)
                {
                    toastNotification.AddSuccessToastMessage(Messages.Messages.Update(updateDto.FullName, type), new ToastrOptions { Title = "Kullanıcı Güncelleme" });
                    return RedirectToAction("Index");
                }
                else
                {
                    foreach (ModelAddIdentityError error in result.Errors.Cast<ModelAddIdentityError>())
                    {
                        ModelState.AddModelError(error.key, error.Description);

                    }
                    toastNotification.AddErrorToastMessage(Messages.Messages.UpdateError(updateDto.FullName, type), new ToastrOptions { Title = "Kullanıcı Güncelleme" });
                    updateDto = await userService.GetUpdateUserDto(updateDto.Id);
                    return View(updateDto);

                }

            }
            toastNotification.AddErrorToastMessage(Messages.Messages.UpdateError(updateDto.FullName, type), new ToastrOptions { Title = "Kullanıcı Güncelleme" });
            updateDto = await userService.GetUpdateUserDto(updateDto.Id);
            return View(updateDto);
        }
        public async Task<IActionResult> Delete([FromRoute] Guid Id, [FromForm] string password)
        {
            if (password != null)
            {
                var name = (await userManager.FindByIdAsync(Id.ToString())).FullName;
                IdentityResult result = await userService.DeleteUserAsync(Id, password);
                if (result.Succeeded)
                {
                    toastNotification.AddSuccessToastMessage(Messages.Messages.Delete(name, type), new ToastrOptions { Title = "Kullanıcı Silme" });
                }
                else
                {
                    toastNotification.AddErrorToastMessage(Messages.Messages.DeleteError(name, type), new ToastrOptions { Title = "Kullanıcı Silme" });

                }
            }
            else
                toastNotification.AddErrorToastMessage("Yönetici Şifresi Alanı Boş Geçilemez", new ToastrOptions { Title = "Kullanıcı Silme" });
            return RedirectToAction("Index");


        }
    }
}
