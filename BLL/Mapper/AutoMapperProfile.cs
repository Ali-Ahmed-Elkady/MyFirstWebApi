using AutoMapper;
using DAL.Entities;
using BLL.Dto;

namespace BLL.Mapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<CustomerDto, Customers>().
                ConstructUsing(dto => new Customers()) // Ensure AutoMapper uses the correct constructor
               .ForMember(dest => dest.CreatedAt, opt => opt.Ignore()).ReverseMap();
            CreateMap<CustomerConsumptionDTO, CustomerConsumptions>().ReverseMap();
            CreateMap<ActivityTypeDto, ActivityType>().ReverseMap();
            CreateMap<TariffDto, Tariff>().ReverseMap();
            CreateMap<TariffStepsDto, TariffSteps>().ReverseMap();

        }
    }
}
