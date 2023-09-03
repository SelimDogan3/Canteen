using AutoMapper;
using Cantin.Entity.Dtos.Users;
using Cantin.Entity.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cantin.Service.AutoMapper.Profiles
{
	public class UserProfile : Profile
	{
		public UserProfile()
		{
			CreateMap<AppUser, UserDto>();
			CreateMap<UserAddDto,AppUser>();
			CreateMap<AppUser, UserUpdateDto>().ReverseMap();
		}
	}
}
