using System;
using System.Collections.Generic;
using System.Reflection;
using GSP.Events;
using GSP.InputHandling;
using UnityEngine;

using GSP.Mediator;
using GSP.Controller;
using System.Collections;

namespace GSP.States
{
	public class BaseState
	{
		protected ControllerComponent m_gameObject;

		protected Dictionary<
			(
				EventArchetype,
				EventSubtype,
				EventFlag
			),
			Type> m_eventStateMap = new Dictionary<
				(
					EventArchetype,
					EventSubtype,
					EventFlag
				),
				Type>();

		public BaseState(ControllerComponent _object)
		{
			m_gameObject = _object;

			return;
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

			return;
		}

		public void Initialize()
		{
				GetMediations();
				InitializeMap();

			return;
		}

		//This method must be implementated in the inherited <Component>BaseState class.
		protected virtual void GetMediations() { }

		//This method must be implemented in the inherited <Component><Behaviour>State classes (unique to individual states).
		protected virtual void InitializeMap() { }

		public virtual void Update() { }

		public virtual void FixedUpdate() { }

		public void SetTransition(Type _state, EventArchetype _type, EventSubtype _subtype, EventFlag _flag = EventFlag.None)
		{
			var key = (_type, _subtype, _flag);

			m_eventStateMap[key] = _state;

			return;
		}

		#nullable enable
		public BaseState? QueryNextState(GameEvent _event)
		{
			BaseState? nextState = null;

			var key = (_event.m_type, _event.m_subtype, _event.m_flag);

			// Try to retrieve the state from the flat dictionary
			if (m_eventStateMap == null)
			{
				//bug to handle, becos this eventStateMap shouldnt be null in normal behaviour
			}
			else if (m_eventStateMap.TryGetValue(key, out var typeState))
			{
				nextState = (BaseState?)Activator.CreateInstance(typeState, this);
			}
	
			return nextState;
		}
		#nullable disable
	}
}
