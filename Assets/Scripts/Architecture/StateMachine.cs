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
		private StateBasedEntityInterface m_thisObject;

		private BaseState m_currentState;
		//private BaseState m_nextState;

		//private Queue<GameEvent> m_broadcasts = new Queue<GameEvent>();

		public StateMachine(StateBasedEntityInterface _entity)
		{
			m_thisObject = _entity;
		}

		public void Start(InitialState _initial)
		{
			m_currentState = (BaseState)Activator.CreateInstance(InitialStates.m_map[_initial], m_thisObject);
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
			else
			{
				m_currentState.React(_ev);
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

		public Type GetState()
		{
			return m_currentState.GetType();
		}
	}
}
