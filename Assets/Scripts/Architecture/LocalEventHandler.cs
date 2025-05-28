using System.Collections;
using System.Collections.Generic;
using UnityEditor.Search;
using UnityEngine;

using GSP.Controller;
using GSP.Triggers;
using Unity.VisualScripting;

namespace GSP.Events
{
	public class LocalEventHandler : LocalEventHandlerInterface
	{
		private EventManagerComponentInterface m_eventManager;

		private TriggerableInterface m_owner;

		private Queue<GameEvent> m_eventQueue;
		private Queue<GameEvent> m_pauseBuffer;

		public LocalEventHandler()
		{
			m_eventManager = EventManagerComponent.Instance;

			m_eventQueue = new Queue<GameEvent>();
			m_pauseBuffer = new Queue<GameEvent>();
		}

		public LocalEventHandler(TriggerableInterface _owner)
		{
			m_eventManager = EventManagerComponent.Instance;

			m_owner = _owner;

			m_eventQueue = new Queue<GameEvent>();
			m_pauseBuffer = new Queue<GameEvent>();
		}

		public void PumpEvents()
		{
			m_eventQueue.Clear();
		}

		public void Listen()
		{
			GameEvent ev = null;

			if (Peek(ref ev))
			{
				Debug.Log(m_owner + " handler peeked " + ev.m_type + ev.m_subtype + ev.m_flag + " // sent by: " + ev.m_author + " // id : " + ev.m_id);
				if (ev.m_type == EventArchetype.Trigger)
				{
					Dequeue(ref ev);

					if (ev.m_flag == EventFlag.Activate)
					{
						m_owner.Enable();
					}
					else if (ev.m_flag == EventFlag.Deactivate)
					{
						m_owner.Disable();
					}
				}

				if (
					ev.m_type == EventArchetype.Gameplay &&
					ev.m_subtype == EventSubtype.Pause)
				{
					Dequeue(ref ev);

					if (ev.m_flag == EventFlag.Active)
					{
						m_owner.Pause();

						Debug.Log(m_owner + "paused");
					}
					else if (ev.m_flag == EventFlag.Inactive)
					{
						m_owner.Unpause();
						Debug.Log(m_owner + "unpaused");
					}
				}
			}
			else { Debug.Log(m_owner + " there was nothing to peek at :( ev check was false" ); }
		}

		#nullable enable
		private bool Process(ref GameEvent? _ev, bool _dequeue)
		{
			bool flag = false;

			if (m_eventQueue.Count > 0)
			{
				_ev = _dequeue ? m_eventQueue.Dequeue() : m_eventQueue.Peek();
				flag = true;
			}

			return flag;
		}

		public bool Peek(ref GameEvent? _ev)
		{
			return Process(ref _ev, false);
		}

		public bool Dequeue(ref GameEvent? _ev)
		{
			return Process(ref _ev, true);
		}
		#nullable disable

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

		public void StashEvents()
		{
			m_pauseBuffer = new Queue<GameEvent>(m_eventQueue);
		}

		public void UnstashEvents()
		{
			m_eventQueue = new Queue<GameEvent>(m_pauseBuffer);
		}
	}
}
