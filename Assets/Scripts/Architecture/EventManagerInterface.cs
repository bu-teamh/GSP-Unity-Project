using System.Collections.Generic;

namespace GSP.Events
{
    public interface EventManagerInterface
    {
        void SubscribeListener(LocalEventHandlerComponentInterface _listener, EventArchetype _type);

        void UnsubscribeListener(LocalEventHandlerComponentInterface _listener);

        void ProcessQueue();

        void Enqueue(GameEvent _ev);
    }
}
