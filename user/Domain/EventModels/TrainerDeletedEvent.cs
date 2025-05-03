using System;
using PTManagementSystem.Domain.Enums;
namespace PTManagementSystem.Domain.EventModels
{
    public class TrainerDeletedEvent 
    {
        public Guid TrainerId { get; }
        public DateTime OccurredOn { get; }

        public TrainerDeletedEvent(Guid trainerId)
        {
            TrainerId = trainerId;
            OccurredOn = DateTime.UtcNow;
        }
    }
}