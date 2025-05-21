using AutoMapper;
using PTManagementSystem.Presentation.DTOs;
using PTManagementSystem.Domain.Entities;

namespace PTManagementSystem.Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserDto>();
            CreateMap<UserDto, User>();

            CreateMap<UserProfile, UserProfileDto>();
            CreateMap<UserProfileDto, UserProfile>();

            CreateMap<Address, AddressDto>();
            CreateMap<AddressDto, Address>();

            CreateMap<PaymentInfo, PaymentInfoDto>()
                .ForMember(dest => dest.cardHolder, opt => opt.MapFrom(src => src.CardHolderName))
                .ForMember(dest => dest.cardNumber, opt => opt.MapFrom(src => src.CardNumber))
                .ForMember(dest => dest.expiryMonth, opt => opt.MapFrom(src => src.ExpiryMonth))
                .ForMember(dest => dest.expiryYear, opt => opt.MapFrom(src => src.ExpiryYear))
                .ForMember(dest => dest.cvc, opt => opt.MapFrom(src => src.CVC));

            CreateMap<PaymentInfoDto, PaymentInfo>()
                .ForMember(dest => dest.CardHolderName, opt => opt.MapFrom(src => src.cardHolder))
                .ForMember(dest => dest.CardNumber, opt => opt.MapFrom(src => src.cardNumber))
                .ForMember(dest => dest.ExpiryMonth, opt => opt.MapFrom(src => src.expiryMonth))
                .ForMember(dest => dest.ExpiryYear, opt => opt.MapFrom(src => src.expiryYear))
                .ForMember(dest => dest.CVC, opt => opt.MapFrom(src => src.cvc));


            CreateMap<TrainerProfile, TrainerProfileDto>();
            CreateMap<TrainerProfileDto, TrainerProfile>();

            CreateMap<Certification, CertificationDto>();
            CreateMap<CertificationDto, Certification>();
        }
    }
}