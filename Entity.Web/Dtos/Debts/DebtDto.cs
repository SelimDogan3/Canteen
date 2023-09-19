using Cantin.Entity.Dtos.Stores;
using Cantin.Entity.Entities;

namespace Cantin.Entity.Dtos.Debts
{
    public class DebtDto 
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName => FirstName + " " + LastName;
        public StoreDto Store { get; set; }
        public List<ProductLine> ProductLines { get; set; }
        public float TotalPrice { get; set; }
        public bool Paid { get; set; }
        public DateTime? PaidDate { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
