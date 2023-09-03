using Cantin.Entity.Dtos.Products;

namespace Cantin.Entity.Dtos.Supplies
{
    public class SupplyUpdateDto
    {
        public Guid Id { get; set; }
        public int Quantity { get; set; }
        public float BuyPrice { get; set; }
        public string Supplier { get; set; }
        public List<ProductDto>? Products { get; set; }
        public Guid ProductId { get; set; }


    }
}
