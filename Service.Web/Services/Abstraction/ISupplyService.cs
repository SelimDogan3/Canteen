using Cantin.Entity.Dtos.Supplies;
using Cantin.Entity.Entities;

namespace Cantin.Service.Services.Abstraction
{
    public interface ISupplyService
	{
		Task<List<SupplyDto>> GetAllSuppliesNonDeleted();
		Task<Supply> GetSupplyById(Guid Id);
		Task<SupplyAddDto> GetSupplyAddDtoAsync();
		Task<string> AddSupplyAsync(SupplyAddDto addDto);
		Task<SupplyUpdateDto> GetSupplyUpdateDto(Guid Id);
		Task<string> UpdateSupplyAsync(SupplyUpdateDto updateDto);
		Task<string> DeleteStockAsync(Guid Id);

	}
}
