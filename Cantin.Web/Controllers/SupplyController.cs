using Cantin.Entity.Dtos.Supplies;
using Cantin.Service.Services.Abstraction;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;

namespace Cantin.Web.Controllers
{
	[Authorize]
	public class SupplyController : Controller
	{
		private readonly ILogger<SupplyController> logger;
		private readonly IToastNotification toast;
		private readonly ISupplyService stockService;
		private readonly string type = "tedarik";


		public SupplyController(ILogger<SupplyController> logger, ISupplyService stockService, IToastNotification toast)
		{
			this.logger = logger;
			this.stockService = stockService;
			this.toast = toast;
		}
		public async Task<IActionResult> Index()
		{
			var supplies = await stockService.GetAllSuppliesNonDeleted();
			return View(supplies);
		}
		[HttpGet]
		public async Task<IActionResult> Add()
		{
			var dto = await stockService.GetSupplyAddDtoAsync();
			return View(dto);
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Add(SupplyAddDto addDto)
		{
			if (ModelState.IsValid)
			{
				await stockService.AddSupplyAsync(addDto);
				toast.AddSuccessToastMessage($"{addDto.Supplier} tarafından alınan tedarik başarıyla eklenmiştir");
				return RedirectToAction("Index");
			}
			toast.AddErrorToastMessage($"{addDto.Supplier} tarafından alınan tedarik eklenirken bir sorun oluştu");
			return View(addDto);
		}
		[HttpGet]
		public async Task<IActionResult> Update(Guid Id)
		{
			var dto = await stockService.GetSupplyUpdateDto(Id);
			return View(dto);
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Update(SupplyUpdateDto updateDto)
		{
			if (ModelState.IsValid)
			{
				await stockService.UpdateSupplyAsync(updateDto);
				toast.AddSuccessToastMessage($"{updateDto.Supplier} tarafından alınan tedarik başarıyla güncellenmiştir");
				return RedirectToAction("Index");
			}
			toast.AddErrorToastMessage($"{updateDto.Supplier} tarafından alınan tedarik güncellenirken bir sorun oluştu ");
			return View(updateDto);
		}
		public async Task<IActionResult> Delete(Guid Id)
		{
			var name = await stockService.DeleteStockAsync(Id);
			toast.AddSuccessToastMessage($"{name} tarafından alınan tedarik başarıyla silinmiştir");

			return RedirectToAction("Index");
		}
	}
}
