using MongoDB.Driver;
using PTManagementSystem.Application.Interfaces;
using PTManagementSystem.Domain.Entities;
using PTManagementSystem.Infrastructure.Data;
using PTManagementSystem.Presentation.DTOs;
using PTManagementSystem.Domain.Enums;

namespace PTManagementSystem.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IMongoCollection<User> _users;

        public UserRepository(UserDbContext dbContext)
        {
            _users = dbContext.Users;
        }

        public async Task<bool> AddUserAsync(RegisterUserDto registerDto)
        {
            var user = new User
            {
                Id = Guid.NewGuid(),
                KeycloakId = registerDto.KeycloakUserId,
                Email = registerDto.Email,
                Name = registerDto.Name,
                IsEmailConfirmed = registerDto.IsEmailConfirmed,
                UserType = registerDto.Role,
                UserProfile = new UserProfile(),
                PaymentInfo = new PaymentInfo(),
                TrainerProfile = registerDto.Role == UserType.TRAINER ? new TrainerProfile() : null
            };

            await _users.InsertOneAsync(user);
            return true;
        }

        public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
        {
            var users = await _users.Find(_ => true).ToListAsync();
            return users.Select(u => new UserDto
            {
                Id = u.Id,
                Email = u.Email,
                KeycloakId = u.KeycloakId,
                IsEmailConfirmed = u.IsEmailConfirmed,
                UserType = u.UserType
            });
        }

        public async Task<UserDto?> GetUserByIdAsync(string userId)
        {
            var user = await _users.Find(u => u.KeycloakId == userId).FirstOrDefaultAsync();
            if (user == null) return null;

            return new UserDto
            {
                Id = user.Id,
                Email = user.Email,
                KeycloakId = user.KeycloakId,
                IsEmailConfirmed = user.IsEmailConfirmed,
                UserType = user.UserType
            };
        }

        public async Task<UserDto?> UpdateUserAsync(string userId, UserDto updateDto)
        {
            var filter = Builders<User>.Filter.Eq(u => u.KeycloakId, userId);
            var update = Builders<User>.Update
                .Set(u => u.Email, updateDto.Email)
                .Set(u => u.IsEmailConfirmed, updateDto.IsEmailConfirmed)
                .Set(u => u.UserType, updateDto.UserType);

            var result = await _users.FindOneAndUpdateAsync(filter, update, new FindOneAndUpdateOptions<User> { ReturnDocument = ReturnDocument.After });

            if (result == null) return null;

            return new UserDto
            {
                Id = result.Id,
                Email = result.Email,
                KeycloakId = result.KeycloakId,
                IsEmailConfirmed = result.IsEmailConfirmed,
                UserType = result.UserType
            };
        }

        public async Task<bool> DeleteUserAsync(string userId)
        {
            var result = await _users.DeleteOneAsync(u => u.KeycloakId == userId);
            return result.DeletedCount > 0;
        }

        public async Task<UserProfileDto?> GetUserProfileAsync(string userId)
        {
            var user = await _users.Find(u => u.KeycloakId == userId).FirstOrDefaultAsync();
            if (user == null || user.UserProfile == null) return null;

            return new UserProfileDto
            {
                //FirstName = user.UserProfile.FirstName,
                //LastName = user.UserProfile.LastName,
                PhoneNumber = user.UserProfile.PhoneNumber,
                DateOfBirth = user.UserProfile.DateOfBirth,
                Gender = user.UserProfile.Gender,
                Height = user.UserProfile.Height,
                Weight = user.UserProfile.Weight,
                FitnessGoals = user.UserProfile.FitnessGoals,
                MedicalConditions = user.UserProfile.MedicalConditions,
                Allergies = user.UserProfile.Allergies,
                Address = new AddressDto
                {
                    Street = user.UserProfile.Address.Street,
                    City = user.UserProfile.Address.City,
                    State = user.UserProfile.Address.State,
                    Country = user.UserProfile.Address.Country,
                    PostalCode = user.UserProfile.Address.PostalCode
                }
            };
        }

        public async Task UpdateUserProfileAsync(string userId, UserProfile profile)
        {
            var filter = Builders<User>.Filter.Eq(u => u.KeycloakId, userId);
            var update = Builders<User>.Update
                .Set(u => u.UserProfile, profile);

            await _users.FindOneAndUpdateAsync(filter, update);
        }

        public async Task<PaymentInfoDto?> GetPaymentInfoAsync(string userId)
        {
            var user = await _users.Find(u => u.KeycloakId == userId).FirstOrDefaultAsync();
            if (user == null || user.PaymentInfo == null) return null;

            return new PaymentInfoDto
            {
                cardHolder= user.PaymentInfo.CardHolderName,
                cardNumber = user.PaymentInfo.CardNumber,
                expiryMonth = user.PaymentInfo.ExpiryMonth,
                expiryYear = user.PaymentInfo.ExpiryYear,
                cvc = user.PaymentInfo.CVC,
               // BillingAddress = user.PaymentInfo.BillingAddress
            };
        }

        public async Task UpdatePaymentInfoAsync(string userId, PaymentInfo paymentInfo)
        {
            var filter = Builders<User>.Filter.Eq(u => u.KeycloakId, userId);
            var update = Builders<User>.Update
                .Set(u => u.PaymentInfo, paymentInfo);

            await _users.FindOneAndUpdateAsync(filter, update);
        }

        public async Task<TrainerProfileDto?> GetTrainerProfileAsync(string trainerId)
        {
            var user = await _users.Find(u => u.KeycloakId == trainerId).FirstOrDefaultAsync();
            if (user == null || user.TrainerProfile == null) return null;

            return new TrainerProfileDto
            {
                Specialization = user.TrainerProfile.Specialization,
                ExperienceYears = user.TrainerProfile.ExperienceYears,
                Certifications = user.TrainerProfile.Certifications.Select(c => new CertificationDto
                {
                    Name = c.Name,
                    IssuingAuthority = c.IssuingAuthority,
                    IssueDate = c.IssueDate,
                    ExpiryDate = c.ExpiryDate
                }).ToList(),
                Bio = user.TrainerProfile.Bio,
                HourlyRate = user.TrainerProfile.HourlyRate,
                AvailableDays = user.TrainerProfile.AvailableDays,
                AvailableHours = user.TrainerProfile.AvailableHours
            };
        }

        public async Task UpdateTrainerProfileAsync(string trainerId, TrainerProfile trainerProfile)
        {
            var filter = Builders<User>.Filter.Eq(u => u.KeycloakId, trainerId);
            var update = Builders<User>.Update
                .Set(u => u.TrainerProfile, trainerProfile);

            await _users.FindOneAndUpdateAsync(filter, update);
        }

        public async Task<IEnumerable<UserDto>> GetAllTrainersAsync()
        {
            var trainers = await _users.Find(u => u.UserType == UserType.TRAINER).ToListAsync();
            return trainers.Select(t => new UserDto
            {
                Id = t.Id,
                Email = t.Email,
                KeycloakId = t.KeycloakId,
                IsEmailConfirmed = t.IsEmailConfirmed,
                UserType = t.UserType
            });
        }
    }
}
