namespace Cantin.Entity.Dtos.Debts
{
    public class DebtFilterDto
    {
        public DateTime? FirstDate { get; set; }
        public DateTime? LastDate { get; set; }
        public List<Guid>? ItemIdList { get; set; }
        public List<Guid>? StoreIdList { get; set; }
        public string? PaymentType { get; set; }
        public float? SaleTotalMinValue { get; set; }
        public float? SaleTotalMaxValue { get; set; }
        public bool? Paid { get; set; }
    }
}
