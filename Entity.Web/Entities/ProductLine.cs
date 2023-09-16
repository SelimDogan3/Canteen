using Cantin.Entity.Dtos.Products;

namespace Cantin.Entity.Entities
{
    public class ProductLine
    {
        public Guid ProductId { get; set; }
        public ProductDto Product { get; set; }
        public int Quantity { get; set; }
        public decimal SubTotal => Decimal.Parse(Product.SalePrice) * Quantity;

    }
}
