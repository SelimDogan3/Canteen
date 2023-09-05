using Cantin.Entity.Dtos.Products;
using Cantin.Entity.Entities;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Cantin.Service.Services.Abstraction
{
	public interface IProductService
	{
		Task<List<ProductDto>> GetAllProductsNonDeletedAsync();
		Task<Product> GetProductByIdAsync(Guid productId);
		Task<ProductDto> GetProductByBarcodeAsync(string barcode);
		ProductAddDto GetProductAddDto();
		Task<string> AddProductAsync(ProductAddDto addDto);
		Task<ProductUpdateDto> GetProductUpdateDtoAsync(Guid Id);
		Task<ProductUpdateDto> GetProductUpdateDtoAsync(Product product);
		Task<string> UpdateProductAsync(ProductUpdateDto updateDto);
		Task<string> DeleteProductByIdAsync(Guid Id);
		Task ValidateProductAsync(ProductAddDto dto,ModelStateDictionary modelState);
		Task ValidateProductAsync(ProductUpdateDto dto,ModelStateDictionary modelState);
	}
}
