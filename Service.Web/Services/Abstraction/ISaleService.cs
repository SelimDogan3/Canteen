using Cantin.Entity.Dtos.Sales;
using Cantin.Entity.Entities;

namespace Cantin.Service.Services.Abstraction
{
	public interface ISaleService
	{
		Task<List<SaleDto>> GetAllSalesNonDeletedAsync();
		SaleAddDto GetSaleAddDto();
		Task AddSaleAsync(SaleAddDto dto);
		Task MatchProductWithSaleAsync(Guid saleId, Guid productId, int quantity);
	}
}
