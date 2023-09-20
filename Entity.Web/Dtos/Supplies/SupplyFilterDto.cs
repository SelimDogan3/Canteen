using Cantin.Core.Helpers;

namespace Cantin.Entity.Dtos.Supplies
{
    public class SupplyFilterDto
    {
        public string? StringFirstDate { get; set; }
        public DateTime? FirstDate => DateTimeFormatter.GetDateTimeForFiltersForStartDate(StringFirstDate);
        public string? StringLastDate { get; set; }
        public DateTime? LastDate => DateTimeFormatter.GetDateTimeForFiltersForFinishDate(StringLastDate);
        public string? StringExpriationFirstDate { get; set; }
        public DateTime? ExpriationFirstDate => DateTimeFormatter.GetDateTimeForExpriationFiltersForStartDate(StringExpriationFirstDate);
        public string? StringExpriationLastDate { get; set; }
        public DateTime? ExpriationLastDate => DateTimeFormatter.GetDateTimeForExpriationFiltersForFinishDate(StringExpriationLastDate);
        public List<Guid>? ItemIdList { get; set; }
        public float? SaleTotalMinValue { get; set; }
        public float? SaleTotalMaxValue { get; set; }
    }
}
