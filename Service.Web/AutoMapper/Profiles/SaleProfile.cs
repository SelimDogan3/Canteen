using AutoMapper;
using Cantin.Entity.Dtos.Sales;
using Cantin.Entity.Entities;

namespace Cantin.Service.AutoMapper.Profiles
{
	public class SaleProfile : Profile
	{
        public SaleProfile()
        {
            CreateMap<Sale, SaleDto>();
            CreateMap<SaleAddDto, Sale>();
            CreateMap<SaleUpdateDto, Sale>().ReverseMap();
        }
    }
}
