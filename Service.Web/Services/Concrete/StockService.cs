using AutoMapper;
using Cantin.Data.Repository.Abstract;
using Cantin.Data.UnitOfWorks;
using Cantin.Entity.Dtos.Sales;
using Cantin.Entity.Entities;
using Cantin.Service.Services.Abstraction;

namespace Cantin.Service.Services.Concrete
{
	public class StockService : IStockService
	{
		private readonly IUnitOfWork unitOfWork;
		private readonly IMapper mapper;

		private IRepository<Stock> repository { get => unitOfWork.GetRepository<Stock>(); }

		public StockService(IUnitOfWork unitOfWork,IMapper mapper)
        {
			this.unitOfWork = unitOfWork;
			this.mapper = mapper;
		}
        public async Task AddStockToStoreAsync(Supply supply)
		{
			Stock stock = mapper.Map<Stock>(supply);
			var baseStock = await repository.GetAsync(x => x.Product.Id == stock.ProductId);
			if (baseStock == null) { 
			await repository.AddAsync(stock);
			}
			else
			{
				baseStock.Quantity += stock.Quantity;
			}
			await unitOfWork.SaveAsync();
		}

		public async Task UpdateStockForSaleAsync(SaleAddDto dto,Guid storeId)
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
	}
}
