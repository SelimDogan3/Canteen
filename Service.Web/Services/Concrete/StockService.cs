using AutoMapper;
using Cantin.Data.Repository.Abstract;
using Cantin.Data.UnitOfWorks;
using Cantin.Entity.Dtos.Debts;
using Cantin.Entity.Dtos.ManuelStockReduction;
using Cantin.Entity.Dtos.Sales;
using Cantin.Entity.Dtos.Stores;
using Cantin.Entity.Entities;
using Cantin.Entity.Enums;
using Cantin.Service.Extensions;
using Cantin.Service.Services.Abstraction;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Cantin.Service.Services.Concrete
{
    public class StockService : IStockService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        private readonly IStoreService storeService;
        private readonly IProductService productService;
        private readonly ClaimsPrincipal _user;
        private string _userEmail => _user.GetLoggedInUserEmail();

        private IRepository<Stock> repository { get => unitOfWork.GetRepository<Stock>(); }
        private IRepository<StockHistory> historyRepository { get => unitOfWork.GetRepository<StockHistory>(); }

        public StockService(IUnitOfWork unitOfWork, IMapper mapper,IStoreService storeService,IProductService productService,IHttpContextAccessor contextAccessor)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            this.storeService = storeService;
            this.productService = productService;
            _user = contextAccessor.HttpContext.User;
        }
        public async Task AddStockToStoreAsync(Supply supply)
        {
            Stock stock = mapper.Map<Stock>(supply);
            //Getting Product and increasing Quantity if not exists then create one
            var baseStock = await repository.GetAsync(x => x.Product.Id == stock.ProductId & stock.StoreId == x.StoreId);
            if (baseStock == null)
            {
                await repository.AddAsync(stock);
            }
            else
            {
                baseStock.Quantity += stock.Quantity;
            }
            await unitOfWork.SaveAsync();
        }

        public async Task UpdateStockAsync(Sale sale)
        {
            var stocks = await repository.GetAllAsync(x => x.StoreId == sale.StoreId);
            foreach (SaleProduct line in sale.SaleProducts)
            {
                stocks.First(x => x.ProductId == line.ProductId).Quantity -= line.Quantity;
            }
            foreach (Stock stock in stocks)
            {
                await repository.UpdateAsync(stock);
            }
            await unitOfWork.SaveAsync();
        }
        public async Task UpdateStockAsync(Debt debt)
        {
            var stocks = await repository.GetAllAsync(x => x.StoreId == debt.StoreId);

            foreach (DebtProduct line in debt.DebtProducts)
            {
                stocks.First(x => x.ProductId == line.ProductId).Quantity -= line.Quantity;
            }
            foreach (Stock stock in stocks)
            {
                await repository.UpdateAsync(stock);
            }
            await AddStockHistoryAsync(StockActions.Debt, debt.Id,debt.StoreId);
            await unitOfWork.SaveAsync();
        }
        public async Task<string> UpdateStockAsync(ManuelStockReductionAddDto dto)
        {
            var stocks = await repository.GetAllAsync(x => x.StoreId == dto.StoreId);
            var stock = stocks.First(x => x.ProductId == dto.ProductId);
            if (stock.Quantity - dto.DecreasingQuantity >= 0) {
                stock.Quantity -= dto.DecreasingQuantity;
                await AddManuelStockReductionAsync(dto);
                await unitOfWork.SaveAsync();
                return "";
            }
            else {
                return "Stok Sayısı 0ın altına inemez";
            }
        }
        public async Task<StockDto> GetStocksOfAnStoreAsync(Guid storeId) {
            var stockDto = await storeService.GetStoreByIdWithStocks(storeId);
            var products = await productService.GetAllProductsNonDeletedAsync();
            var noneStocks = products.Where(x => stockDto.Stocks.Any(y => y.ProductId == x.Id)).ToList();
            return stockDto;
        }

        public async Task<List<StockDto>> GetAllStocksIncludingStoresAsync()
        {
            var stores = await storeService.GetAllStoresNonDeletedWithStocksAsync();
            var dtos = mapper.Map<List<StockDto>>(stores);
            return dtos;
        }

        public async Task AddManuelStockReductionAsync(ManuelStockReductionAddDto dto)
        {
            
            var manuelSD = mapper.Map<ManuelStockReduction>(dto);
            manuelSD.CreatedBy = _userEmail;
            await unitOfWork.GetRepository<ManuelStockReduction>().AddAsync(manuelSD);
            await AddStockHistoryAsync(StockActions.ManualReduction,manuelSD.Id,dto.StoreId);
            await unitOfWork.SaveAsync();
        }
        public async Task AddStockHistoryAsync(StockActions action, Guid id,Guid storeId)
        {

            StockHistory historyEntry = new StockHistory
            {
                Type = action,
                CreatedBy = _userEmail,
                StoreId = storeId
            };

            switch (action)
            {
                case StockActions.Supply:
                    historyEntry.SupplyId = id;
                    break;
                case StockActions.Sale:
                    historyEntry.SaleId = id;
                    break;
                case StockActions.Debt:
                    historyEntry.DebtId = id;
                    break;
                case StockActions.ManualReduction:
                    historyEntry.ManualStockReductionId = id;
                    break;
                default:
                    throw new ArgumentException("Invalid stock action type", nameof(action));
            }

            try
            {
                await historyRepository.AddAsync(historyEntry);
            }
            catch (Exception ex)
            {
                // Burada hatayı loglama veya geri dönüş yapma işlemleri gerçekleştirilebilir.
                throw; //veya özelleştirilmiş bir hata mesajı dönebilirsiniz.
            }
        }
    }
}
