using Cantin.Entity.Dtos.Products;
using Cantin.Entity.Dtos.Supplies;
using Cantin.Entity.Entities;
using Microsoft.AspNetCore.Mvc.ModelBinding;

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
		Task ValidateSupplyAsync(SupplyAddDto dto, ModelStateDictionary modelState);
		Task ValidateSupplyAsync(SupplyUpdateDto dto, ModelStateDictionary modelState);
	}
}
