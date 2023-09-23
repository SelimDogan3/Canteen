using Cantin.Entity.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cantin.Data.Mappings
{
    public class StoreMap : IEntityTypeConfiguration<Store>
    {
        public void Configure(EntityTypeBuilder<Store> builder)
        {
            builder.HasData(new Store {
                Id = Guid.Parse("90411F34-B61A-4A4D-BBB1-6D98A2F9CF34"),
                Name = "SuperAdmin",
                Adress = "SuperAdmin",
                PhoneNumber = "123214512412512",
                CreatedBy = "Superadmin",
                CreatedDate = DateTime.Now,
                IsDeleted = false,
            });
        }
    }
}
