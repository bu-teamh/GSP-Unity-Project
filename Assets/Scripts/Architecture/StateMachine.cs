#nullable enable

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using GSP.Events;
using Unity.VisualScripting;
using System.Data.Common;
using System;

namespace GSP.States
{
	public class StateMachine : StateMachineInterface
	{
		private BaseState m_currentState;
		private BaseState m_nextState;

		private Queue<GameEvent> m_broadcasts = new Queue<GameEvent>();

		public StateMachine(
			object _object,
			InitialState _initial
			)
		{
			m_currentState = (BaseState)Activator.CreateInstance(InitialStates.m_map[_initial], _object);
			m_nextState = m_currentState;
		}

		public void Update(GameEvent? _ev = null)
		{
			m_nextState = m_currentState.Update(_ev);
		}

		public void FixedUpdate()
		{

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
