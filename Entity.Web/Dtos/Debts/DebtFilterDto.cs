namespace Cantin.Entity.Dtos.Debts
{
    public class DebtFilterDto
    {
        public string? StringFirstDate { get; set; }
        public DateTime? FirstDate => StringFirstDate != null && StringFirstDate != "" ? Convert.ToDateTime(StringFirstDate) : null;
        public string? StringLastDate { get; set; }
        public DateTime? LastDate => StringLastDate != null && StringLastDate != "" ? Convert.ToDateTime(StringLastDate) : null;
        public List<Guid>? ItemIdList { get; set; }
        public List<Guid>? StoreIdList { get; set; }
        public string? PaymentType { get; set; }
        public float? SaleTotalMinValue { get; set; }
        public float? SaleTotalMaxValue { get; set; }
        public bool? Paid { get; set; }
    }
}
