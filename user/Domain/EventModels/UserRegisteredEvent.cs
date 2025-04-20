using PTManagementSystem.Domain.Enums;
using System;
using PTManagementSystem.Domain.Entities;

namespace PTManagementSystem.Domain.EventModels
{
	public class UserRegisteredEvent
	{
		public Guid UserId { get; }
		public string Email { get; }
		public UserType UserType { get; }
		public DateTime OccurredOn { get; }

		public UserRegisteredEvent(Guid userId, string email, UserType userType)
		{
			UserId = userId;
			Email = email;
			UserType = userType;
			OccurredOn = DateTime.UtcNow;
		}
	}
}