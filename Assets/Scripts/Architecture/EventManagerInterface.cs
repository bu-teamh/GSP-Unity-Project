using System.Collections.Generic;

namespace GSP.Events
{
    public interface EventManagerInterface
    {
        void SubscribeListener(LocalEventHandlerInterface _listener, EventArchetype _type);

        void UnsubscribeListener(LocalEventHandlerInterface _listener);

        void ProcessQueue();

        void Enqueue(GameEvent _ev);
    }
}
