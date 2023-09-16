using Cantin.Entity.Dtos.Debts;
using Cantin.Service.Extensions;
using Cantin.Service.Services.Abstraction;
using Microsoft.AspNetCore.Mvc;

namespace Cantin.Web.Components
{
    public class DebtAuthorizationViewHandlerViewComponent : ViewComponent
    {
        private readonly IDebtService debtService;

        public DebtAuthorizationViewHandlerViewComponent(IDebtService debtService)
        {
            this.debtService = debtService;

            
        }
        public async Task<IViewComponentResult> InvokeAsync(List<DebtDto> debts) {
           var role = HttpContext.User.GetRole();
            if (role != "Admin" && role != "Superadmin"){
                return View("EmployeeDebtView",debts);  
            }
            else
            {
                debts = await debtService.GetAllDebtsNonDeletedAsync();
                return View("AdminDebtView", debts);
            }
        }
    }
}
