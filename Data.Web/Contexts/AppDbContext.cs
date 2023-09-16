using Cantin.Entity.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Cantin.Data.Contexts
{
    public class AppDbContext : IdentityDbContext<AppUser, AppRole, Guid, AppUserClaim, AppUserRole, AppUserLogin, AppRoleClaim, AppUserToken>

    {
        public AppDbContext()
        {

        }
        public AppDbContext(DbContextOptions opt) : base(opt)
        {

        }
        public DbSet<Product> Products { get; set; }
        public DbSet<Sale> Sales { get; set; }
        public DbSet<Supply> Supplies { get; set; }
        public DbSet<Store> Stores { get; set; }
        public DbSet<Stock> Stocks { get; set; }
        public DbSet<Debt> Debts { get; set; }
        public DbSet<SaleProduct> SaleProducts { get; set; }
        public DbSet<DebtProduct> DebtProducts { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

    }
}
