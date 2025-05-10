using PTManagementSystem.Application.Interfaces;
using PTManagementSystem.Presentation.DTOs;

namespace PTManagementSystem.Application.UseCases.Queries
{
    public class GetUser
    {
        private readonly IUserRepository _userRepository;
        //private readonly IMapper _mapper;

        public GetUser(IUserRepository userRepository)
        {
            _userRepository = userRepository;

        }

        public async Task<UserDto?> ExecuteAsync(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
                throw new ArgumentException("User ID cannot be null or empty.", nameof(userId));

            return await _userRepository.GetUserByIdAsync(userId); ;
        }
    }
}
