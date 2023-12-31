﻿using AutoMapper;
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
        private readonly IStoreService storeService;

        public SaleController(ILogger<SaleController> logger,ISaleService saleService,IMapper mapper,IProductService productService, IStoreService storeService)
        {
			this.logger = logger;
			this.saleService = saleService;
			this.mapper = mapper;
			this.productService = productService;
            this.storeService = storeService;
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
			var products = await productService.GetAllProductsNonDeletedAsync();
			var stores = await storeService.GetAllStoreDtosNonDeleted();
			ViewBag.Products = products;
			ViewBag.Stores = stores;
			return View(sales);
		}
		[HttpGet]
        public async Task<IActionResult> Add()
		{
			ViewBag.MostUsedProducts = await productService.GetMostUsedProductsAsync();
			return View();
		}
		[HttpPost]
        public async Task<IActionResult> Add([FromBody]SaleAddDto addDto) {
			await saleService.AddSaleAsync(addDto);
			return Ok();
			//s
		}
		[HttpPost]
        [Authorize(
        Policy = "AdminOnly")]
        public async Task<ActionResult<SaleDto>> GetWithFilter([FromBody] SaleFilterDto filterDto) {
			var sales = await saleService.GetAllSalesNonDeletedAsync(filterDto);
			return Json(sales);
		}
        [Authorize(
        Policy = "AdminOnly")]
        public async Task<IActionResult> Detail(Guid Id)
        {
			var sale = await saleService.GetSaleByIdAsync(Id);
            return View(sale);
        }
    }
}
