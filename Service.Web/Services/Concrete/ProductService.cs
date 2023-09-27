using AutoMapper;
using Cantin.Data.Repository.Abstract;
using Cantin.Data.UnitOfWorks;
using Cantin.Entity.Dtos.Products;
using Cantin.Entity.Entities;
using Cantin.Service.Extensions;
using Cantin.Service.Services.Abstraction;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Collections.Generic;
using System.Security.Claims;

namespace Cantin.Service.Services.Concrete
{
	public class ProductService : IProductService
	{
		private readonly IUnitOfWork unitOfWork;
		private readonly IMapper mapper;
		private readonly IValidator<Product> validator;
		private readonly ClaimsPrincipal _user;

		private IRepository<Product> repository { get => unitOfWork.GetRepository<Product>(); }
		public ProductService(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor contextAccessor,IValidator<Product> validator)
		{
			this.unitOfWork = unitOfWork;
			this.mapper = mapper;
			this.validator = validator;
			_user = contextAccessor.HttpContext.User;
		}

		public async Task<List<ProductDto>> GetAllProductsNonDeletedAsync()
		{
			List<Product> products = await repository.GetAllAsync(x => !x.IsDeleted);
            List<ProductDto> map = mapper.Map<List<ProductDto>>(products);
			return map;

		}

		public async Task<Product> GetProductByIdAsync(Guid productId)
		{
			Product product = await repository.GetByGuidAsync(productId);
			return product;
		}
		public async Task<ProductDto> GetProductByBarcodeAsync(string barcode)
		{
			Product? product = await repository.GetAsync(x => x.Barcode == barcode);
			ProductDto map = mapper.Map<ProductDto>(product);
			return map;
		}
		public ProductAddDto GetProductAddDto() => new();


		public async Task<string> AddProductAsync(ProductAddDto addDto)
		{
			Product product = mapper.Map<Product>(addDto);
			product.CreatedBy = _user.GetLoggedInUserEmail();
			await repository.AddAsync(product);
			await unitOfWork.SaveAsync();
			return product.Name;
		}

		public async Task<ProductUpdateDto> GetProductUpdateDtoAsync(Guid Id)
		{
			Product product = await GetProductByIdAsync(Id);
			ProductUpdateDto dto = mapper.Map<ProductUpdateDto>(product);
			return dto;

		}

		public Task<ProductUpdateDto> GetProductUpdateDtoAsync(Product product)
		{
			ProductUpdateDto dto = mapper.Map<ProductUpdateDto>(product);
			return Task.FromResult(dto);

		}

		public async Task<string> UpdateProductAsync(ProductUpdateDto updateDto)
		{
			Product product = await GetProductByIdAsync(updateDto.Id);
			Product newProduct = mapper.Map(updateDto, product);
			newProduct.ModifiedBy = _user.GetLoggedInUserEmail();
			newProduct.ModifiedDate = DateTime.Now;
			await repository.UpdateAsync(newProduct);
			await unitOfWork.SaveAsync();
			return newProduct.Name;
		}

		public async Task<string> DeleteProductByIdAsync(Guid Id)
		{
			Product product = await GetProductByIdAsync(Id);
			product.IsDeleted = true;
			product.DeletedBy = _user.GetLoggedInUserEmail();
			product.DeletedDate = DateTime.Now;
			product.Barcode += "OLD";
			await repository.UpdateAsync(product);
			await unitOfWork.SaveAsync();
			return product.Name;
		}

		public async Task ValidateProductAsync(ProductAddDto dto, ModelStateDictionary modelState)
		{
			Product product = mapper.Map<Product>(dto);
			ValidationResult result = await validator.ValidateAsync(product);
			if (!result.IsValid) {
				result.AddErrorsToModelState(modelState);
			}
			
		}

		public async Task ValidateProductAsync(ProductUpdateDto dto, ModelStateDictionary modelState)
		{
			Product product = mapper.Map<Product>(dto);
			ValidationResult result = await validator.ValidateAsync(product);
			if (!result.IsValid)
			{
				result.AddErrorsToModelState(modelState);
			}
		}

        public async Task<List<ProductDto>> GetMostUsedProductsAsync()
        {
			List<Product> products = await repository.GetAllAsync(x => !x.IsDeleted & x.MostUsed);
			var map = mapper.Map<List<ProductDto>>(products);
			return map;
        }
    }
}
