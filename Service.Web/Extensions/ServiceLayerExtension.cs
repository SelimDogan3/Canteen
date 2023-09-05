using Cantin.Service.Services.Abstraction;
using Cantin.Service.Services.Concrete;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;
using System.Globalization;
using System.Reflection;

namespace Cantin.Service.Extensions
{
	public static class ServiceLayerExtension
    {
        public static IServiceCollection LoadServiceLayerExtensions(this IServiceCollection services)
        {
            var assembly = Assembly.GetExecutingAssembly();
            services.AddAutoMapper(assembly);
            services.AddFluentValidation(opt => { 
                opt.RegisterValidatorsFromAssembly(assembly);
                opt.DisableDataAnnotationsValidation = true;
                opt.ValidatorOptions.LanguageManager.Culture = new CultureInfo("tr");
                })
                ;
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IStoreService, StoreService>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<ISupplyService, SupplyService>();
            services.AddScoped<IStockService, StockService>();
            services.AddScoped<ISaleService, SaleService>();
            return services;
        }
    }
}
