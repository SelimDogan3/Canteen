using Cantin.Core.Entities;

namespace Cantin.Entity.Entities
{
    public class ManuelStockReduction : EntityBase
    {
        public Guid StoreId { get; set; }
        public Store Store { get; set; }
        public Guid ProductId { get; set; }
        public Product Product { get; set; }
        public int DecreasingQuantity { get; set; }
        public string Reason { get; set; }
    }
}
