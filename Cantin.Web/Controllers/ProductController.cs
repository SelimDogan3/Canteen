using Cantin.Entity.Dtos.Products;
using Cantin.Service.Services.Abstraction;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;

namespace Cantin.Web.Controllers
{
	[Authorize(
        Policy = "AdminOnly"
        )]
	public class ProductController : Controller
	{
		private readonly ILogger<ProductController> logger;
		private readonly IProductService productService;
		private readonly IToastNotification toastNotification;
		private readonly string type = "ürün";

		public ProductController(ILogger<ProductController> logger, IProductService productService, IToastNotification toastNotification)
		{
			this.logger = logger;
			this.productService = productService;
			this.toastNotification = toastNotification;
		}
		public async Task<IActionResult> Index()
		{
			var products = await productService.GetAllProductsNonDeletedAsync();
			return View(products);
		}
		[HttpGet]
		public IActionResult Add()
		{
			var productAddDto = productService.GetProductAddDto();
			return View(productAddDto);
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Add([FromForm] ProductAddDto addDto)
		{
			await productService.ValidateProductAsync(addDto, ModelState); 
			if (ModelState.IsValid)
			{
				string name = await productService.AddProductAsync(addDto);
				toastNotification.AddSuccessToastMessage(Messages.Messages.Add(name, type));
				return RedirectToAction("Index");
			}
			toastNotification.AddErrorToastMessage(Messages.Messages.AddError(addDto.Name, type));

			return View(addDto);
		}
		[HttpGet]
		public async Task<IActionResult> Update(Guid Id)
		{
			var productAddDto = await productService.GetProductUpdateDtoAsync(Id);
			return View(productAddDto);
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Update(ProductUpdateDto updateDto)
		{
			await productService.ValidateProductAsync(updateDto, ModelState);
			if (ModelState.IsValid)
			{
				string name = await productService.UpdateProductAsync(updateDto);
				toastNotification.AddSuccessToastMessage(Messages.Messages.Update(name, type));
				return RedirectToAction("Index");
			}
			toastNotification.AddErrorToastMessage(Messages.Messages.UpdateError(updateDto.Name, type));
			return View(updateDto);
		}
		public async Task<IActionResult> Delete(Guid Id)
		{
			string name = await productService.DeleteProductByIdAsync(Id);
			toastNotification.AddSuccessToastMessage(Messages.Messages.Delete(name, type));
			return RedirectToAction("Index");
		}
	}
}
