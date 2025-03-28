#nullable enable

using System.Collections;
using System.Collections.Generic;
using UnityEditor.Search;
using UnityEngine;

namespace GSP.Events
{
	public class LocalEventHandler : LocalEventHandlerInterface
	{
		private EventManagerComponentInterface m_eventManager;

		private Queue<GameEvent> m_eventQueue;

		public LocalEventHandler()
		{
			m_eventManager = EventManagerComponent.Instance;

			m_eventQueue = new Queue<GameEvent>();
		}

		public void PumpEvents()
		{
			m_eventQueue.Clear();
		}

        public bool HasEvents()
        {
            return m_eventQueue.Count > 0;
        }

        public bool Dequeue(ref GameEvent? _ev)
        {
			bool flag = false;

			if (m_eventQueue.Count > 0)
			{
				_ev = m_eventQueue.Dequeue();

				flag = true;
			}

			return flag;
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
