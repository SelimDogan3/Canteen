using AutoMapper;
using Cantin.Entity.Dtos.ManuelStockReduction;
using Cantin.Entity.Dtos.Stores;
using Cantin.Entity.Entities;

namespace Cantin.Service.AutoMapper.Profiles
{
	public class StockProfile : Profile
	{
        public StockProfile()
        {
            CreateMap<Supply, Stock>();
            CreateMap<Stock, StockLine>();
            CreateMap<StoreDto, StockDto>();
            CreateMap<ManuelStockReductionAddDto,ManuelStockReduction>();
        }
    }
}
