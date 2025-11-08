using AutoMapper;
using DDDExample.Application.DTOs;
using DDDExample.Domain.Entities;

namespace DDDExample.Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Product, ProductDto>().ReverseMap();
        }
    }
}
