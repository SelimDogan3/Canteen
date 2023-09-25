using Cantin.Core.Entities;

namespace Cantin.Entity.Entities
{
    public class Store : EntityBase
    {
        public string Name { get; set; }
        public string Adress { get; set; }
        public string PhoneNumber { get; set; }
        public ICollection<AppUser> AppUsers { get; set; }
        public ICollection<Stock> Stocks { get; set; }
        public ICollection<Sale> Sales { get; set; }
        public ICollection<Supply> Suppliles { get; set; }
        public ICollection<Debt> Debts { get; set; }
    }


}

