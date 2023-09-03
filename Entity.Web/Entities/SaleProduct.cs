using Cantin.Core.Entities;

namespace Cantin.Entity.Entities
{
	public class SaleProduct : EntityBase
	{
        public Guid ProductId { get; set; }
        public Guid SaleId { get; set; }
        public int Quantity { get; set; }
    }
}
