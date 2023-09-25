using AutoMapper;
using Cantin.Data.Filters;
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
using System.Linq.Expressions;
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
			_user = contextAccessor.HttpContext!.User;

		}
		public async Task<List<SaleDto>> GetAllSalesNonDeletedAsync(SaleFilterDto? filterDto = null)
		{
			Expression<Func<Sale,bool>> query;
            if (filterDto != null) { 
			query = await SaleFilter.FilterAsync(filterDto);
            }
			else { 
				query = x => !x.IsDeleted;
            }
            List<Sale> sales = await repository.GetAllAsync(query, x => x.Store,x => x.SaleProducts);
			List<SaleDto> map = mapper.Map<List<SaleDto>>(sales);
			foreach (var saleDto in map)
			{
				saleDto.ProductLines = await GetProductLineWithSaleId(saleDto.Id);
			}
			return map;
		}
		public async Task<List<SaleDto>> GetSalesForEmployeeAsync()
		{
			var user = await userManager.FindByEmailAsync(_user.GetLoggedInUserEmail());
			//24 saate göre hesaplar DateTime.Compare(x.CreatedDate.AddDays(1.0), DateTime.Now) > 0
			List<Sale> sales = await repository.GetAllAsync(x => !x.IsDeleted & x.CreatedDate.Day == DateTime.Now.Day & x.StoreId == user.StoreId , x => x.Store);
			var map = mapper.Map<List<SaleDto>>(sales);
			foreach (var saleDto in map)
			{
				saleDto.ProductLines = await GetProductLineWithSaleId(saleDto.Id);
			}
			return map;
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
			await stockService.UpdateStockAsync(sale);
			await unitOfWork.SaveAsync();
		}
		public async Task MatchProductWithSaleAsync(Guid saleId, Guid productId, int quantity)
		{
			SaleProduct saleProduct = new SaleProduct() { SaleId = saleId, ProductId = productId, Quantity = quantity};
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
				ProductLine line = new() { Product = dto, Quantity = saleProduct.Quantity};
				lines.Add(line);
			}
			return lines;
		}
	}
}
