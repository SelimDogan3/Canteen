using AutoMapper;
using Cantin.Data.Repository.Abstract;
using Cantin.Data.UnitOfWorks;
using Cantin.Entity.Dtos.Products;
using Cantin.Entity.Dtos.Stores;
using Cantin.Entity.Dtos.Supplies;
using Cantin.Entity.Entities;
using Cantin.Service.Extensions;
using Cantin.Service.Services.Abstraction;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Cantin.Service.Services.Concrete
{
    public class SupplyService : ISupplyService
	{
		private readonly IUnitOfWork unitOfWork;
		private readonly IMapper mapper;
		private readonly IProductService productService;
		private readonly IStoreService storeService;
		private readonly IStockService stockService;
		private readonly ClaimsPrincipal _user;
		private IRepository<Supply> repository { get => unitOfWork.GetRepository<Supply>(); }

		public SupplyService(IUnitOfWork unitOfWork,IMapper mapper,IHttpContextAccessor contextAccessor,IProductService productService,IStoreService storeService,IStockService stockService)
        {
			this.unitOfWork = unitOfWork;
			this.mapper = mapper;
			this.productService = productService;
			this.storeService = storeService;
			this.stockService = stockService;
			_user = contextAccessor.HttpContext!.User;
		}
        public async Task<List<SupplyDto>> GetAllSuppliesNonDeleted()
		{
			List<Supply> supplies = await repository.GetAllAsync(x => !x.IsDeleted,x => x.Product,x => x.Store);
			List<SupplyDto> map = mapper.Map<List<SupplyDto>>(supplies);
			return map;
		}
		public async Task<Supply> GetSupplyById(Guid Id)
		{
			Supply supply = await repository.GetByGuidAsync(Id);
			return supply;
		}

		public async Task<SupplyAddDto> GetSupplyAddDtoAsync() {
			List<ProductDto> products = await productService.GetAllProductsNonDeletedAsync();
			List<StoreDto> stores = await storeService.GetAllStoreDtosNonDeleted();
			SupplyAddDto dto = new() {Products = products,Stores = stores };
			return dto;
		}
		
		public async Task<string> AddSupplyAsync(SupplyAddDto addDto)
		{
			Supply supply = mapper.Map<Supply>(addDto);
			supply.CreatedBy = _user.GetLoggedInUserEmail();
			await repository.AddAsync(supply);
			await stockService.AddStockToStoreAsync(supply);
			await unitOfWork.SaveAsync();
			return supply.Supplier;
		}
		public async Task<SupplyUpdateDto> GetSupplyUpdateDto(Guid Id)
		{
			Supply supply = await GetSupplyById(Id);
			SupplyUpdateDto dto = mapper.Map<SupplyUpdateDto>(supply);
			List<ProductDto> products = await productService.GetAllProductsNonDeletedAsync();
			dto.Products = products;
			return dto;
		}

		public async Task<string> UpdateSupplyAsync(SupplyUpdateDto updateDto)
		{
			Supply supply = await GetSupplyById(updateDto.Id);
			Supply newSupply = mapper.Map(updateDto, supply);
			newSupply.ModifiedBy = _user.GetLoggedInUserEmail();
			newSupply.ModifiedDate = DateTime.Now;
			await repository.UpdateAsync(newSupply);
			await unitOfWork.SaveAsync();
			return updateDto.Supplier;
		}
		public async Task<string> DeleteStockAsync(Guid Id)
		{
			Supply supply = await GetSupplyById(Id);
			supply.IsDeleted = true;
			supply.DeletedDate = DateTime.Now;
			supply.DeletedBy = _user.GetLoggedInUserEmail();
			await repository.UpdateAsync(supply);
			await unitOfWork.SaveAsync();
			return supply.Supplier;
		}
	}
}
