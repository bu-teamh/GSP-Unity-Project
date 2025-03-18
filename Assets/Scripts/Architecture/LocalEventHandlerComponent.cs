using System.Collections;
using System.Collections.Generic;
using UnityEditor.Search;
using UnityEngine;

namespace GSP.Events
{
    public class LocalEventHandlerComponent : LocalEventHandlerComponentInterface
    {
        private EventManagerComponentInterface m_eventManager;

        private Queue<GameEvent> m_eventQueue;

        public LocalEventHandlerComponent()
        {
            m_eventManager = EventManagerComponent.Instance;

            m_eventQueue = new Queue<GameEvent>();
        }

        public bool HasEvents()
        {
            return m_eventQueue.Count > 0;
        }

        public GameEvent Dequeue()
        {
            return m_eventQueue.Dequeue();
        }

        public void Enqueue(GameEvent _ev) 
        { 
            m_eventQueue.Enqueue(_ev);
        }

        public void Dispatch(GameEvent _ev)
        {
            m_eventManager.Enqueue(_ev);
        }

        public void Subscribe(EventArchetype _type)
        {
            m_eventManager.SubscribeListener(this, _type);
        }

        public void Unsubscribe()
        {
            m_eventManager.UnsubscribeListener(this);
        }
    }
}