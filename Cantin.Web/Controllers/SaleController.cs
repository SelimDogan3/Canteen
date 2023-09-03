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
		private readonly ClaimsPrincipal _user;

		public SaleController(ILogger<SaleController> logger,ISaleService saleService,IMapper mapper,IProductService productService)
        {
			this.logger = logger;
			this.saleService = saleService;
			this.mapper = mapper;
			this.productService = productService;
			_user = HttpContext.User;
		}
		public async Task<IActionResult> GetProductByBarcode(string barCode) {
			var product = await productService.GetProductByBarcodeAsync(barCode);
			return Ok(product);
		}
        public async Task<IActionResult> Index()
		{
			List<SaleDto>? sales = default;
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
