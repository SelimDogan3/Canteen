using AutoMapper;
using Cantin.Entity.Dtos.Supplies;
using Cantin.Entity.Entities;

namespace Cantin.Service.AutoMapper.Profiles
{
    public class SupplyProfile : Profile
	{
		public SupplyProfile() {
			CreateMap<Supply,SupplyDto>();
			CreateMap<SupplyAddDto,Supply>();
			CreateMap<Supply, SupplyUpdateDto>().ReverseMap();
			CreateMap<Supply, SupplyLine>();
		
		
		}
	}
}
