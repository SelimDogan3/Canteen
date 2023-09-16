using Cantin.Core.Entities;

namespace Cantin.Entity.Entities
{
    public class Debt : EntityBase
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName => FirstName + " " + LastName;
        public Guid StoreId { get; set; }
        public Store Store { get; set; }
        public ICollection<DebtProduct> DebtProducts { get; set; }
        public bool Paid { get; set; }
        public DateTime? PaidDate { get; set; }
        public float TotalPrice { get; set; }
        public string? PaymentType { get; set; }
        public float? PaidAmount { get; set; }
        public float? Exchange { get; set; }
    }
}
