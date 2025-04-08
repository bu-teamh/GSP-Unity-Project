using GSP.Mediator;
using GSP.States;
using UnityEngine;

namespace GSP.Events
{
    public interface EventManagerComponentInterface
    {
		public GameStateManagerComponentInterface GameStateManager { get; }

		void Enqueue(GameEvent _ev);

        void SubscribeListener(LocalEventHandlerInterface _listener, EventArchetype _type);

        void UnsubscribeListener(LocalEventHandlerInterface _listener);
    }
}
