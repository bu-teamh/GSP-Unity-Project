using System.Collections;
using System.Collections.Generic;
using System;
using System.Diagnostics.Contracts;
using GSP.Controller;
using GSP.Events;
using GSP.Mediator;
using UnityEngine;
using GSP.Timer;

namespace GSP.States
{
	public class GameStateManager : GameStateManagerInterface, StateBasedEntityInterface
	{
		public GameStateManagerComponent m_component;

		private StateMachineInterface m_stateMachine;
		private LocalEventHandlerInterface m_handler;

		private Dictionary<GlobalValue, int> m_globalValues = new();
		private InitialState m_initialState = InitialState.GameInitialState;

		private Dictionary<TimerType, GameTimer> m_timers = new();

		public LocalEventHandlerInterface Handler => m_handler;
		public InitialState InitialState => m_initialState;

		public GameStateManager(GameStateManagerComponent _component)
		{
			m_component = _component;

			m_stateMachine = new StateMachine(this);
			m_handler = new LocalEventHandler();

			m_handler.Subscribe(EventArchetype.Input);

			m_globalValues[GlobalValue.PlayerHealth] = 100;
			m_globalValues[GlobalValue.PlayerCharge] = 0;
		}

		public void Start()
		{
			m_stateMachine.Start(m_initialState);
		}

		public void Update()
		{
			GameEvent ev = null;

			if (m_handler.Dequeue(ref ev))
			{
				m_stateMachine.Process(ev);
			}

			//update attributes
			m_stateMachine.Update();
		}

		public void FixedUpdate()
		{
			//do physics
			m_stateMachine.FixedUpdate();
		}

		public Type GetState()
		{
			return m_stateMachine.GetState();
		}

		#nullable enable
		public int? GetValue(GlobalValue _attribute)
		{
			int? value = null;

			if (m_globalValues.ContainsKey(_attribute))
			{
				value = m_globalValues[_attribute];
			}
			else
			{
				//error, no such attribute stored
			}

			return value;
		}
		#nullable disable
	}
}
