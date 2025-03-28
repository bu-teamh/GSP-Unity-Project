using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using GSP.Events;
using Unity.VisualScripting;
using System.Data.Common;
using System;
using System.Xml.Linq;

namespace GSP.States
{
	public class StateMachine : StateMachineInterface
	{
		private object m_gameObject;

		private BaseState m_currentState;
		//private BaseState m_nextState;

		private Queue<GameEvent> m_broadcasts = new Queue<GameEvent>();

		public StateMachine(object _object)
		{
			m_gameObject = _object;
		}

		public void Start(InitialState _initial)
		{
			m_currentState = (BaseState)Activator.CreateInstance(InitialStates.m_map[_initial], m_gameObject);
			m_currentState.Initialize();
		}

		public void FreeState()
		{
			m_currentState = null;
		}

		#nullable enable
		public void Process(GameEvent _ev)
		{
			BaseState? state = m_currentState.QueryNextState(_ev);

			if (state != null)
			{
				m_currentState = state;
				m_currentState.Initialize();
			}

			return;
		}
		#nullable disable

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
