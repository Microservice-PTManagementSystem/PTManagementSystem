namespace PTManagementSystem.Application.Interfaces
{
    public interface IMessageBroker
    {
        Task PublishAsync<T>(T @event) where T : class;
        Task SubscribeAsync<T>(Func<T, Task> handler) where T : class;
    }
}
