using Cantin.Entity.Dtos.Sales;
using Cantin.Entity.Entities;
using Cantin.Service.Extensions;
using Cantin.Service.Services.Abstraction;
using Cantin.Service.Services.Concrete;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Cantin.Web.Components
{
    public class SaleAuthorizationViewHandlerViewComponent : ViewComponent
    {

        private readonly IHttpContextAccessor contextAccessor;

        public SaleAuthorizationViewHandlerViewComponent(IHttpContextAccessor contextAccessor)
        {
            this.contextAccessor = contextAccessor;
        }
        public async Task<IViewComponentResult> InvokeAsync(List<SaleDto> sales) {
            var role = contextAccessor.HttpContext.User.GetRole();
            if (role != "Admin" && role != "Superadmin")
            {
                return View("EmployeeSaleView", sales);
            }
            else
            {
                return View("AdminSaleView", sales);
            }
        }

    }
}
