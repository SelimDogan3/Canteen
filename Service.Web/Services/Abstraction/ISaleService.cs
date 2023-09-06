using Cantin.Entity.Dtos.Sales;
using Cantin.Entity.Entities;

namespace Cantin.Service.Services.Abstraction
{
	public interface ISaleService
	{
		Task<List<SaleDto>> GetAllSalesNonDeletedAsync();
		Task<List<SaleDto>> GetSalesForEmployeeAsync();
		Task<SaleAddDto> GetSaleAddDtoAsync();
		Task AddSaleAsync(SaleAddDto dto);
		Task MatchProductWithSaleAsync(Guid saleId, Guid productId, int quantity);
	}
}
