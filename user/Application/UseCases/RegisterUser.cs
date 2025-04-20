using AutoMapper;
using PTManagementSystem.Application.Interfaces;
using PTManagementSystem.Domain.Entities;
using PTManagementSystem.Presentation.DTOs;
using FluentValidation.Results;
using PTManagementSystem.Application.Services;

namespace PTManagementSystem.Application.UseCases
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

        public async Task RegisterAsync(RegisterUserDto dto)
        {
            // Mapping
            var user = _mapper.Map<User>(dto);

            // Validasyon
            // ValidationResult validation = _validationService.ValidateCreateUser(user);
            // if (!validation.IsValid)
            // {
            //     var errorMessages = string.Join(" | ", validation.Errors.Select(e => e.ErrorMessage));
            //     throw new ApplicationException($"User invalid: {errorMessages}");
            // }

            // Keycloak
            var created = await _keycloakService.CreateUserAsync(
                dto.Email,
                dto.Email,
                dto.Password
            );

            if (!created)
                throw new ApplicationException("Failed to create user on Keycloak.");

            var userId = await _keycloakService.GetUserIdByEmailAsync(dto.Email);
            var role = dto.UserType.ToString().ToLower(); 

            await _keycloakService.AssignRoleAsync(userId, role);

            
           // await _userRepository.AddAsync(user);
        }
    }
}
