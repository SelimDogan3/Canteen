using AutoMapper;
using Cantin.Entity.Dtos.Debts;
using Cantin.Entity.Entities;

namespace Cantin.Service.AutoMapper.Profiles
{
    public class DebtProfile : Profile
    {
        public DebtProfile() {
            CreateMap<Debt, DebtDto>() ;

            CreateMap<DebtAddDto, Debt>();
            CreateMap<DebtProduct, ProductLine>();
            CreateMap<DebtPaidDto, Debt>();
        
        }
    }
}
