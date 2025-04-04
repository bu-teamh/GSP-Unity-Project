#nullable enable

namespace GSP.Events
{
    public interface LocalEventHandlerInterface
    {
		void PumpEvents();

		void Listen();

		public bool Peek(ref GameEvent? _ev);

		bool Dequeue(ref GameEvent? _ev);

		void Enqueue(GameEvent _ev);

        void Dispatch(GameEvent _ev);

        void Subscribe(EventArchetype _type);

        void Unsubscribe();
    }
}
