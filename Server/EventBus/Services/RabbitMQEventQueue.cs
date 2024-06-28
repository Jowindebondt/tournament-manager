
using System.Text;
using EventBus.Events;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace EventBus.Services;

public class RabbitMQEventQueue : IEventQueue, IDisposable
{
    private IConnection _connection;
    private IModel _channel;

    public RabbitMQEventQueue(string connectionString)
    {
        var factory = new ConnectionFactory
        {
            Uri = new Uri(connectionString)
        };
        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();
    }

    public async Task ConsumeAsync<T>(Action<T> callback)
        where T : IEvent
    {
        _channel.QueueDeclare(typeof(T).Name.ToLower(), true, false, false);
        var consumer = new EventingBasicConsumer(_channel);
        consumer.Received += (model, args) =>
        {
            var body = args.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            callback.Invoke(JsonConvert.DeserializeObject<T>(message)!);
            _channel.BasicAck(args.DeliveryTag, false);
        };

        _channel.BasicConsume(typeof(T).Name.ToLower(), false, consumer);
        await Task.CompletedTask;
    }

    public async Task PublishAsync<T>(T eventObject)
        where T : IEvent
    {
        _channel.QueueDeclare(typeof(T).Name.ToLower(), true, false, false);
        var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(eventObject));
        var properties = _channel.CreateBasicProperties();
        properties.Persistent = true;
        _channel.BasicPublish(string.Empty, typeof(T).Name.ToLower(), properties, body);
        await Task.CompletedTask;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    private void Dispose(bool disposing)
    {
        if (!disposing)
        {
            return;
        }

        if (_channel != null)
        {
            _channel.Close();
            _channel = null!;
        }

        if (_connection != null)
        {
            _connection.Close();
            _connection = null!;
        }
    }
}
