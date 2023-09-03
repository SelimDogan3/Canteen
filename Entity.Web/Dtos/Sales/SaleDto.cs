using Cantin.Entity.Entities;

namespace Cantin.Entity.Dtos.Sales
{
    public class SaleDto
    {
        public Guid Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid StoreId { get; set; }
        public Store? Store { get; set; }
        public string SoldTotal { get; set; } = "0";
        public List<ProductLine>? ProductLines { get; set; }
    }
}
