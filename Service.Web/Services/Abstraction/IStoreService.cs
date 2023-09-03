using Cantin.Entity.Dtos.Stores;

namespace Cantin.Service.Services.Abstraction
{
	public interface IStoreService
	{
		Task<List<StoreDto>> GetAllStoreDtosNonDeleted();
		Task<StoreDto> GetStoreById(Guid Id);
		StoreAddDto GetStoreAddDto();
		Task<string> AddStoreAsync(StoreAddDto dto);
        Task<StoreUpdateDto> GetStoreUpdateDtoByIdAsync(Guid Id);
		Task<string> UpdateStoreAsync(StoreUpdateDto dto);
		Task<string> DeleteStoreAsyncById(Guid Id); 
    }
}
