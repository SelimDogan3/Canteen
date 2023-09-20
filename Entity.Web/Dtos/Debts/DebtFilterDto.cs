using Cantin.Core.Helpers;

namespace Cantin.Entity.Dtos.Debts
{
    public class DebtFilterDto
    {
        public string? StringFirstDate { get; set; }
        public DateTime? FirstDate => DateTimeFormatter.GetDateTimeForFiltersForStartDate(StringFirstDate);
        public string? StringLastDate { get; set; }
        public DateTime? LastDate => DateTimeFormatter.GetDateTimeForFiltersForFinishDate(StringLastDate);
        public List<Guid>? ItemIdList { get; set; }
        public List<Guid>? StoreIdList { get; set; }
        public string? PaymentType { get; set; }
        public float? SaleTotalMinValue { get; set; }
        public float? SaleTotalMaxValue { get; set; }
        public bool? Paid { get; set; }
    }
}
