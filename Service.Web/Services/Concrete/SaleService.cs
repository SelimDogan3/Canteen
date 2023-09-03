﻿using AutoMapper;
using Cantin.Data.Repository.Abstract;
using Cantin.Data.UnitOfWorks;
using Cantin.Entity.Dtos.Products;
using Cantin.Entity.Dtos.Sales;
using Cantin.Entity.Entities;
using Cantin.Service.Extensions;
using Cantin.Service.Services.Abstraction;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Globalization;
using System.Security.Claims;

namespace Cantin.Service.Services.Concrete
{
	public class SaleService : ISaleService
	{
		private readonly IUnitOfWork unitOfWork;
		private readonly IMapper mapper;
		private readonly IProductService productService;
		private readonly IStoreService storeService;
		private readonly UserManager<AppUser> userManager;
		private readonly IStockService stockService;
		private readonly ClaimsPrincipal _user;

		private IRepository<Sale> repository { get => unitOfWork.GetRepository<Sale>(); }

		public SaleService(IUnitOfWork unitOfWork, IMapper mapper, IProductService productService, IHttpContextAccessor contextAccessor, IStoreService storeService, UserManager<AppUser> userManager, IStockService stockService)
		{
			this.unitOfWork = unitOfWork;
			this.mapper = mapper;
			this.productService = productService;
			this.storeService = storeService;
			this.userManager = userManager;
			this.stockService = stockService;
			_user = contextAccessor.HttpContext.User;

		}
		public async Task<List<SaleDto>> GetAllSalesNonDeletedAsync()
		{
			List<Sale> sales = await repository.GetAllAsync(x => !x.IsDeleted, x => x.Store);
			List<SaleDto> map = mapper.Map<List<SaleDto>>(sales);
			foreach (var saleDto in map)
			{
				saleDto.ProductLines = await GetProductLineWithSaleId(saleDto.Id);
			}
			return map;
		}
		public SaleAddDto GetSaleAddDto()
		{
			SaleAddDto dto = new SaleAddDto();
			return dto;
		}
		public async Task AddSaleAsync(SaleAddDto dto)
		{
			Sale sale = mapper.Map<Sale>(dto);
			sale.CreatedBy = _user.GetLoggedInUserEmail();
			AppUser user = await userManager.FindByEmailAsync(_user.GetLoggedInUserEmail());
			sale.StoreId = user.StoreId;
			await repository.AddAsync(sale);
			foreach (var productLine in dto.ProductLines)
			{
				await MatchProductWithSaleAsync(sale.Id, productLine.ProductId, productLine.Quantity);
			}
			await stockService.UpdateStockForSaleAsync(dto, user.StoreId);
			await unitOfWork.SaveAsync();
		}
		public async Task MatchProductWithSaleAsync(Guid saleId, Guid productId, int quantity)
		{
			SaleProduct saleProduct = new SaleProduct() { SaleId = saleId, ProductId = productId, Quantity = quantity };
			saleProduct.CreatedBy = _user.GetLoggedInUserEmail();
			await unitOfWork.GetRepository<SaleProduct>().AddAsync(saleProduct);
		}
		public async Task<List<ProductLine>> GetProductLineWithSaleId(Guid saleId)
		{
			List<ProductLine> lines = new List<ProductLine>();
			List<SaleProduct> saleProducts = await unitOfWork.GetRepository<SaleProduct>().GetAllAsync(x => x.SaleId == saleId);
			foreach (var saleProduct in saleProducts)
			{
				var product = await productService.GetProductByIdAsync(saleProduct.ProductId);
				var dto = mapper.Map<ProductDto>(product);
				ProductLine line = new() { Product = dto, Quantity = saleProduct.Quantity, SubTotal = saleProduct.Quantity * decimal.Parse(dto.SalePrice, CultureInfo.InvariantCulture) };
				lines.Add(line);
			}
			return lines;
		}
	}
}
