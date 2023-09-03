using AutoMapper;
using Cantin.Entity.Dtos.Sales;
using Cantin.Service.Services.Abstraction;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cantin.Web.Controllers
{
	[Authorize]
	public class SaleController : Controller
	{
		private readonly ILogger<SaleController> logger;
		private readonly ISaleService saleService;
		private readonly IMapper mapper;
		private readonly IProductService productService;

		public SaleController(ILogger<SaleController> logger,ISaleService saleService,IMapper mapper,IProductService productService)
        {
			this.logger = logger;
			this.saleService = saleService;
			this.mapper = mapper;
			this.productService = productService;
		}
		public async Task<IActionResult> GetProductByBarcode(string barCode) {
			var product = await productService.GetProductByBarcodeAsync(barCode);
			return Ok(product);
		}
        public async Task<IActionResult> Index()
		{
			var sales = await saleService.GetAllSalesNonDeletedAsync();
			return View(sales);
		}
		[HttpGet]
		public async Task<IActionResult> Add()
		{
			
			var dto = saleService.GetSaleAddDto();
			return View(dto);
		}
		[HttpPost]
		public async Task<IActionResult> Add([FromBody]SaleAddDto addDto) {
			await saleService.AddSaleAsync(addDto);
			return Ok();

		}
	}
}
