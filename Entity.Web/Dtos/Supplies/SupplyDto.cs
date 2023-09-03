using Cantin.Entity.Dtos.Products;
using Cantin.Entity.Dtos.Stores;

namespace Cantin.Entity.Dtos.Supplies
{
    public class SupplyDto
    {
        public Guid Id { get; set; }
        public int Quantity { get; set; }
        public float UnitPrice { get; set; }
        public float Total => Quantity * UnitPrice;
        public string Supplier { get; set; }
        public ProductDto Product { get; set; }
        public StoreDto Store { get; set; }
    }
}
