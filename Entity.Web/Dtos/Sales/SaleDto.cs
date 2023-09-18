using Cantin.Entity.Dtos.Stores;
using Cantin.Entity.Entities;

namespace Cantin.Entity.Dtos.Sales
{
    public class SaleDto
    {
        public Guid Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid StoreId { get; set; }
        public StoreDto? Store { get; set; }
        public float SoldTotal { get; set; } = 0;
		public float PaidAmount { get; set; }
		public float Exchange => (float) Math.Round((PaidAmount - SoldTotal),3);
		public string PaymentType { get; set; } = "Nakit";
		public List<ProductLine>? ProductLines { get; set; }
    }
}
