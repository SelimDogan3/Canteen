using Cantin.Data.Contexts;
using Cantin.Data.Identity;
using Cantin.Data.UnitOfWorks;
using Cantin.Entity.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Cantin.Data.Extensions
{
	public static class DataLayerExtensions
	{

		public static IServiceCollection LoadDataLayerExtensions(this IServiceCollection services,IConfiguration config) {
			services.AddDbContext<AppDbContext>(opt => opt.UseSqlServer(config.GetConnectionString("DefaultConnection")));
			services.AddIdentity<AppUser, AppRole>(
				opt =>
				{
					opt.Password.RequireNonAlphanumeric = false;
					opt.Password.RequireLowercase = false;
					opt.Password.RequireUppercase = true;
					opt.User.RequireUniqueEmail = false;
				})
			.AddRoleManager<RoleManager<AppRole>>()
			.AddEntityFrameworkStores<AppDbContext>()
			.AddDefaultTokenProviders()
			.AddUserValidator<AppIdentityUserValidator>()
			.AddErrorDescriber<AppIdentityErrorDescriber>()
			;
			services.AddScoped<IUnitOfWork, UnitOfWork>();
			return services;
		}
	}
}
