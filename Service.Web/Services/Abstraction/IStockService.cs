using Cantin.Entity.Dtos.Debts;
using Cantin.Entity.Dtos.Sales;
using Cantin.Entity.Dtos.Stores;
using Cantin.Entity.Entities;

namespace Cantin.Service.Services.Abstraction
{
	public interface IStockService
	{
		Task<List<StockDto>> GetAllStocksIncludingStores();
		Task<StockDto> GetStocksOfAnStoreAsync(Guid storeId);
        Task AddStockToStoreAsync(Supply supply);
		Task UpdateStockAsync(SaleAddDto dto, Guid storeId);
		Task UpdateStockAsync(DebtAddDto dto, Guid storeId);
		Task UpdateStockAsync(Guid storeId, Guid ProductId, int Quantity);

    }
}
