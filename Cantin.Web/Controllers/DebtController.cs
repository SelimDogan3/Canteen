using Cantin.Entity.Dtos.Debts;
using Cantin.Service.Services.Abstraction;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cantin.Web.Controllers
{
    public class DebtController : Controller
    {
        private readonly IDebtService debtService;
        private readonly IProductService productService;
        private readonly IStoreService storeService;

        public DebtController(IDebtService debtService, IProductService productService, IStoreService storeService)
        {
            this.debtService = debtService;
            this.productService = productService;
            this.storeService = storeService;
        }
        public async Task<IActionResult> Index()
        {
            var debts = await debtService.GetAllDebtsNonDeletedAsync();
            ViewBag.Products = await productService.GetAllProductsNonDeletedAsync();
            ViewBag.Stores = await storeService.GetAllStoreDtosNonDeleted();
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
        [Authorize(Policy = "AdminOnly")]

        public async Task<ActionResult<List<DebtDto>>> GetWithFilter([FromBody] DebtFilterDto filterDto) {
            var debts = await debtService.GetAllDebtsNonDeletedAsync(filterDto);
            return Ok(debts);
        }
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> Detail(Guid Id) {
            var debt = await debtService.GetDebtByIdAsync(Id);
            return View(debt);
        }
    }
}
