using Cantin.Entity.Dtos.Sales;
using Cantin.Entity.Entities;

namespace Cantin.Service.Services.Abstraction
{
	public interface IStockService
	{
		Task AddStockToStoreAsync(Supply supply);
		Task UpdateStockForSaleAsync(SaleAddDto dto, Guid storeId);
	}
}
