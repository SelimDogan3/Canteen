using Cantin.Entity.Dtos.Products;
using Cantin.Entity.Entities;

namespace Cantin.Entity.Dtos.Sales
{
	public class SaleAddDto
	{
		public decimal SoldTotal { get; set; }
		public List<ProductLine> ProductLines { get; set; } = new List<ProductLine>();
		public List<ProductDto>? MostUsedProducts { get; set; } = new List<ProductDto>();
    }
}
