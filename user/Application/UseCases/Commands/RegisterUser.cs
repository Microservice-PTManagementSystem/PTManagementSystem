using AutoMapper;
using PTManagementSystem.Application.Interfaces;
using PTManagementSystem.Domain.Entities;
using PTManagementSystem.Presentation.DTOs;
using FluentValidation.Results;
using PTManagementSystem.Application.Services;
using PTManagementSystem.Application.Helpers;
using PTManagementSystem.Domain.Enums;

namespace PTManagementSystem.Application.UseCases.Commands
{
    public class RegisterUser
    {
        private readonly IKeycloakService _keycloakService;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IUserValidationService _validationService;


        public RegisterUser(
            IKeycloakService keycloakService,
            IUserRepository userRepository,
            IMapper mapper,
            IUserValidationService validationService)
        {
            _keycloakService = keycloakService;
            _userRepository = userRepository;
            _mapper = mapper;
            _validationService = validationService;
        }
        public async Task RegisterAsync(KeycloakRegisterDto dto)
        {
            var payload = JwtHelper.DecodePayload(dto.token);

            var sub = payload["sub"]?.ToString();
            var email = payload["email"]?.ToString();
            var name = payload["name"]?.ToString();
            var emailVerified = payload["email_verified"]?.ToString()?.ToLower() == "true";

            if (string.IsNullOrEmpty(sub) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(name))
            {
                throw new Exception("There is missing information in the token.");
            }

            if (!Enum.TryParse<UserType>(dto.role, true, out var userType))
            {
                throw new Exception("Invalid user role.");
            }

            var existingUser = await _userRepository.GetUserByIdAsync(sub);
            if (existingUser != null)
            {
               Console.WriteLine("User already exists in database."); 
                return ;
            }

            var registerDto = new RegisterUserDto
            {
                KeycloakUserId = sub,
                Email = email,
                Name = name,
                IsEmailConfirmed = emailVerified,
                Role = userType
            };

            var validationResult = await _validationService.ValidateUserExistsAsync(registerDto.KeycloakUserId);
            if (!validationResult.IsValid)
            {
                var errorMessages = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
                throw new ArgumentException($"User already existing or invalid data: {errorMessages}");
            }

            var result = await _userRepository.AddUserAsync(registerDto);
            if (!result)
            {
                throw new Exception("User could not be registered.");
            }

            
            var roleAssigned = await _keycloakService.AssignRoleAsync(sub, dto.role);
            if (!roleAssigned)
            {
                throw new Exception("Role assignment failed.");
            }
        }

    }
}
