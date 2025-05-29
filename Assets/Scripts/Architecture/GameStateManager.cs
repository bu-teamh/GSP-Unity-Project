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

		private Dictionary<GlobalValue, GlobalValueBundle> m_globalValues = new();
		private InitialState m_initialState = InitialState.GameInitialState;

		private Dictionary<TimerType, GameTimer> m_timers = new();

		public LocalEventHandlerInterface Handler => m_handler;
		public InitialState InitialState => m_initialState;

		public GameStateManager(GameStateManagerComponent _component)
		{
			m_component = _component;

			m_stateMachine = new StateMachine(this);
			m_handler = new LocalEventHandler();

			m_handler.Subscribe(EventArchetype.Input); // a cheeky hack because otherwise input events aren't mapped, need to fix
			m_handler.Subscribe(EventArchetype.GameplayInput);
			m_handler.Subscribe(EventArchetype.MenuInput);

			m_globalValues[GlobalValue.PlayerHealth] = new GlobalValueBundle(0, 100, 100);
			m_globalValues[GlobalValue.PlayerCharge] = new GlobalValueBundle(0, 200, 200);
			m_globalValues[GlobalValue.InventoryHealth] = new GlobalValueBundle(0, 3, 0);
			m_globalValues[GlobalValue.InventoryRevive] = new GlobalValueBundle(0, 3, 0);
			m_globalValues[GlobalValue.InventoryFuel] = new GlobalValueBundle(0, 3, 0);
			m_globalValues[GlobalValue.InventoryKey] = new GlobalValueBundle(0, 1, 0);
			m_globalValues[GlobalValue.InventoryCog] = new GlobalValueBundle(0, 1, 0);
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
				value = m_globalValues[_attribute].Value();
			}
			else
			{
				//error, no such attribute stored
			}

			return value;
		}

		public int? GetMaximum(GlobalValue _attribute)
		{
			int? maximum = null;

			if(m_globalValues.ContainsKey(_attribute))
			{
				maximum = m_globalValues[_attribute].Maximum();
			}
			else
			{
				//error, no such attribute stored
			}
			return maximum;
		}

		public int? GetMinimum(GlobalValue _attribute)
		{
			int? mimimum = null;
			if((m_globalValues.ContainsKey(_attribute)))
			{
				mimimum = m_globalValues[_attribute].Minimum();
			}
			else
			{
				//error, no such attribute stored
			}
			return mimimum;
		}
		#nullable disable

		public void AddToValue(GlobalValue _attribute, int _value)
		{
			if (m_globalValues.ContainsKey(_attribute))
			{
				m_globalValues[_attribute].Add(_value);
			}
			else
			{
				//error, no such attribute stored
			}
		}
	}
}
