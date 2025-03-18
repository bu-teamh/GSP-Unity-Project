using GSP.Mediator;
using UnityEngine;

namespace GSP.Events
{
    public interface EventManagerComponentInterface
    {
        void Enqueue(GameEvent _ev);

        void SubscribeListener(LocalEventHandlerComponentInterface _listener, EventArchetype _type);

        void UnsubscribeListener(LocalEventHandlerComponentInterface _listener);
    }
}