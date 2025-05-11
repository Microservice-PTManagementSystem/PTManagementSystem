using PTManagementSystem.Application.Interfaces;
using System.Text.Json;

namespace User.Tests.Infrastructure.ExternalServices
{
    public class MockRabbitMQService : IMessageBroker
    {
        private readonly List<object> _publishedEvents = new();

        public Task PublishAsync<T>(T @event) where T : class
        {
            _publishedEvents.Add(@event);
            return Task.CompletedTask;
        }

        public Task SubscribeAsync<T>(Func<T, Task> handler) where T : class
        {
            return Task.CompletedTask;
        }

        public List<object> GetPublishedEvents()
        {
            return _publishedEvents;
        }

        public void ClearPublishedEvents()
        {
            _publishedEvents.Clear();
        }
    }
} 