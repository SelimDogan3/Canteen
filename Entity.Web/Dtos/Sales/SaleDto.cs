using Cantin.Entity.Entities;

namespace Cantin.Entity.Dtos.Sales
{
    public class SaleDto
    {
        public Guid Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid StoreId { get; set; }
        public Store? Store { get; set; }
        public float SoldTotal { get; set; } = 0;
		public float PaidAmount { get; set; }
		public float Exchange => PaidAmount - SoldTotal;
		public string PaymentType { get; set; } = "Nakit";
		public List<ProductLine>? ProductLines { get; set; }
    }
}
