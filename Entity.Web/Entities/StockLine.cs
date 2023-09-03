using Cantin.Entity.Dtos.Products;

namespace Cantin.Entity.Entities
{
    public class StockLine
    {
        public Guid ProductId { get; set; }
        public ProductDto? Product { get; set; }
        public int Quantity { get; set; }
    }
}
