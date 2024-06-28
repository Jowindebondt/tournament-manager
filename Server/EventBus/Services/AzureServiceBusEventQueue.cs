using EventBus.Events;
using Azure.Messaging.ServiceBus;
using System.Text;
using Newtonsoft.Json;

namespace EventBus.Services;

public class AzureServiceBusEventQueue : IEventQueue, IAsyncDisposable
{
    private ServiceBusClient _client;
    private List<ServiceBusProcessor> _processors = [];

    public AzureServiceBusEventQueue(string connectionString)
    {
        _client = new ServiceBusClient(connectionString);
    }

    public async Task ConsumeAsync<T>(Action<T> callback) where T : IEvent
    {
        var queueName = GetQueueName<T>();
        var options = new ServiceBusProcessorOptions
        {
            AutoCompleteMessages = false,
        };
        var processor = _client.CreateProcessor(queueName, options);
        _processors.Add(processor);
        processor.ProcessMessageAsync += async (args) =>
        {
            string body = args.Message.Body.ToString();
            callback.Invoke(JsonConvert.DeserializeObject<T>(body)!);
            await args.CompleteMessageAsync(args.Message);
        };
        await processor.StartProcessingAsync();
    }

    public async Task PublishAsync<T>(T eventObject) where T : IEvent
    {
        var queueName = GetQueueName<T>();
        var sender = _client.CreateSender(queueName);
        var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(eventObject));
        var message = new ServiceBusMessage(body);
        await sender.SendMessageAsync(message);
    }

    public async ValueTask DisposeAsync()
    {
        await DisposeAsync(true);
    }

    private async Task DisposeAsync(bool disposing)
    {
        if (!disposing)
        {
            return;
        }

        foreach (var processor in _processors)
        {
            await processor.StopProcessingAsync();
        }
        _processors.Clear();

        if (_client != null)
        {
            await _client.DisposeAsync();
            _client = null!;
        }
    }

    private static string GetQueueName<T>()
    {
        return typeof(T).Name.ToLower();
    }
}
