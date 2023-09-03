using Cantin.Entity.Dtos.Stores;
using Cantin.Entity.Entities;

namespace Cantin.Entity.Dtos.Users
{
    public class UserUpdateDto
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public String FullName => FirstName + " " + LastName;
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Adress { get; set; }
        public List<AppRole>? Roles { get; set; }
        public Guid RoleId { get; set; }
        public List<StoreDto>? Stores { get; set; }
        public Guid StoreId { get; set; }
    }
}
