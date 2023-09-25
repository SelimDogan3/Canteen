using AutoMapper;
using Cantin.Data.Repository.Abstract;
using Cantin.Data.UnitOfWorks;
using Cantin.Entity.Dtos.Debts;
using Cantin.Entity.Dtos.Sales;
using Cantin.Entity.Dtos.Stores;
using Cantin.Entity.Entities;
using Cantin.Service.Services.Abstraction;

namespace Cantin.Service.Services.Concrete
{
    public class StockService : IStockService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        private readonly IStoreService storeService;
        private readonly IProductService productService;

        private IRepository<Stock> repository { get => unitOfWork.GetRepository<Stock>(); }

        public StockService(IUnitOfWork unitOfWork, IMapper mapper,IStoreService storeService,IProductService productService)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            this.storeService = storeService;
            this.productService = productService;
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

        public async Task UpdateStockAsync(SaleAddDto dto, Guid storeId)
        {
            var stocks = await repository.GetAllAsync(x => x.StoreId == storeId);
            foreach (ProductLine line in dto.ProductLines)
            {
                stocks.First(x => x.ProductId == line.ProductId).Quantity -= line.Quantity;
            }
            foreach (Stock stock in stocks)
            {
                await repository.UpdateAsync(stock);
            }
            await unitOfWork.SaveAsync();
        }
        public async Task UpdateStockAsync(DebtAddDto dto, Guid storeId)
        {
            var stocks = await repository.GetAllAsync(x => x.StoreId == storeId);
            foreach (ProductLine line in dto.ProductLines)
            {
                stocks.First(x => x.ProductId == line.ProductId).Quantity -= line.Quantity;
            }
            foreach (Stock stock in stocks)
            {
                await repository.UpdateAsync(stock);
            }
            await unitOfWork.SaveAsync();
        }
        public async Task UpdateStockAsync(Guid storeId,Guid ProductId,int Quantity)
        {
            var stocks = await repository.GetAllAsync(x => x.StoreId == storeId);
            var stock = stocks.First(x => x.ProductId == ProductId);
            stock.Quantity -= Quantity;
            await unitOfWork.SaveAsync();
        }
        public async Task<StockDto> GetStocksOfAnStoreAsync(Guid storeId) {
            var stockDto = await storeService.GetStoreByIdWithStocks(storeId);
            var products = await productService.GetAllProductsNonDeletedAsync();
            var noneStocks = products.Where(x => stockDto.Stocks.Any(y => y.ProductId == x.Id)).ToList();
            return stockDto;
        }

        public async Task<List<StockDto>> GetAllStocksIncludingStores()
        {
            var stores = await storeService.GetAllStoresNonDeletedWithStocksAsync();
            var dtos = mapper.Map<List<StockDto>>(stores);
            return dtos;
        }
    }
}
