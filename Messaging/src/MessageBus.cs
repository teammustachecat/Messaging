using System;
using System.Collections.Generic;

namespace MustacheCat.Core.Messaging {
  public class MessageBus : IMessageBus {

    protected Dictionary<Type, ICollection<Delegate>> listeners;

    public MessageBus() {
       listeners = new Dictionary<Type, ICollection<Delegate>>();
    }

    public Action Subscribe<TEvent>(Action<TEvent> action) {
      var eventType = typeof(TEvent);
      ICollection<Delegate> eventHandlers;

      if (listeners.ContainsKey(eventType)) {
        eventHandlers = listeners[eventType];
      } else {
        eventHandlers = NewHandlerCollection();
        listeners[eventType] = eventHandlers;
      }

      eventHandlers.Add(action);
      return () => Unsubscribe(action);
    }

    public void Unsubscribe<TEvent>(Action<TEvent> action) {
      var eventType = typeof(TEvent);
      if (listeners.ContainsKey(eventType)) {
        var eventHandlers = listeners[eventType];
        eventHandlers.Remove(action);

        if (eventHandlers.Count == 0) {
          RemoveHandlerCollection(eventType);
        }
      }
    }

    public void Trigger<TEvent>(TEvent evt) {
      var eventType = typeof(TEvent);
      if (listeners.ContainsKey(eventType)) {
        var eventHandlers = listeners[eventType];
        foreach (var handler in eventHandlers) {
          ((Action<TEvent>)handler).Invoke(evt);
        }
      }
    }

    protected ICollection<Delegate> NewHandlerCollection() {
      return new HashSet<Delegate>();
    }

    protected void RemoveHandlerCollection(Type eventType) {
          listeners.Remove(eventType);
    }
  }
}
