using AutoMapper;
using Cantin.Entity.Dtos.Roles;
using Cantin.Entity.Entities;

namespace Cantin.Service.AutoMapper.Profiles
{
    public class RoleProfile : Profile
    {
        public RoleProfile() {
            CreateMap<AppRole,RoleDto>();
            CreateMap<RoleAddDto,AppRole > ();
            CreateMap<RoleUpdateDto,AppRole>().ReverseMap();
        }
    }
}
