using AutoMapper;
using Cantin.Entity.Dtos.Stores;
using Cantin.Entity.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cantin.Service.AutoMapper.Profiles
{
	public class StoreProfile : Profile
	{
        public StoreProfile()
        {
            CreateMap<Store,StoreDto>();
            CreateMap<StoreAddDto, Store>();
            CreateMap<StoreUpdateDto, Store>().ReverseMap();

        }
    }
}
