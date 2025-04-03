using System;
using System.Collections.Generic;
using System.Reflection;
using GSP.Events;
using GSP.InputHandling;
using UnityEngine;

using GSP.Mediator;
using GSP.Controller;
using System.Collections;
using System.Linq;
using UnityEngine.AI;
using GSP.Timer;

namespace GSP.States
{
	public class BaseState
	{
		protected ControllerComponent m_gameObject;
		protected LocalEventHandlerInterface m_handler;
		protected NavMeshAgent m_agent;
		protected object m_creator;
		protected CharacterController m_characterController;
		protected Transform m_transform;

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

		private Dictionary<
			TimerType,
			GameTimer> m_timerMap = new Dictionary<
				TimerType,
				GameTimer>();

		public BaseState(ControllerComponent _object)
		{
			m_gameObject = _object;
			m_handler = m_gameObject.m_handler;
			m_characterController = m_gameObject.m_characterController;
			m_transform = m_gameObject.transform;
			m_agent = m_gameObject.m_agent;
			m_creator = m_gameObject.m_creator;

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
			m_eventStateMap.Clear();

			GetMediations();
			InitializeMap();
			Awake();

			return;
		}

		//This method must be implementated in the inherited <Component>BaseState class.
		protected virtual void GetMediations() { }

		//This method must be implemented in the inherited <Component><Behaviour>State classes (unique to individual states).
		protected virtual void InitializeMap() { }

		//This method is called before the first Update cycle and after the transitions an mediations have been initialized. 
		protected virtual void Awake() { }

		public virtual void Update() { }

		public virtual void FixedUpdate() { }

		public HashSet<T> CastGroup<T>(HashSet<object> _group)
		{
			return new HashSet<T>(_group.Cast<T>()); // Safely cast objects to the specified type T
		}


		public void SetTransition(Type _state, EventArchetype _type, EventSubtype _subtype, EventFlag _flag = EventFlag.None)
		{
			var key = (_type, _subtype, _flag);

			m_eventStateMap[key] = _state;

			return;
		}

		public void SendExternalEvent(object _author, EventPriority _priority, EventArchetype _type, EventSubtype _subtype, EventFlag _flag = EventFlag.None)
		{
			GameEvent ev = new GameEvent(
				_type,
				_subtype,
				_priority,
				_flag,
				_author
			);

			m_handler.Dispatch(ev);

			return;
		}

		public void SendInternalEvent(object _author, EventSubtype _subtype, EventFlag _flag = EventFlag.None)
		{
			GameEvent ev = new GameEvent(
				EventArchetype.Internal,
				_subtype,
				EventPriority.Routine,
				_flag,
				_author
			);

			m_handler.Enqueue(ev);

			return;
		}

		public void SetTimer(TimerType _type, float _end)
		{
			m_timerMap[_type] = new GameTimer(_end);

			return;
		}

		public bool UpdateTimer(TimerType _type)
		{
			bool finished = false;

			if (m_timerMap.ContainsKey(_type))
			{
				finished = m_timerMap[_type].Update();
			}
			else
			{
				Debug.Log("timer wasn't set up in the dict in the first place");
			}

			return finished;
		}

		public void StartTimer(TimerType _type)
		{
			m_timerMap[_type].Start();
		}

		public void ResetTimer(TimerType _type)
		{
			m_timerMap[_type].Reset();
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

		public void SetCreator(object _creator)
		{
			m_creator = _creator;
		}
	}
}
