using Cantin.Entity.Dtos.ManuelStockReduction;
using Cantin.Entity.Dtos.Stores;
using Cantin.Service.Extensions;
using Cantin.Service.Services.Abstraction;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;

namespace Cantin.Web.Areas.Admin.Controllers
{
    public class StoreController : Controller
    {
        private readonly ILogger<StoreController> logger;
        private readonly IStoreService storeService;
        private readonly IStockService stockService;
        private readonly IToastNotification toastMessage;
        private readonly IUserService userService;
        private readonly string type = "Mağaza";

        public StoreController(ILogger<StoreController> logger, IStoreService storeService, IStockService stockService, IToastNotification toastMessage, IUserService userService)
        {
            this.logger = logger;
            this.storeService = storeService;
            this.stockService = stockService;
            this.toastMessage = toastMessage;
            this.userService = userService;
        }
        [Authorize(
        Policy = "AdminOnly"
        )]
        public async Task<IActionResult> Index()
        {
            var stores = await storeService.GetAllStoreDtosNonDeleted();
            return View(stores);
        }
        [Authorize(
        Policy = "AdminOnly"
        )]
        [HttpGet]
        public IActionResult Add()
        {
            var dto = storeService.GetStoreAddDto();
            return View(dto);
        }
        [Authorize(
        Policy = "AdminOnly"
        )]
        [HttpPost]
        public async Task<IActionResult> Add(StoreAddDto dto)
        {
            await storeService.ValidateStoreAsync(dto, ModelState);
            if (ModelState.IsValid)
            {
                var name = await storeService.AddStoreAsync(dto);
                toastMessage.AddSuccessToastMessage(Messages.Messages.Add(name, type), new ToastrOptions { Title = "Mağaza Ekleme" });
                return RedirectToAction("Index");
            }
            toastMessage.AddErrorToastMessage(Messages.Messages.AddError(dto.Name, type), new ToastrOptions { Title = "Mağaza Ekleme" });
            return View(dto);
        }
        [Authorize(
        Policy = "AdminOnly"
        )]
        [HttpGet]
        public async Task<IActionResult> Update(Guid Id)
        {
            var dto = await storeService.GetStoreUpdateDtoByIdAsync(Id);
            return View(dto);
        }
        [Authorize(
        Policy = "AdminOnly"
        )]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(StoreUpdateDto dto)
        {
            if (ModelState.IsValid)
            {
                var name = await storeService.UpdateStoreAsync(dto);
                toastMessage.AddSuccessToastMessage(Messages.Messages.Update(name, type), new ToastrOptions { Title = "Mağaza Güncelleme" });

                return RedirectToAction("Index");
            }
            toastMessage.AddErrorToastMessage(Messages.Messages.UpdateError(dto.Name, type), new ToastrOptions { Title = "Mağaza Güncelleme" });
            return View(dto);
        }
        [Authorize(
        Policy = "AdminOnly"
        )]
        public async Task<IActionResult> Delete(Guid Id)
        {
            var name = await storeService.DeleteStoreAsyncById(Id);
            toastMessage.AddSuccessToastMessage(Messages.Messages.Delete(name, type), new ToastrOptions { Title = "Mağaza Silme" });
            return RedirectToAction("Index");
        }
        [Authorize(
        Policy = "AdminOnly"
        )]
        public async Task<IActionResult> Stock()
        {
            List<StockDto> stocks = await stockService.GetAllStocksIncludingStores();
            return View(stocks);
        }
        [Authorize]
        public async Task<IActionResult> StockDetail([FromRoute] Guid Id)
        {
            var role = HttpContext.User.GetRole();
            var user = await userService.GetUserByEmail(HttpContext.User.GetLoggedInUserEmail());
            if ((role != "Superadmin" && role != "Admin") && user.StoreId != Id)
            {
                return RedirectToAction("AccessDenied", "Auth");
            }
            StockDto stock = await stockService.GetStocksOfAnStoreAsync(Id);
            return View("StockDetail", stock);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DecreaseProduct(ManuelStockReductionAddDto dto)
        {
            if (ModelState.IsValid)
            {
                var result = await stockService.UpdateStockAsync(dto); //updating stock
                if (result == "")
                {
                    toastMessage.AddSuccessToastMessage("Ürün Stoğu Başarı ile azaltıldı", new ToastrOptions { Title = "Stok Bilgisi" });
                }
                else
                {
                    toastMessage.AddErrorToastMessage(result, new ToastrOptions { Title = "Stok Bilgisi" });
                }
            }
            else
            {
                toastMessage.AddErrorToastMessage("Stok güncellenirken bir hata ile karşılaşıldı", new ToastrOptions { Title = "Stok Bilgisi" });

            }
            return RedirectToAction("StockDetail", new { Id = dto.StoreId });

        }
    }
}
