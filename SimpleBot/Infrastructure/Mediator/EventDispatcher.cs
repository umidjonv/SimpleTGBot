using Microsoft.AspNetCore.Components;
using SimpleBot.Infrastructure.Events;
using SimpleBot.Infrastructure.Mediator.Events;
using SimpleBot.Infrastructure.Mediator.MQ;
using SimpleBot.Infrastructure.Mediator.Receivers;

namespace SimpleBot.Infrastructure.Mediator
{
    public interface IEventDispatcher
    {
        //bool RegisterHandler<T>(Func<T, Task> handler) where T : BaseEvent;
        List<Type> RegisterAllHandlers();
    }

    public class EventDispatcher :IEventDispatcher
    {
        private readonly List<Type> _handlers = new();
        private readonly IReceiver _receiver;

        public EventDispatcher(IReceiver receiver)
        {
            _receiver = receiver;
        }

        public List<Type> RegisterAllHandlers()
        {
            //RegisterHandler<StartEvent>(_receiver.Handle);
            //RegisterHandler<ProductEvent>(_receiver.Handle);

            _handlers.Add(typeof(StartEvent));
            _handlers.Add(typeof(ProductEvent));

            return _handlers;
        }

        //public bool RegisterHandler<T>(Func<T, Task> handler) where T : BaseEvent
        //{
        //    if (_handlers.ContainsKey(typeof(T)))
        //    {
        //        return false;
        //    }
            
        //    _handlers.Add(typeof(T), handler);

        //    return true;
        //}
    }
}
