using System.Collections;
using System.Collections.Generic;
using GSP.Events;
using GSP.States;
using GSP.Timer;
using UnityEngine;

namespace GSP.Interface
{
	public class UIManager : UIManagerInterface, StateBasedEntityInterface
	{
		private UIManagerComponent m_component;

		private StateMachineInterface m_stateMachine;
		private LocalEventHandlerInterface m_handler;

		private InitialState m_initialState = InitialState.UIInitialState;

		private Dictionary<TimerType, GameTimer> m_timers = new();

		public LocalEventHandlerInterface Handler => m_handler;
		public InitialState InitialState => m_initialState;

		public UIManager(UIManagerComponent _component)
		{
			m_component = _component;

			m_stateMachine = new StateMachine(this);
			m_handler = new LocalEventHandler();

			m_handler.Subscribe(EventArchetype.MenuInput);
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

	}
}
