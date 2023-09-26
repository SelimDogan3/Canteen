using Cantin.Entity.Dtos.Sales;
using Cantin.Entity.Entities;

namespace Cantin.Service.Services.Abstraction
{
	public interface ISaleService
	{
        Task<List<SaleDto>> GetAllSalesNonDeletedAsync(SaleFilterDto? filterDto = null);
		Task<List<SaleDto>> GetSalesForEmployeeAsync();
		Task AddSaleAsync(SaleAddDto dto);
		Task MatchProductWithSaleAsync(Guid saleId, Guid productId, int quantity);
		Task<SaleDto> GetSaleByIdAsync(Guid id);
	}
}
