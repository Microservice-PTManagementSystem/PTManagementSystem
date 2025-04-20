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
            CreateMap<RegisterUserDto, User>();
            CreateMap<UpdateUserDto, User>();

            CreateMap<SpecializationDto, Specialization>();
            CreateMap<Specialization, SpecializationDto>();

            CreateMap<UserProfile, UserProfileDto>();
            CreateMap<UserProfileDto, UserProfile>();

            CreateMap<PersonalInfo, PersonalInfoDto>();
            CreateMap<PersonalInfoDto, PersonalInfo>();

            CreateMap<ContactInfo, ContactInfoDto>();
            CreateMap<ContactInfoDto, ContactInfo>();

            CreateMap<Address, AddressDto>();
            CreateMap<AddressDto, Address>();

            CreateMap<PaymentInfo, PaymentInfoDto>();
            CreateMap<PaymentInfoDto, PaymentInfo>();

            CreateMap<TrainerProfile, TrainerProfileDto>();
            CreateMap<TrainerProfileDto, TrainerProfile>();

            CreateMap<Certification, CertificationDto>();
            CreateMap<CertificationDto, Certification>();

            CreateMap<TimeSlot, TimeSlotDto>();
            CreateMap<TimeSlotDto, TimeSlot>();
        }
    }
}