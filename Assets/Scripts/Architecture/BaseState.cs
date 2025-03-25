#nullable enable

using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using GSP.Events;
using UnityEngine;

namespace GSP.States
{
	public class BaseState
	{
		protected object? m_gameObject;

		protected Dictionary<
			EventArchetype,
			Dictionary<
				EventSubtype,
				BaseState
				>
			> ? m_eventStateMap;

		public BaseState(object _object)
		{
			m_gameObject = _object;
		}

		public BaseState(BaseState _state)
		{
			var fields = _state.GetType().GetFields(
				BindingFlags.Instance |
				BindingFlags.NonPublic |
				BindingFlags.Public
			);

			foreach (var field in fields)
			{
				var value = field.GetValue(_state);
				field.SetValue(this, value);
			}
		}

		protected virtual void InitialiseMap() { }

		public virtual BaseState? Update(GameEvent? _event = null)
		{
			return null;
		}

		protected BaseState? QueryNextState(GameEvent? _event)
		{
			BaseState? nextState = null;

			if (_event != null)
			{
				if (m_eventStateMap != null)
				{
					if (m_eventStateMap.ContainsKey(_event.m_type))
					{
						if (m_eventStateMap[_event.m_type].ContainsKey(_event.m_subtype))
						{
							nextState = m_eventStateMap[_event.m_type][_event.m_subtype];
						}
					}
				}
				else
				{
					//bug handle, this shouldnt be null in normal behaviour
				}
			}
	
			return nextState;
		}
	}

	public class PlayerBaseState :  BaseState
	{
		// --- --- --- ---
		// attributes for player are defined here - the child class, as mentioend earlier
		// --- --- --- ---

		public PlayerBaseState(object _object) : base(_object) { }

		public PlayerBaseState(BaseState _state) : base(_state) { }

		protected override void InitialiseMap() { }

		public override BaseState? Update(GameEvent? _event = null)
		{
			//this base class should never return a state after doing logic, only manipulate attributes, otherwise there could be a conflict

			// this has functionality that should be done during ALL states
			//if block, if event = w, do x, else do y

			return null;
		}
	}

	public class PlayerIdleState : PlayerBaseState
	{

		public PlayerIdleState(object _object) : base(_object)
		{
			InitialiseMap();
		}

		public PlayerIdleState(BaseState _state) : base(_state)
		{
			InitialiseMap();
		}

		protected override void InitialiseMap()
		{
			//m_eventStateMap[bla] =  
		}

		public override BaseState? Update(GameEvent? _event = null)
		{
			// does base class update method
			base.Update(_event);

			//if block, if event = w, do x, else do y, nextstate = z
			// this state inherits from base state and has functionality that should be only done during specific state

			//you should not instruct the gameobject to go to a specific state from here:
			//if it is called for, you need to send an event like so:
			// GameEvent ev = new GameEvent(params);
			// m_gameObject.m_handler.Enqueue(ev)
			// and then add that event type to state map to react to that event in this state

			return QueryNextState(_event);
		}
	}
}
