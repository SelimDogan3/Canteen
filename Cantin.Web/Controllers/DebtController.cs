using Cantin.Entity.Dtos.Debts;
using Cantin.Service.Services.Abstraction;
using Microsoft.AspNetCore.Mvc;

namespace Cantin.Web.Controllers
{
    public class DebtController : Controller
    {
        private readonly IDebtService debtService;

        public DebtController(IDebtService debtService)
        {
            this.debtService = debtService;
        }
        public async Task<IActionResult> Index()
        {
            var debts = await debtService.GetAllDebtsNonDeletedAsync();
            return View(debts);
        }
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] DebtAddDto addDto) {
            await debtService.AddDebtAsync(addDto);
            return Ok();
        }
        [HttpGet]
        public async Task<IActionResult> Pay(Guid Id) {
            var debt = await debtService.GetDebtByIdAsync(Id);
            return View(debt);
        }
        [HttpPost]
        public async Task<IActionResult> Pay(DebtPaidDto dto) {
            await debtService.PayDebtAsync(dto);
            return RedirectToAction("Index");
        }
        [HttpPost]
        public async Task<ActionResult<List<DebtDto>>> GetWithFilter([FromBody]DebtFilterDto filterDto) {
            var debts = await debtService.GetAllDebtsNonDeletedAsync(filterDto);
            return Ok(debts);
        }
    }
}
