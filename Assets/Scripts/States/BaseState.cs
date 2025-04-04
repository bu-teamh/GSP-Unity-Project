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

		protected Type m_switchState;

		public BaseState(ControllerComponent _object)
		{
			m_gameObject = _object;
			m_handler = m_gameObject.m_handler;
			m_characterController = m_gameObject.m_characterController;
			m_transform = m_gameObject.transform;
			m_agent = m_gameObject.m_agent;
			m_creator = m_gameObject.m_creator;
			m_switchState = null;

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
			m_timerMap.Clear();
			m_switchState = null;

			GetMediations();
			Awake();

			InitializeMap();
			InitializeTimers();

			return;
		}

		//This method must be implementated in the inherited <Component>BaseState class.
		protected virtual void GetMediations() { }

		//This method must be implemented in the inherited <Component><Behaviour>State classes (unique to individual states).
		protected virtual void InitializeMap() { }

		//This method should be inimplented in the inherited <Component>BaseState class(es).
		protected virtual void InitializeTimers() { }

		//This method is called before the first Update cycle and after the transitions, mediations and timers have been initialized. 
		protected virtual void Awake() { }

		public virtual void Update() { }

		public virtual void FixedUpdate() { }

		public HashSet<T> CastGroup<T>(HashSet<object> _group)
		{
			return new HashSet<T>(_group.Cast<T>()); // Safely cast objects to the specified type T
		}


		//got to here
		/// <summary>
		/// Initialises a state change transition prior to first update cycle of the state. To be used in InitializeMap().
		/// </summary>
		/// <param name="_state">Desired transition based on specified event attributes. Pass {typeof()}. Passing a class that doesn't inherit BaseState fails. </param>
		/// <param name="_type"></param>
		/// <param name="_subtype"></param>
		/// <param name="_flag"></param>
		public void SetTransition(Type _state, EventArchetype _type, EventSubtype _subtype, EventFlag _flag = EventFlag.None)
		{
			if(_state != typeof(BaseState))
			{
				var key = (_type, _subtype, _flag);

				m_eventStateMap[key] = _state;
			}

			

			return;
		}

		/// <summary>
		/// Generates an event using passed-in attributes and dispatches it to the Event Manager. {EventArchetype.Internal} will fail.
		/// </summary>
		/// <param name="_author">Event author (pass {this}), for debugging purposes.</param>
		/// <param name="_priority">Priority order of event.</param>
		/// <param name="_type">Main order of event.</param>
		/// <param name="_subtype">Subtype of event.</param>
		/// <param name="_flag">Flag of event. Optional argument: if not passed, {EventFlag.None} assumed.</param>
		/// <param name="_subject">Subject of event. Optional argument: if not passed, {null} assumed.</param>
		/// <param name="_data">Data of event. Optional argument: if not passed, {null} assumed.</param>
		public void SendExternalEvent(
			object _author,
			EventPriority _priority,
			EventArchetype _type,
			EventSubtype _subtype,
			EventFlag _flag = EventFlag.None,
			object _subject = null,
			object _data = null
		)
		{
			if (_type == EventArchetype.Internal)
			{
				//error
			}
			else
			{
				GameEvent ev = new GameEvent(
				_type,
				_subtype,
				_priority,
				_flag,
				_author,
				_subject,
				_data
			);

				m_handler.Dispatch(ev);
			}
			
			return;
		}

		/// <summary>
		/// Generates an event of main order {EventType.Internal} using passed-in attributes and asyncrhonously queues it locally, bypassing Event Manager overhead.
		/// </summary>
		/// <param name="_author">Event author (pass {this}), for debugging purposes.</param>
		/// <param name="_subtype">Subtype of internal event.</param>
		/// <param name="_flag">Flag of internal event.</param>
		/// <param name="_priority">Priority of internal event (currently unused locally; pass {EventPriority.Routine}).</param>
		public void SendInternalEvent(
			object _author,
			EventSubtype _subtype,
			EventFlag _flag = EventFlag.None,
			EventPriority _priority = EventPriority.Routine
		)
		{
			GameEvent ev = new GameEvent(
				EventArchetype.Internal,
				_subtype,
				_priority,
				_flag,
				_author
			);

			m_handler.Enqueue(ev);

			return;
		}

		/// <summary>
		/// Initialises a timer of the passed-in type in the state's registry of timers.
		/// </summary>
		/// <param name="_type">Custom passed-in timer type (simply used to differentiate between multiple timers).</param>
		/// <param name="_end">Amount of time to durate before finishing (in seconds). Float by {Time.deltaFixedTime} will relate to amount of frames to durate (50FPS).</param>
		public void SetTimer(TimerType _type, float _end)
		{
			m_timerMap[_type] = new GameTimer(_end);

			return;
		}

		/// <summary>
		/// Recalculates the timer's time and uses it to decide whether the timer is completed.
		/// </summary>
		/// <param name="_type">The passed-in timer type.</param>
		/// <returns>Returns whether </returns>
		public bool CheckTimer(TimerType _type)
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

		/// <summary>
		/// Tries to start the timer of passed-in type initialised at the state's instantiation.
		/// </summary>
		/// <param name="_type">The passed-in timer type.</param>
		/// <returns>Returns true if the timer was started (and false if the timer had alreay been started by some other means).</returns>
		public bool StartTimer(TimerType _type)
		{
			return m_timerMap[_type].Start();
		}

		/// <summary>
		/// Tries to interrupt the passed-in timer type initialised at the state's instantiation.
		/// </summary>
		/// <param name="_type">The passed-in timer type.</param>
		/// <returns>Returns true if the timer was interrupted (and false if the timer was not yet started).</returns>
		public bool InterruptTimer(TimerType _type)
		{
			return m_timerMap[_type].Interrupt();
		}

		/// <summary>
		/// Called by the StateMachine. All dequeued events are queried against the state change tree.
		/// </summary>
		/// <param name="_event">The passed-in event.</param>
		/// <returns>Returns a generated instance of the next state (initialised using the current state).</returns>
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

		/// <summary>
		/// Called by the StateMachine. If a dequeued event doesn't trigger a state change, then the StateMachine passes it to this method for further conditional exection.
		/// </summary>
		/// <param name="_event">The passed-in event.</param>
		public virtual void React(GameEvent _event) { }

		/// <summary>
		/// Compare a certain event against passed-in event attributes to check for equivalency.
		/// </summary>
		/// <param name="_event">The event against which flags should be tested.</param>
		/// <param name="_type">Overarching event type to compare.</param>
		/// <param name="_subtype">Subtybe of overarching event to compare.</param>
		/// <param name="_flag">Flag against which to compare.</param>
		/// <returns>Returns true if there is an equivalency between the passed-in event and the passed-in flags.</returns>
		protected bool CompareEvent(GameEvent _event, EventArchetype _type, EventSubtype _subtype, EventFlag _flag = EventFlag.None)
		{
			bool equivalent = false;

			if (
				_event.m_type == _type &&
				_event.m_subtype == _subtype &&
				_event.m_flag == _flag
			)
			{
				equivalent = true;
			}

			return equivalent;
		}
	}
}
