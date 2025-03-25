using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using GSP.Events;
using Unity.VisualScripting;

namespace GSP.States
{
	public class StateMachine : StateMachineInterface
	{
		private object m_gameObject;

		private BaseState m_currentState;
		private BaseState m_nextState;

		private Queue<GameEvent> m_broadcasts;

		public StateMachine(
			object _object,
			BaseState _initial
			)
		{
			m_gameObject = _object;
			m_currentState = _initial;
			m_nextState = _initial;
		}

		public void Update(GameEvent _ev)
		{
			//m_nextState = m_currentState.Update(_ev);
		}

		public bool HasEvents()
		{
			return m_broadcasts.Count > 0;
		}

		public GameEvent Dequeue()
		{
			return m_broadcasts.Dequeue();
		}
	}
}
