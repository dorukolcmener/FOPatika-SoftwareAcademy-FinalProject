using System.Globalization;
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
                                        .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.Apartment.User.Phone))
                                        .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => $"{src.Apartment.User.Name} {src.Apartment.User.Surname}"))
                                        .ForMember(dest => dest.Type, opt => opt.MapFrom(src => Enum.GetName(typeof(BillType), src.Type)));

        CreateMap<Apartment, ApartmentViewModel>().ForMember(dest => dest.UserName, opt => opt.MapFrom(src => $"{src.User.Name} {src.User.Surname}"))
                                                  .ForMember(dest => dest.EMail, opt => opt.MapFrom(src => src.User.EMail))
                                                  .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.User.Phone));

        CreateMap<User, UserViewModel>().ForMember(dest => dest.Role, opt => opt.MapFrom(src => Enum.GetName(typeof(UserType), src.Role)));
        CreateMap<UserViewModel, User>().ForMember(dest => dest.Role, opt => opt.MapFrom(src => Enum.Parse(typeof(UserType), src.Role)));

        CreateMap<Vehicle, VehicleViewModel>().ForMember(dest => dest.UserName, opt => opt.MapFrom(src => $"{src.Owner.Name} {src.Owner.Surname}"))
                                              .ForMember(dest => dest.EMail, opt => opt.MapFrom(src => src.Owner.EMail))
                                              .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.Owner.Phone));

        CreateMap<ApartmentCreateViewModel, Apartment>();
        CreateMap<Apartment, ApartmentCreateViewModel>();

        CreateMap<BillCreateViewModel, Bill>().ForMember(dest => dest.ApartmentId, opt => opt.MapFrom(src => src.ApartmentId))
                                              .ForMember(dest => dest.DueDate, opt => opt.MapFrom(src => DateTime.ParseExact(src.DueDate, "mm/dd/yyyy", CultureInfo.InvariantCulture).ToUniversalTime()))
                                              .ForMember(dest => dest.Type, opt => opt.MapFrom(src => Enum.Parse(typeof(BillType), src.Type)));

        CreateMap<Message, MessageViewModel>().ForMember(dest => dest.From, opt => opt.MapFrom(src => src.From.EMail))
                                             .ForMember(dest => dest.To, opt => opt.MapFrom(src => src.To.EMail))
                                             .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.Date.ToString("dd/MM/yyyy HH:mm:ss")));
    }
}