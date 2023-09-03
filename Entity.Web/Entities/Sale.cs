using Cantin.Core.Entities;

namespace Cantin.Entity.Entities
{
    public class Sale : EntityBase
    {
        public string SoldTotal { get; set; }
        public Guid StoreId { get; set; }
        public Store Store { get; set; }
        public ICollection<SaleProduct> SaleProducts { get; set; }


    }
}
