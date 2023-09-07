using Cantin.Core.Entities;

namespace Cantin.Entity.Entities
{
    public class Sale : EntityBase
    {
        public float SoldTotal { get; set; }
        public float PaidAmount { get; set; }
        public float Exchange => PaidAmount - SoldTotal;
        public string PaymentType { get; set; } = "Nakit";
        public Guid StoreId { get; set; }
        public Store Store { get; set; }
        public ICollection<SaleProduct> SaleProducts { get; set; }


    }
}
