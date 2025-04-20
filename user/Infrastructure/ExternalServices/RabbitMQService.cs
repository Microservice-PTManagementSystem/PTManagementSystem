using RabbitMQ.Client;
using PTManagementSystem.Application.Interfaces;
using RabbitMQ.Client.Exceptions;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace user.Infrastructure.ExternalServices

{
    public class RabbitMQService: IMessageBroker
    {
        private readonly IConnection _connection;
        private readonly RabbitMQ.Client.IModel _channel;
        private readonly IServiceProvider _serviceProvider;

        public RabbitMQService(IServiceProvider serviceProvider)
        {
            var factory = new ConnectionFactory()
            {
                HostName = "rabbitmq",
                UserName = "guest",
                Password = "guest"
            };

            int retryCount = 0;
            while (true)
            {
                try
                {
                    _connection = factory.CreateConnection();
                    _channel = _connection.CreateModel();
                    break;
                }
                catch (BrokerUnreachableException ex)
                {
                    retryCount++;
                    Console.WriteLine($"RabbitMQ connection failed. Try again ({retryCount}): {ex.Message}");
                    Thread.Sleep(5000);
                }
            }

            _serviceProvider = serviceProvider;
        }
        public Task PublishAsync<T>(T @event) where T : class
        {
            var queueName = typeof(T).Name;
            _channel.QueueDeclare(queue: queueName, durable: true, exclusive: false, autoDelete: false);

            var message = JsonSerializer.Serialize(@event);
            var body = Encoding.UTF8.GetBytes(message);

            var properties = _channel.CreateBasicProperties();
            properties.Persistent = true;

            _channel.BasicPublish(
                exchange: "",
                routingKey: queueName,
                basicProperties: properties,
                body: body
            );

            Console.WriteLine($"[RabbitMQ] Event published: {queueName}");
            return Task.CompletedTask;
        }

        public Task SubscribeAsync<T>(Func<T, Task> handler) where T : class
        {
            var queueName = typeof(T).Name;
            _channel.QueueDeclare(queue: queueName, durable: true, exclusive: false, autoDelete: false);

            var consumer = new AsyncEventingBasicConsumer(_channel);
            consumer.Received += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var json = Encoding.UTF8.GetString(body);
                var @event = JsonSerializer.Deserialize<T>(json);

                if (@event != null)
                {
                    await handler(@event);
                    _channel.BasicAck(ea.DeliveryTag, false);
                }
            };

            _channel.BasicConsume(queue: queueName, autoAck: false, consumer: consumer);

            Console.WriteLine($"[RabbitMQ] Subscribed to: {queueName}");
            return Task.CompletedTask;
        }
    }

}

