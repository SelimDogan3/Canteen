using Cantin.Entity.Dtos.Stores;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Cantin.Service.Services.Abstraction
{
	public interface IStoreService
	{
		Task<List<StoreDto>> GetAllStoreDtosNonDeleted();
		Task<List<StoreDto>> GetAllStoresNonDeletedWithStocksAsync();
        Task<StoreDto> GetStoreById(Guid Id);
		Task<StockDto> GetStoreByIdWithStocks(Guid Id);
        StoreAddDto GetStoreAddDto();
		Task<string> AddStoreAsync(StoreAddDto dto);
        Task<StoreUpdateDto> GetStoreUpdateDtoByIdAsync(Guid Id);

		Task<string> UpdateStoreAsync(StoreUpdateDto dto);
		Task<string> DeleteStoreAsyncById(Guid Id);
		Task ValidateStoreAsync(StoreAddDto dto, ModelStateDictionary modelState);
		Task ValidateStoreAsync(StoreUpdateDto dto, ModelStateDictionary modelState);

    }
}
