using System;
using System.Collections.Generic;

namespace Client.Runtime.Utils
{
    public class EventBus : IDisposable
    {
        private Dictionary<Type, List<Delegate>> _eventHandlers = new();

        public void Subscribe<TEvent>(Action<TEvent> handler)
        {
            if (!_eventHandlers.ContainsKey(typeof(TEvent)))
            {
                _eventHandlers[typeof(TEvent)] = new List<Delegate>();
            }

            _eventHandlers[typeof(TEvent)].Add(handler);
        }

        public void Unsubscribe<TEvent>(Action<TEvent> handler)
        {
            if (_eventHandlers.ContainsKey(typeof(TEvent)))
            {
                _eventHandlers[typeof(TEvent)].Remove(handler);
            }
        }

        public void Publish<TEvent>(TEvent @event)
        {
            if (_eventHandlers.ContainsKey(typeof(TEvent)))
            {
                foreach (var handler in _eventHandlers[typeof(TEvent)])
                {
                    ((Action<TEvent>) handler)(@event);
                }
            }
        }

        public void Dispose()
        {
            _eventHandlers.Clear();
        }
    }
}