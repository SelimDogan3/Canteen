namespace Cantin.Entity.Dtos.Sales
{
    public class SaleFilterDto
    {
        public DateTime? FirstDate { get; set; }
        public DateTime? LastDate { get; set; }
        public List<Guid>? ItemIdList { get; set; }
        public List<Guid>? StoreIdList { get; set; }
        public string? PaymentType { get; set; }
        public float? SaleTotalMinValue { get; set; }
        public float? SaleTotalMaxValue { get; set; }
    }
}
