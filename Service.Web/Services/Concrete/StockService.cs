using AutoMapper;
using Cantin.Data.Repository.Abstract;
using Cantin.Data.UnitOfWorks;
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

        private IRepository<Stock> repository { get => unitOfWork.GetRepository<Stock>(); }

        public StockService(IUnitOfWork unitOfWork, IMapper mapper,IStoreService storeService)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            this.storeService = storeService;
        }
        public async Task AddStockToStoreAsync(Supply supply)
        {
            Stock stock = mapper.Map<Stock>(supply);
            var baseStock = await repository.GetAsync(x => x.Product.Id == stock.ProductId);
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

        public async Task UpdateStockForSaleAsync(SaleAddDto dto, Guid storeId)
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
        public async Task<List<Stock>> GetStocksOfAnStore(Guid storeId) {
            List<Stock> stocks = await unitOfWork.GetRepository<Stock>().GetAllAsync(x => x.StoreId == storeId,x => x.Product);
            return stocks;
        }

        public async Task<List<StockDto>> GetAllStocksIncludingStores()
        {
            var stockDtos = new List<StockDto>();
            var stores = await storeService.GetAllStoreDtosNonDeleted();
            var dtos = mapper.Map<List<StoreDto>>(stores);
            foreach (var storeDto in dtos)
            {
                var stockDto = mapper.Map<StockDto>(storeDto);
                var stocks = await GetStocksOfAnStore(storeDto.Id);
                var stockLines = mapper.Map<List<StockLine>>(stocks);
                stockDto.StockLines = stockLines;
                stockDtos.Add(stockDto);
            }
            return stockDtos;
        }
    }
}
