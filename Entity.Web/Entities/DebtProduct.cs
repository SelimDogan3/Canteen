using Cantin.Core.Entities;

namespace Cantin.Entity.Entities
{
    public class DebtProduct : EntityBase
    {
        public Guid Id { get; set; }
        public Guid DebtId { get; set; }
        public Debt Debt { get; set; }
        public Guid ProductId { get; set; }
        public Product Product { get; set; }
        public int Quantity { get; set; }
        public decimal? SubTotal { get; set; }
    }
}
