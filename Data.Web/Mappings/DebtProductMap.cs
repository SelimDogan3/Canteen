using Cantin.Entity.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cantin.Data.Mappings
{
    public class DebtProductMap : IEntityTypeConfiguration<DebtProduct>
    {
        public void Configure(EntityTypeBuilder<DebtProduct> builder)
        {
            builder.Property(x => x.SubTotal).HasColumnType("decimal(18,4)");
        }
    }
}
