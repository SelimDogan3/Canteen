using Cantin.Entity.Dtos.Sales;
using Cantin.Entity.Dtos.Stores;
using Cantin.Entity.Entities;

namespace Cantin.Service.Services.Abstraction
{
	public interface IStockService
	{
		Task<List<StockDto>> GetAllStocksIncludingStores();
		Task<List<Stock>> GetStocksOfAnStore(Guid storeId);
        Task AddStockToStoreAsync(Supply supply);
		Task UpdateStockForSaleAsync(SaleAddDto dto, Guid storeId);
	}
}
