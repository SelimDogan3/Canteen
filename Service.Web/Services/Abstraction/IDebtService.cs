using Cantin.Entity.Dtos.Debts;
using Cantin.Entity.Entities;

namespace Cantin.Service.Services.Abstraction
{
    public interface IDebtService
    {
        Task<List<DebtDto>> GetAllDebtsNonDeletedAsync(DebtFilterDto? filterDto = null);
        Task<DebtDto> GetDebtByIdAsync(Guid Id);
        Task<List<ProductLine>> GetMatchingProductsAsync(Guid debtId);
        Task MatchProductWithDebtsAsync(Guid debtId, Guid productId, int quantity);
        Task AddDebtAsync(DebtAddDto dto);
        Task PayDebtAsync(DebtPaidDto dto);
    }
}
