using AutoMapper;
using Cantin.Data.Filters;
using Cantin.Data.Repository.Abstract;
using Cantin.Data.UnitOfWorks;
using Cantin.Entity.Dtos.Debts;
using Cantin.Entity.Entities;
using Cantin.Service.Extensions;
using Cantin.Service.Services.Abstraction;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Linq.Expressions;
using System.Security.Claims;

namespace Cantin.Service.Services.Concrete
{
    public class DebtService : IDebtService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        private readonly UserManager<AppUser> userManager;
        private readonly IProductService productService;
        private readonly IStockService stockService;
        private readonly ClaimsPrincipal _user;

        private IRepository<Debt> repository => unitOfWork.GetRepository<Debt>();

        public DebtService(IUnitOfWork unitOfWork,IMapper mapper,IHttpContextAccessor contextAccessor,UserManager<AppUser> userManager,IProductService productService,IStockService stockService)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            this.userManager = userManager;
            this.productService = productService;
            this.stockService = stockService;
            _user = contextAccessor!.HttpContext!.User;
        }

        public async Task<List<DebtDto>> GetAllDebtsNonDeletedAsync(DebtFilterDto? filterDto = null)
        {
            Expression<Func<Debt, bool>> query;
            if (filterDto != null)
            {
                query = await DebtFilter.FilterAsync(filterDto);
            }
            else
            {
                query = x => !x.IsDeleted;
            }
            List<Debt> debts = await repository.GetAllAsync(query,x => x.DebtProducts,x => x.Store);
            List<DebtDto> maps = mapper.Map<List<DebtDto>>(debts);
            foreach (DebtDto map in maps)
            {
                map.ProductLines = await GetMatchingProductsAsync(map.Id);
            }
            return maps;
        }
        public async Task<List<ProductLine>> GetMatchingProductsAsync(Guid debtId) {
            List<DebtProduct> lines = await unitOfWork.GetRepository<DebtProduct>().GetAllAsync(x => x.DebtId == debtId, x => x.Product);
            var maps = mapper.Map<List<ProductLine>>(lines);
            return maps;
        }

        public async Task MatchProductWithDebtsAsync(Guid debtId,Guid productId,int quantity)
        {
            var debtProduct = new DebtProduct {DebtId = debtId,ProductId = productId,Quantity = quantity };
            debtProduct.CreatedBy = _user.GetLoggedInUserEmail();
            await unitOfWork.GetRepository<DebtProduct>().AddAsync(debtProduct);
        }
        public DebtAddDto GetDebtAddDto()
        {
            var dto = new DebtAddDto();
            return dto;
        }
        public async Task AddDebtAsync(DebtAddDto dto) {
            var debt = mapper.Map<Debt>(dto);
            var user = await userManager.FindByEmailAsync(_user.GetLoggedInUserEmail());
            debt.StoreId = user.StoreId;
            debt.CreatedBy = _user.GetLoggedInUserEmail();
            await repository.AddAsync(debt);
            foreach (ProductLine line in dto.ProductLines)
            {
                await MatchProductWithDebtsAsync(debt.Id,line.ProductId,line.Quantity);
            }
            //decreaseing stock
            await stockService.UpdateStockAsync(debt);
            await unitOfWork.SaveAsync();
        }

        public async Task<DebtDto> GetDebtByIdAsync(Guid Id)
        {
            var debt = await repository.GetAsync(x => x.Id == Id);
            var map = mapper.Map<DebtDto>(debt);
            map.ProductLines = await GetMatchingProductsAsync(map.Id);
            return map;
        }

        public async Task PayDebtAsync(DebtPaidDto dto)
        {
            var debt = await repository.GetByGuidAsync(dto.Id);
            var map = mapper.Map(dto, debt);
            await repository.UpdateAsync(map);
            await unitOfWork.SaveAsync();
        }
    }
}
