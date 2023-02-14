using ApartmentManagement.Entities;
using ApartmentManagement.Models;
using AutoMapper;

namespace ApartmentManagement.Common;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Bill, BillViewModel>().ForMember(dest => dest.Block, opt => opt.MapFrom(src => src.Apartment.Block))
                                        .ForMember(dest => dest.Floor, opt => opt.MapFrom(src => src.Apartment.Floor))
                                        .ForMember(dest => dest.Door, opt => opt.MapFrom(src => src.Apartment.Door))
                                        .ForMember(dest => dest.EMail, opt => opt.MapFrom(src => src.Apartment.User.EMail))
                                        .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => $"{src.Apartment.User.Name} {src.Apartment.User.Surname}"))
                                        .ForMember(dest => dest.Type, opt => opt.MapFrom(src => Enum.GetName(typeof(BillType), src.Type)));
    }
}