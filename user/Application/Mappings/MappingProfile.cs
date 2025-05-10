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

            CreateMap<PaymentInfo, PaymentInfoDto>();
            CreateMap<PaymentInfoDto, PaymentInfo>();

            CreateMap<TrainerProfile, TrainerProfileDto>();
            CreateMap<TrainerProfileDto, TrainerProfile>();

            CreateMap<Certification, CertificationDto>();
            CreateMap<CertificationDto, Certification>();
        }
    }
}