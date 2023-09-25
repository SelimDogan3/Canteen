namespace Cantin.Entity.Dtos.ManuelStockReduction
{
    public class ManuelStockReductionAddDto
    {
        public Guid ProductId { get; set; }
        public Guid StoreId { get; set; }
        public int DecreasingQuantity { get; set; }
        public string FReason { get; set; }
        public string Reason => FReason == null || FReason == "" ? "Belirtilmedi" : FReason;
    }
}
