using Cantin.Entity.Dtos.Supplies;
using Cantin.Entity.Entities;
using System.Linq.Expressions;

namespace Cantin.Data.Filters
{
    public static class SupplyFilter
    {
        public static Task<Expression<Func<Supply, bool>>> FilterAsync(SupplyFilterDto filterDto)
        {
            var filters = new List<Expression<Func<Supply, bool>>?>
        {
            GetDateFilter(filterDto.FirstDate, filterDto.LastDate),
            GetDateFilterForExpriation(filterDto.ExpriationFirstDate,filterDto.ExpriationLastDate),
            GetProductItemsFilter(filterDto.ItemIdList),
            GetSaleTotalFilter(filterDto.SaleTotalMinValue, filterDto.SaleTotalMaxValue)
        }.Where(filter => filter != null).ToList();

            return Task.FromResult(CombineFilters<Supply>(filters!));
        }
        private static Expression<Func<Supply, bool>>? GetDateFilter(DateTime? startDate, DateTime? endDate)
        {
            if (startDate.HasValue && endDate.HasValue)
                return sale => sale.CreatedDate >= startDate && sale.CreatedDate <= endDate;

            if (startDate.HasValue)
                return sale => sale.CreatedDate >= startDate;

            if (endDate.HasValue)
                return sale => sale.CreatedDate <= endDate;

            return null;
        }
        private static Expression<Func<Supply, bool>>? GetDateFilterForExpriation(DateTime? startDate, DateTime? endDate)
        {
            if (startDate.HasValue && endDate.HasValue)
                return sale => sale.ExpirationDate >= startDate && sale.ExpirationDate <= endDate;

            if (startDate.HasValue)
                return sale => sale.ExpirationDate >= startDate;

            if (endDate.HasValue)
                return sale => sale.ExpirationDate <= endDate;

            return null;
        }

        private static Expression<Func<Supply, bool>>? GetProductItemsFilter(List<Guid>? itemIds)
        {
            if (itemIds == null || !itemIds.Any())
                return null;

            return supply => itemIds.Contains(supply.ProductId);
        }

        private static Expression<Func<Supply, bool>>? GetSaleTotalFilter(float? minValue, float? maxValue)
        {
            if (minValue.HasValue && maxValue.HasValue)
                return supply => supply.UnitPrice * supply.Quantity >= minValue && supply.UnitPrice * supply.Quantity <= maxValue;

            if (minValue.HasValue)
                return supply => supply.UnitPrice * supply.Quantity >= minValue;

            if (maxValue.HasValue)
                return supply => supply.UnitPrice * supply.Quantity <= maxValue;

            return null;
        }

        private static Expression<Func<T, bool>>? CombineFilters<T>(List<Expression<Func<T, bool>>> filters)
        {
            if (filters.Count > 0)
            {
                var parameter = Expression.Parameter(typeof(T), "x");
                var body = filters.Select(filter => Expression.Invoke(filter, parameter))
                                  .Aggregate<Expression, Expression>(null, (current, next) => current == null ? (Expression)next : Expression.AndAlso(current, next));

                return Expression.Lambda<Func<T, bool>>(body, parameter);
            }
            return null;
        }
    }
}
