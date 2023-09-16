using Cantin.Entity.Dtos.Stores;
using Cantin.Service.Services.Abstraction;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;

namespace Cantin.Web.Areas.Admin.Controllers
{
    [Authorize(
		Policy = "AdminOnly"
		)]


    public class StoreController : Controller
	{
		private readonly ILogger<StoreController> logger;
		private readonly IStoreService storeService;
		private readonly IStockService stockService;
		private readonly IToastNotification toastMessage;
		private readonly string type = "Mağaza";

		public StoreController(ILogger<StoreController> logger, IStoreService storeService, IStockService stockService, IToastNotification toastMessage)
		{
			this.logger = logger;
			this.storeService = storeService;
			this.stockService = stockService;
			this.toastMessage = toastMessage;
		}
		public async Task<IActionResult> Index()
		{
			var stores = await storeService.GetAllStoreDtosNonDeleted();
			return View(stores);
		}
		[HttpGet]
		public IActionResult Add()
		{
			var dto = storeService.GetStoreAddDto();
			return View(dto);
		}
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
		[HttpGet]
		public async Task<IActionResult> Update(Guid Id)
		{
			var dto = await storeService.GetStoreUpdateDtoByIdAsync(Id);
			return View(dto);
		}
		[HttpPost]
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
		public async Task<IActionResult> Delete(Guid Id)
		{
			var name = await storeService.DeleteStoreAsyncById(Id);
			toastMessage.AddSuccessToastMessage(Messages.Messages.Delete(name, type), new ToastrOptions { Title = "Mağaza Silme" });
			return RedirectToAction("Index");
		}
		public async Task<IActionResult> Stock()
		{
			List<StockDto> stocks = await stockService.GetAllStocksIncludingStores();
			return View(stocks);
		}
	}
}
