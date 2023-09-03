using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Cantin.Entity.Entities;

namespace Cantin.Data.Mappings
{
    public class UserRoleMap : IEntityTypeConfiguration<AppUserRole>
    {
        public void Configure(EntityTypeBuilder<AppUserRole> builder)
        {
            // Primary key
            builder.HasKey(r => new { r.UserId, r.RoleId });

            // Maps to the AspNetUserRoles table
            builder.ToTable("AspNetUserRoles");

            builder.HasData(new AppUserRole
            {
                UserId = Guid.Parse("6168A092-56D5-439D-A0B8-940FBDA81950"),
                RoleId = Guid.Parse("2AC8179A-3F45-40D2-AC0E-65D58333E265"),
            },
            new AppUserRole
            {
                UserId = Guid.Parse("4083029D-7624-44D6-ACFA-4A54DEEFBD3F"),
                RoleId = Guid.Parse("EEBCD6BA-D079-4FDE-A81A-DF80076C8002")
            });
        }
    }
}
