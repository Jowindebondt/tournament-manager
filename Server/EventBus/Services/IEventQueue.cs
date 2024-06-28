using EventBus.Events;

namespace EventBus.Services;

public interface IEventQueue
{
    Task PublishAsync<T>(T eventObject) where T : IEvent;
        
    Task ConsumeAsync<T>(Action<T> callback) where T : IEvent;
}
