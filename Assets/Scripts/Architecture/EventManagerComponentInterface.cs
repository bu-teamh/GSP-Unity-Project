using GSP.Mediator;
using UnityEngine;

namespace GSP.Events
{
    public interface EventManagerComponentInterface
    {
        void Enqueue(GameEvent _ev);

        void SubscribeListener(LocalEventHandlerInterface _listener, EventArchetype _type);

        void UnsubscribeListener(LocalEventHandlerInterface _listener);
    }
}
