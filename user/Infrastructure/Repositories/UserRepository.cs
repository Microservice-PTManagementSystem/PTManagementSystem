using MongoDB.Driver;
using PTManagementSystem.Application.Interfaces;
using PTManagementSystem.Domain.Entities;
using PTManagementSystem.Infrastructure.Data;
using PTManagementSystem.Presentation.DTOs;

namespace PTManagementSystem.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IMongoCollection<User> _users;

        public UserRepository(UserDbContext dbContext)
        {
            _users = dbContext.Users;
        }

        public async Task<bool> AddUserAsync( RegisterUserDto registerDto)
        {
            var user = new User
            {
                Id = Guid.NewGuid(),
                KeycloakId = registerDto.KeycloakUserId,
                Email = registerDto.Email,
                IsEmailConfirmed = registerDto.IsEmailConfirmed,
                UserType = registerDto.UserType,
                Profile = new UserProfile(),
                PaymentInfo = new PaymentInfo(),
                TrainerProfile = new TrainerProfile()
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

        public async Task<UserDto> GetUserByIdAsync(string userId)
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

        public async Task<UserDto> UpdateUserAsync(string userId, UserDto updateDto)
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

        public async Task<UserProfileDto> GetUserProfileAsync(string userId)
        {
            var user = await _users.Find(u => u.KeycloakId == userId).FirstOrDefaultAsync();
            if (user == null || user.Profile == null) return null;

            return new UserProfileDto
            {
           
            };
        }

        public async Task UpdateUserProfileAsync(string userId, UserProfile profileDto)
        {
            var filter = Builders<User>.Filter.Eq(u => u.KeycloakId, userId);
            var update = Builders<User>.Update
                .Set(u => u.Profile, new UserProfile
                {
                    PersonalInfo = new PersonalInfo
                    {
                        FirstName = profileDto.PersonalInfo.FirstName,
                        LastName = profileDto.PersonalInfo.LastName,
                        DateOfBirth = profileDto.PersonalInfo.DateOfBirth,
                        Gender = profileDto.PersonalInfo.Gender
                    },
                    ContactInfo = new ContactInfo
                    {
                        Email = profileDto.ContactInfo.Email,
                        Phone = profileDto.ContactInfo.Phone
                    },
                    Address = new Address
                    {
                        Street = profileDto.Address.Street,
                        City = profileDto.Address.City,
                        State = profileDto.Address.State,
                        Country = profileDto.Address.Country,
                        PostalCode = profileDto.Address.PostalCode
                    }
                });

            var result = await _users.FindOneAndUpdateAsync(filter, update, new FindOneAndUpdateOptions<User> { ReturnDocument = ReturnDocument.After });

            //if (result == null) return null;

            //return profileDto;
        }

        public async Task<PaymentInfoDto> GetPaymentInfoAsync(string userId)
        {
            var user = await _users.Find(u => u.KeycloakId == userId).FirstOrDefaultAsync();
            if (user == null || user.PaymentInfo == null) return null;

            return new PaymentInfoDto
            {
                // Burayı da UseCase'de maplemek istiyorsan boş dönebiliriz
            };
        }

        public async Task UpdatePaymentInfoAsync(string userId, PaymentInfo paymentDto)
        {
            var filter = Builders<User>.Filter.Eq(u => u.KeycloakId, userId);
            var update = Builders<User>.Update
                .Set(u => u.PaymentInfo, new PaymentInfo
                {
                    CardHolderName = paymentDto.CardHolderName,
                    CardNumber = paymentDto.CardNumber,
                    ExpirationMonth = paymentDto.ExpirationMonth,
                    ExpirationYear = paymentDto.ExpirationYear,
                    Cvv = paymentDto.Cvv,
                    BillingAddress = paymentDto.BillingAddress
                });

            var result = await _users.FindOneAndUpdateAsync(filter, update, new FindOneAndUpdateOptions<User> { ReturnDocument = ReturnDocument.After });

            //if (result == null) return null;

            //return paymentDto;
        }

        public async Task<TrainerProfileDto> GetTrainerProfileAsync(string trainerId)
        {
            var user = await _users.Find(u => u.KeycloakId == trainerId).FirstOrDefaultAsync();
            if (user == null || user.TrainerProfile == null) return null;

            return new TrainerProfileDto
            {
                // Mapping yine UseCase'de
            };
        }

        public async Task UpdateTrainerProfileAsync(string trainerId, TrainerProfile trainerDto)
        {
            var filter = Builders<User>.Filter.Eq(u => u.KeycloakId, trainerId);
            var update = Builders<User>.Update
                .Set(u => u.TrainerProfile, new TrainerProfile
                {
                    Specializations = trainerDto.Specializations.Select(s => new Specialization { Value = s.Value }).ToList(),
                    Certifications = trainerDto.Certifications.Select(c => new Certification
                    {
                        Name = c.Name,
                        IssuingAuthority = c.IssuingAuthority,
                        IssueDate = c.IssueDate,
                        ExpiryDate = c.ExpiryDate
                    }).ToList(),
                    //AvailableSlots = trainerDto.AvailableSlots.Select(a => new TimeSlot
                    //{
                    //    DayOfWeek = a.DayOfWeek,
                    //    StartTime = a.StartTime,
                    //    EndTime = a.EndTime
                    //}).ToList(),
                    YearsOfExperience = trainerDto.YearsOfExperience,
                    HourlyRate = trainerDto.HourlyRate
                });

            var result = await _users.FindOneAndUpdateAsync(filter, update, new FindOneAndUpdateOptions<User> { ReturnDocument = ReturnDocument.After });

            //if (result == null) return null;

            //return trainerDto;
        }

        public async Task<IEnumerable<UserDto>> GetAllTrainersAsync()
        {
            var trainers = await _users.Find(u => u.UserType == Domain.Enums.UserType.TRAINER).ToListAsync();
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
