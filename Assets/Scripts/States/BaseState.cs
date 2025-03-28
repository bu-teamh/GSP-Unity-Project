using System;
using System.Collections.Generic;
using System.Reflection;
using GSP.Events;
using GSP.InputHandling;
using UnityEngine;

using GSP.Mediator;
using GSP.Controller;

namespace GSP.States
{
	public class BaseState
	{
		protected ControllerComponent m_gameObject;

		protected Dictionary<
			EventArchetype,
			Dictionary<
				EventSubtype,
				Dictionary<
					EventFlag,
					Type
					>
				>
			> m_eventStateMap = new Dictionary<
				EventArchetype,
				Dictionary<
					EventSubtype,
					Dictionary<
						EventFlag,
						Type
						>
					>
				>();

		protected bool m_initialized = false;

		public BaseState(ControllerComponent _object)
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

		public void Initialize()
		{
				GetMediations();
				InitializeMap();
		}

		//This method must be implementated in the inherited <Component>BaseState class.
		protected virtual void GetMediations() { }

		//This method must be implemented in the inherited <Component><Behaviour>State classes (unique to individual states).
		protected virtual void InitializeMap() { }

		public virtual void Update() { }

		public virtual void FixedUpdate() { }

		#nullable enable
		public BaseState? QueryNextState(GameEvent _event)
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
							if (m_eventStateMap[_event.m_type][_event.m_subtype].ContainsKey(_event.m_flag))
							{
								Type state = m_eventStateMap[_event.m_type][_event.m_subtype][_event.m_flag];

								nextState = (BaseState)Activator.CreateInstance(state, this);
							}
						}
					}
				}
				else
				{
					//bug to handle, becos this eventStateMap shouldnt be null in normal behaviour
				}
			}
	
			return nextState;
		}
		#nullable disable
	}
}
