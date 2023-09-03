using Cantin.Entity.Dtos.Products;
using Cantin.Entity.Dtos.Stores;

namespace Cantin.Entity.Dtos.Supplies
{
    public class SupplyAddDto
    {
        public int Quantity { get; set; }
        public float UnitPrice { get; set; }
        public string Supplier { get; set; }
        public List<ProductDto>? Products { get; set; }
        public Guid ProductId { get; set; }
        public List<StoreDto>? Stores { get; set; }
        public Guid StoreId { get; set; }

    }
}
