using Cantin.Core.Entities;
using Microsoft.AspNetCore.Identity;

namespace Cantin.Entity.Entities
{
    public class AppUser : IdentityUser<Guid>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public String FullName => FirstName + " " + LastName;
        public string Adress { get; set; }
        public Guid StoreId { get; set; }
        public Store Store { get; set; }

    }
}
