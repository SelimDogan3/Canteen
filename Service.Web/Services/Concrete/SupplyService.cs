using AutoMapper;
using Cantin.Data.Repository.Abstract;
using Cantin.Data.UnitOfWorks;
using Cantin.Entity.Dtos.Products;
using Cantin.Entity.Dtos.Stores;
using Cantin.Entity.Dtos.Supplies;
using Cantin.Entity.Entities;
using Cantin.Service.Extensions;
using Cantin.Service.Services.Abstraction;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
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
		private readonly IValidator<Supply> validator;
		private readonly ClaimsPrincipal _user;
		private IRepository<Supply> repository { get => unitOfWork.GetRepository<Supply>(); }

		public SupplyService(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor contextAccessor, IProductService productService, IStoreService storeService, IStockService stockService, IValidator<Supply> validator)
		{
			this.unitOfWork = unitOfWork;
			this.mapper = mapper;
			this.productService = productService;
			this.storeService = storeService;
			this.stockService = stockService;
			this.validator = validator;
			_user = contextAccessor.HttpContext!.User;
		}
		public async Task<List<SupplyDto>> GetAllSuppliesNonDeleted()
		{
			List<Supply> supplies = await repository.GetAllAsync(x => !x.IsDeleted, x => x.Product, x => x.Store);
			var dtos = new List<SupplyDto>();
            foreach (var supply in supplies)
            {
				var match = dtos.FirstOrDefault(x => x.Supplier == supply.Supplier);
				if (match != null)
				{
					var line = mapper.Map<SupplyLine>(supply);
					match.SupplyLines.Add(line);
				}
				else {
					var line = mapper.Map<SupplyLine>(supply);
                    var dto = new SupplyDto {Supplier = supply.Supplier};
					dto.SupplyLines.Add(line);
					dtos.Add(dto);
				}
            }
			return dtos;

        }
		public async Task<Supply> GetSupplyById(Guid Id)
		{
			Supply supply = await repository.GetByGuidAsync(Id);
			return supply;
		}

		public async Task<SupplyAddDto> GetSupplyAddDtoAsync()
		{
			List<ProductDto> products = await productService.GetAllProductsNonDeletedAsync();
			List<StoreDto> stores = await storeService.GetAllStoreDtosNonDeleted();
			SupplyAddDto dto = new() { Products = products, Stores = stores };
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

		public async Task ValidateSupplyAsync(SupplyAddDto dto, ModelStateDictionary modelState)
		{
			Supply supply = mapper.Map<Supply>(dto);
			ValidationResult result = await validator.ValidateAsync(supply);
			if (!result.IsValid)
			{
				result.AddErrorsToModelState(modelState);
			}
		}

		public async Task ValidateSupplyAsync(SupplyUpdateDto dto, ModelStateDictionary modelState)
		{
			Supply supply = mapper.Map<Supply>(dto);
			ValidationResult result = await validator.ValidateAsync(supply);
			if (!result.IsValid)
			{
				result.AddErrorsToModelState(modelState);
			}
		}
	}
}
