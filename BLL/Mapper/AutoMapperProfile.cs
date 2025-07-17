using AutoMapper;
using DAL.Entities;
using BLL.Dto;
using BLL.Dto.Account;

namespace BLL.Mapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<CustomerDto, Customers>().
                ConstructUsing(dto => new Customers()) 
               .ForMember(dest => dest.CreatedAt, opt => opt.Ignore()).ReverseMap();
            CreateMap<CustomerConsumptionDTO, CustomerConsumptions>().ReverseMap();
            CreateMap<ActivityTypeDto, ActivityType>().ReverseMap();
            CreateMap<TariffDto, Tariff>().ReverseMap();
            CreateMap<TariffStepsDto, TariffSteps>().ReverseMap();
            CreateMap<AppUser,UserDto>().ReverseMap();


        }
    }
}
