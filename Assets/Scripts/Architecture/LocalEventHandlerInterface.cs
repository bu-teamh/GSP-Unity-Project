#nullable enable

namespace GSP.Events
{
    public interface LocalEventHandlerInterface
    {
        bool HasEvents();

		bool Dequeue(ref GameEvent? _ev);

		void Enqueue(GameEvent _ev);

        void Dispatch(GameEvent _ev);

        void Subscribe(EventArchetype _type);

        void Unsubscribe();
    }
}
