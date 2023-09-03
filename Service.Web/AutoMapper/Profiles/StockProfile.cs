using AutoMapper;
using Cantin.Entity.Entities;

namespace Cantin.Service.AutoMapper.Profiles
{
	public class StockProfile : Profile
	{
        public StockProfile()
        {
            CreateMap<Supply, Stock>();
        }
    }
}
