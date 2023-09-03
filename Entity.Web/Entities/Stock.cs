using Cantin.Core.Entities;

namespace Cantin.Entity.Entities
{
    public class Stock : EntityBase
    {
        public Guid ProductId { get; set; }
        public Product Product { get; set; }
        public int Quantity { get; set; }
        public Guid StoreId { get; set; }
        public Store Store { get; set; }
    }
}
