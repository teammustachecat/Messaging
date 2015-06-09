using System;

namespace MustacheCat.Core.Messaging {
	public interface IMessageBus {
		Action Subscribe<TEvent>(Action<TEvent> handler);
		void Unsubscribe<TEvent>(Action<TEvent> handler);
		void Trigger<TEvent>(TEvent message);
	}
}
