using Cantin.Entity.Dtos.Debts;
using Cantin.Entity.Dtos.ManuelStockReduction;
using Cantin.Entity.Dtos.Sales;
using Cantin.Entity.Dtos.Stores;
using Cantin.Entity.Entities;

namespace Cantin.Service.Services.Abstraction
{
	public interface IStockService
	{
		Task<List<StockDto>> GetAllStocksIncludingStoresAsync();
		Task<StockDto> GetStocksOfAnStoreAsync(Guid storeId);
        Task AddStockToStoreAsync(Supply supply);
        Task AddManuelStockReductionAsync(ManuelStockReductionAddDto manuelSD);
		Task UpdateStockAsync(Sale sale);
		Task UpdateStockAsync(Debt debt);
		Task<string> UpdateStockAsync(ManuelStockReductionAddDto manuelSD);

    }
}
