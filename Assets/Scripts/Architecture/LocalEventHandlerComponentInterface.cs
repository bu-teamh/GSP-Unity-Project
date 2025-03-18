namespace GSP.Events
{
    public interface LocalEventHandlerComponentInterface
    {
        bool HasEvents();

        GameEvent Dequeue();

        void Enqueue(GameEvent _ev);

        void Dispatch(GameEvent _ev);

        void Subscribe(EventArchetype _type);

        void Unsubscribe();
    }
}