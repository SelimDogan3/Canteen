using AutoMapper;
using Cantin.Entity.Dtos.Products;
using Cantin.Entity.Entities;

namespace Cantin.Service.AutoMapper.Profiles
{
	public class ProductProfile : Profile
	{
        public ProductProfile()
        {
            CreateMap<Product,ProductDto>();
            CreateMap<ProductAddDto, Product>();
            CreateMap<Product,ProductUpdateDto>().ReverseMap();

        }
    }
}
