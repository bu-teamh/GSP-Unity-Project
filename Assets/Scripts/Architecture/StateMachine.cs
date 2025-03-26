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
		//private BaseState m_nextState;

		private Queue<GameEvent> m_broadcasts = new Queue<GameEvent>();

		public StateMachine(
			object _object,
			InitialState _initial
			)
		{
			m_currentState = (BaseState)Activator.CreateInstance(InitialStates.m_map[_initial], _object);
			//m_nextState = m_currentState;
		}

		public void Process(GameEvent _ev)
		{
			BaseState? state = m_currentState.QueryNextState(_ev);

			if (state != null)
			{
				m_currentState = state;
			}

			return;
		}

		public void Update()
		{
			m_currentState.Update();
		}

		public void FixedUpdate()
		{
			m_currentState.FixedUpdate();
		}

		public GameEvent Dequeue()
		{
			return m_broadcasts.Dequeue();
		}
	}
}
