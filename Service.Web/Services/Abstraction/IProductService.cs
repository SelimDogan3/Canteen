using Cantin.Entity.Dtos.Products;
using Cantin.Entity.Entities;

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
	}
}
