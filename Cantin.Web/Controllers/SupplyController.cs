using Cantin.Entity.Dtos.Supplies;
using Cantin.Service.Services.Abstraction;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;

namespace Cantin.Web.Areas.Admin.Controllers
{
    [Authorize(
		Policy ="AdminOnly")]


    public class SupplyController : Controller
	{
		private readonly ILogger<SupplyController> logger;
		private readonly IToastNotification toast;
		private readonly IProductService productService;
		private readonly IStoreService storeService;
		private readonly ISupplyService supplyService;
		private readonly string type = "tedarik";


		public SupplyController(ILogger<SupplyController> logger, ISupplyService supplyService, IToastNotification toast,IProductService productService,IStoreService storeService)
		{
			this.logger = logger;
			this.supplyService = supplyService;
			this.toast = toast;
			this.productService = productService;
			this.storeService = storeService;
		}
		public async Task<IActionResult> Index()
		{
			var supplies = await supplyService.GetAllSuppliesNonDeleted();
			return View(supplies);
		}
		[HttpGet]
		public async Task<IActionResult> Add()
		{
			var dto = await supplyService.GetSupplyAddDtoAsync();
			return View(dto);
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Add(SupplyAddDto addDto)
		{
			await supplyService.ValidateSupplyAsync(addDto, ModelState);
			if (ModelState.IsValid)
			{
				await supplyService.AddSupplyAsync(addDto);
				toast.AddSuccessToastMessage($"{addDto.Supplier} tarafından alınan tedarik başarıyla eklenmiştir", new ToastrOptions { Title = "Tedarik Ekleme" });
				return RedirectToAction("Index");
			}
			toast.AddErrorToastMessage($"{addDto.Supplier} tarafından alınan tedarik eklenirken bir sorun oluştu", new ToastrOptions { Title = "Tedarik Ekleme" });
			addDto.Products = await productService.GetAllProductsNonDeletedAsync();
			addDto.Stores = await storeService.GetAllStoreDtosNonDeleted();
			return View(addDto);
		}
		[HttpGet]
		public async Task<IActionResult> Update(Guid Id)
		{
			var dto = await supplyService.GetSupplyUpdateDto(Id);
			return View(dto);
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Update(SupplyUpdateDto updateDto)
		{
			await supplyService.ValidateSupplyAsync(updateDto, ModelState);
			if (ModelState.IsValid)
			{
				await supplyService.UpdateSupplyAsync(updateDto);
				toast.AddSuccessToastMessage($"{updateDto.Supplier} tarafından alınan tedarik başarıyla güncellenmiştir", new ToastrOptions { Title = "Tedarik Güncelleme" });
				return RedirectToAction("Index");
			}
			toast.AddErrorToastMessage($"{updateDto.Supplier} tarafından alınan tedarik güncellenirken bir sorun oluştu ", new ToastrOptions { Title = "Tedarik Güncelleme" });
			updateDto.Products = await productService.GetAllProductsNonDeletedAsync();

			return View(updateDto);
		}
		public async Task<IActionResult> Delete(Guid Id)
		{
			var name = await supplyService.DeleteStockAsync(Id);
			toast.AddSuccessToastMessage($"{name} tarafından alınan tedarik başarıyla silinmiştir", new ToastrOptions { Title = "Tedarik Silme" });

			return RedirectToAction("Index");
		}
	}
}
