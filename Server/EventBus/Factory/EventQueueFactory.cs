using EventBus.Services;

namespace EventBus.Factory;

public class EventQueueFactory
{
    public static IEventQueue CreateEventQueue(EventQueueType eventQueueType, string connectionString)
    {
        return eventQueueType switch
        {
            EventQueueType.AzureServiceBus => new AzureServiceBusEventQueue(connectionString),
            EventQueueType.RabbitMQ => new RabbitMQEventQueue(connectionString),
            _ => throw new NotSupportedException($"Event queue of type {eventQueueType} is not supported")
        };
    }
}
