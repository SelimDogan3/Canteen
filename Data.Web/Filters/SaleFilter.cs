using Cantin.Entity.Dtos.Sales;
using Cantin.Entity.Entities;
using System.Linq.Expressions;

namespace Cantin.Data.Filters
{
    public static class SaleFilter
    {
        public static Task<Expression<Func<Sale, bool>>> FilterAsync(SaleFilterDto filterDto)
        {
            var filters = new List<Expression<Func<Sale, bool>>?>
        {
            GetDateFilter(filterDto.FirstDate, filterDto.LastDate),
            GetProductItemsFilter(filterDto.ItemIdList),
            GetPaymentTypeFilter(filterDto.PaymentType),
            GetStoresFilter(filterDto.StoreIdList),
            GetSaleTotalFilter(filterDto.SaleTotalMinValue, filterDto.SaleTotalMaxValue)
        }.Where(filter => filter != null).ToList();

            return Task.FromResult(CombineFilters<Sale>(filters!));
        }
        private static Expression<Func<Sale, bool>>? GetDateFilter(DateTime? startDate, DateTime? endDate)
        {
            if (startDate.HasValue && endDate.HasValue)
                return sale => sale.CreatedDate >= startDate && sale.CreatedDate <= endDate;

            if (startDate.HasValue)
                return sale => sale.CreatedDate >= startDate;

            if (endDate.HasValue)
                return sale => sale.CreatedDate <= endDate;

            return null;
        }

        private static Expression<Func<Sale, bool>>? GetProductItemsFilter(List<Guid>? itemIds)
        {
            if (itemIds == null || !itemIds.Any())
                return null;

            return sale => sale.SaleProducts.Any(sp => itemIds.Contains(sp.ProductId));
        }

        private static Expression<Func<Sale, bool>>? GetPaymentTypeFilter(string? paymentType)
        {
            if (string.IsNullOrEmpty(paymentType))
                return null;

            return sale => sale.PaymentType == paymentType;
        }

        private static Expression<Func<Sale, bool>>? GetStoresFilter(List<Guid>? storeIds)
        {
            if (storeIds == null || !storeIds.Any())
                return null;

            return sale => storeIds.Contains(sale.StoreId);
        }

        private static Expression<Func<Sale, bool>>? GetSaleTotalFilter(float? minValue, float? maxValue)
        {
            if (minValue.HasValue && maxValue.HasValue)
                return sale => sale.SoldTotal >= minValue && sale.SoldTotal <= maxValue;

            if (minValue.HasValue)
                return sale => sale.SoldTotal >= minValue;

            if (maxValue.HasValue)
                return sale => sale.SoldTotal <= maxValue;

            return null;
        }

        private static Expression<Func<T, bool>>? CombineFilters<T>(List<Expression<Func<T, bool>>> filters)
        {
            if (filters.Count > 0) { 
            var parameter = Expression.Parameter(typeof(T), "x");
            var body = filters.Select(filter => Expression.Invoke(filter, parameter))
                              .Aggregate<Expression, Expression>(null, (current, next) => current == null ? (Expression)next : Expression.AndAlso(current, next));

            return Expression.Lambda<Func<T, bool>>(body, parameter);
            }
            return null;
        }


    }
}
