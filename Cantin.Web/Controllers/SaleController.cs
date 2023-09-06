using AutoMapper;
using Cantin.Entity.Dtos.Sales;
using Cantin.Service.Extensions;
using Cantin.Service.Services.Abstraction;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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
			ClaimsPrincipal _user = HttpContext.User;
			List<SaleDto> sales;
			if (_user.GetRole() == "Employee")
			{
				sales = await saleService.GetSalesForEmployeeAsync();
			}
			else {
				sales = await saleService.GetAllSalesNonDeletedAsync();
			}
			return View(sales);
		}
		[HttpGet]
		public async Task<IActionResult> Add()
		{
			
			var dto = await saleService.GetSaleAddDtoAsync();
			return View(dto);
		}
		[HttpPost]
		public async Task<IActionResult> Add([FromBody]SaleAddDto addDto) {
			await saleService.AddSaleAsync(addDto);
			return Ok();

		}
	}
}
