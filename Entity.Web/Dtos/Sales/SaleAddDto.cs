using Cantin.Entity.Entities;

namespace Cantin.Entity.Dtos.Sales
{
	public class SaleAddDto
	{
		public decimal SoldTotal { get; set; }
		public List<ProductLine> ProductLines { get; set; } = new List<ProductLine>();
    }
}
