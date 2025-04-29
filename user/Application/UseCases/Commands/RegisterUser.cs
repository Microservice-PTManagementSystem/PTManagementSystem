using AutoMapper;
using PTManagementSystem.Application.Interfaces;
using PTManagementSystem.Domain.Entities;
using PTManagementSystem.Presentation.DTOs;
using FluentValidation.Results;
using PTManagementSystem.Application.Services;
using PTManagementSystem.Domain.Enums;
using user.Presentation.DTOs;

namespace user.Application.UseCases.Commands
{
    public class RegisterUser
    {
        private readonly IKeycloakService _keycloakService;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly UserValidationService _validationService;

        public RegisterUser(
            IKeycloakService keycloakService,
            IUserRepository userRepository,
            IMapper mapper,
            UserValidationService validationService)
        {
            _keycloakService = keycloakService;
            _userRepository = userRepository;
            _mapper = mapper;
            _validationService = validationService;
        }

        public async Task RegisterAsync(KeycloakRegisterDto dto)
        {
            Console.WriteLine("GELEN REGISTER KEYCLOAK EVENT: " + dto.KeycloakUserId);

            // Validation
            //await _validationService.ValidateUserDoesNotExistAsync(dto.KeycloakUserId);

            // Keycloak user info
            var keycloakUser = await _keycloakService.GetUserInfoAsync(dto.KeycloakUserId);
            Console.WriteLine("GELEN REGISTER KEYCLOAK USER BİLGİLERİ: " + keycloakUser);
            if (keycloakUser == null)
            {
                throw new Exception("Keycloak user not found.");
            }

            //Keycloak role
            var roles = await _keycloakService.GetUserRolesIncludingGroupsAsync(dto.KeycloakUserId);
            Console.WriteLine("GELEN REGISTER KEYCLOAK ROLELERI:");
            foreach (var role in roles)
            {
                Console.WriteLine("- " + role);
            }


            if (roles == null || !roles.Any())
            {
                throw new Exception("No roles found for this user.");
            }


            UserType userType;
            if (roles.Contains("Trainer"))
                userType = UserType.TRAINER;
            else if (roles.Contains("Client"))
                userType = UserType.CLIENT;
            else
                throw new Exception("Invalid user role.");


            var registerDto = new RegisterUserDto
            {
                KeycloakUserId = dto.KeycloakUserId,
                Email = keycloakUser.Email,
                IsEmailConfirmed = keycloakUser.EmailVerified,
                UserType = userType
            };


            var result = await _userRepository.AddUserAsync(registerDto);
            if (!result)
            {
                throw new Exception("Kullan�c� kaydedilemedi.");
            }
        }

    }
}
