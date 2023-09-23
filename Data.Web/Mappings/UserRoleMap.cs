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
            });
        }
    }
}
