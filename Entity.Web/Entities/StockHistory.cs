using Cantin.Core.Entities;
using Cantin.Entity.Enums;

namespace Cantin.Entity.Entities
{
    public class StockHistory : EntityBase
    {
        public StockActions Type { get; set; }

        public Guid? StoreId { get; set; }
        public virtual Store Store { get; set; }

        public Guid? SaleId { get; set; }
        public virtual Sale Sale { get; set; }

        public Guid? SupplyId { get; set; }
        public virtual Supply Supply { get; set; }

        public Guid? ManualStockReductionId { get; set; }
        public virtual ManuelStockReduction ManualStockReduction { get; set; }

        public Guid? DebtId { get; set; }
        public virtual Debt Debt { get; set; }
    }
}
