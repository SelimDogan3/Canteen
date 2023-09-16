using Cantin.Entity.Dtos.Debts;
using Cantin.Entity.Dtos.Sales;
using Cantin.Entity.Entities;
using System.Linq.Expressions;

namespace Cantin.Data.Filters
{
    public static class DebtFilter
    {
        public static Task<Expression<Func<Debt, bool>>> FilterAsync(DebtFilterDto filterDto)
        {
            var filters = new List<Expression<Func<Debt, bool>>?>
        {
            GetDateFilter(filterDto.FirstDate, filterDto.LastDate),
            GetProductItemsFilter(filterDto.ItemIdList),
            GetPaymentTypeFilter(filterDto.PaymentType),
            GetPaidFilter(filterDto.Paid),
            GetStoresFilter(filterDto.StoreIdList),
            GetSaleTotalFilter(filterDto.SaleTotalMinValue, filterDto.SaleTotalMaxValue)
        }.Where(filter => filter != null).ToList();

            return Task.FromResult(CombineFilters<Debt>(filters!));
        }
        private static Expression<Func<Debt, bool>>? GetDateFilter(DateTime? startDate, DateTime? endDate)
        {
            if (startDate.HasValue && endDate.HasValue)
                return debt => debt.CreatedDate >= startDate && debt.CreatedDate <= endDate;

            if (startDate.HasValue)
                return debt => debt.CreatedDate >= startDate;

            if (endDate.HasValue)
                return debt => debt.CreatedDate <= endDate;

            return null;
        }
        private static Expression<Func<Debt, bool>>? GetProductItemsFilter(List<Guid>? itemIds)
        {
            if (itemIds == null || !itemIds.Any())
                return null;

            return sale => sale.DebtProducts.Any(sp => itemIds.Contains(sp.ProductId));
        }
        private static Expression<Func<Debt, bool>>? GetPaymentTypeFilter(string? paymentType)
        {
            if (string.IsNullOrEmpty(paymentType))
                return null;

            return debt => debt.PaymentType == paymentType;
        }
        private static Expression<Func<Debt, bool>>? GetStoresFilter(List<Guid>? storeIds)
        {
            if (storeIds == null || !storeIds.Any())
                return null;

            return debt => storeIds.Contains(debt.StoreId);
        }
        private static Expression<Func<Debt, bool>>? GetSaleTotalFilter(float? minValue, float? maxValue)
        {
            if (minValue.HasValue && maxValue.HasValue)
                return debt => debt.TotalPrice >= minValue && debt.TotalPrice <= maxValue;

            if (minValue.HasValue)
                return debt => debt.TotalPrice >= minValue;

            if (maxValue.HasValue)
                return debt => debt.TotalPrice <= maxValue;

            return null;
        }
        private static Expression<Func<Debt, bool>> GetPaidFilter(bool? paid = false) {
            return debt => debt.Paid == paid;
        }
        private static Expression<Func<T, bool>> CombineFilters<T>(List<Expression<Func<T, bool>>> filters)
        {
            var parameter = Expression.Parameter(typeof(T), "x");
            var body = filters.Select(filter => Expression.Invoke(filter, parameter))
                              .Aggregate<Expression, Expression>(null, (current, next) => current == null ? (Expression)next : Expression.AndAlso(current, next));

            return Expression.Lambda<Func<T, bool>>(body, parameter);
        }
    }
}
