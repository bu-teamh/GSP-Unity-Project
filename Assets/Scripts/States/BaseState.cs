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
using System.Net.NetworkInformation;

namespace GSP.States
{
	/// <summary>
	/// Serves as the foundational state class from which all behavioral states derive common functionality.
	/// This class is designed for polymorphic use and is not intended to be instantiated directly.
	/// </summary>
	public class BaseState
	{
		/// <summary>
		/// Attribute class to mark attributes to be excluded from reflection during construction of a new state.
		/// </summary>
		[AttributeUsage(AttributeTargets.Field)]
		private class ExcludeFromReflectionAttribute : Attribute
		{
			//Utility attribute class to force the constructor to exclude fields from reflection.
		}

		/// <summary>
		/// Injected reference to the ComponentController which owns the state.
		/// </summary>
		protected StateBasedEntityInterface m_thisObject;

		/// <summary>
		/// A map used by the StateMachine (to which the state belongs) to transition state.
		/// </summary>
		[ExcludeFromReflection]
		protected Dictionary<
			(
				EventArchetype,
				EventSubtype,
				EventFlag
			),
			Type> m_eventStateMap = new();

		[ExcludeFromReflection]
		/// <summary>
		/// A siwtchable state table for use in {SetTransition()}. Assign as {typeof()}.
		/// </summary>
		protected Dictionary<
			SwitchState,
			Type> m_switchStateMap = new();

		/// <summary>
		/// Called by the StateMachine during initialisation of the ControllerComponent. Injects owning ControllerComponent into the current state.
		/// </summary>
		/// <param name="_object">Owner injected into the state during construction.</param>
		public BaseState(StateBasedEntityInterface _object)
		{
			m_thisObject = _object;

			return;
		}

		/// <summary>
		/// Called by the StateMachine during state changes. Alternative Constructor for BaseState.
		/// Accepts a previous state as an argument and reflects the its attributes onto the current state (excluding BaseState fields that should be initialized to empty).
		/// </summary>
		/// <param name="_state">Passed-in previous state.</param>
		public BaseState(BaseState _state)
		{
			var fields = _state.GetType().GetFields(
				BindingFlags.Instance |
				BindingFlags.NonPublic |
				BindingFlags.Public
			);

			foreach (var field in fields)
			{
				if (!Attribute.IsDefined(field, typeof(ExcludeFromReflectionAttribute)))
				{
					var value = field.GetValue(_state);
					field.SetValue(this, value);
				}
			}

			return;
		}

		/// <summary>
		/// Called by the StateMachine immediately after the construction of the current state.
		/// </summary>
		public void Initialize()
		{
			GetMediations();
			Awake();

			InitializeMap();

			return;
		}

		/// <summary>
		/// Called by {Initialize()} immediately after the construction of the current state. For initialisations of mediated objects and groups (see Mediator). 
		/// It should override the virtual BaseState function. Use {base.GetMediations()} to execute the functionality of the immediate parent state (if necessary).
		/// </summary>
		protected virtual void GetMediations() { }


		/// <summary>
		/// Called by {Initialize()} immediately after the construction of the current state. For initialisations or singularly necessary operations. 
		/// It should override the virtual BaseState function. Use {base.Awake()} to execute the functionality of the immediate parent state (if necessary).
		/// </summary>
		protected virtual void Awake() { }

		/// <summary>
		/// Called by {Initialize()} immediately after the construction of the current state. For initialising state change transitions. 
		/// It should override the virtual BaseState function. Use {base.Awake()} to execute the functionality of the immediate parent state (if necessary).
		/// </summary>
		protected virtual void InitializeMap() { }

		/// <summary>
		/// Called by the StateMachine during Unity's Update lifecycle method. Defines the non-physics based operations for the current state.
		/// It should override the virtual BaseState function. Use {base.Update()} to execute the functionality of the immediate parent state (if necessary).
		/// </summary>
		public virtual void Update() { }

		/// <summary>
		/// Called by the StateMachine during Unity's FixedUpdate lifecycle method. Defines the physics operations for the current state.
		/// It should override the virtual BaseState function. Use {base.FixedUpdate()} to execute the functionality of the immediate parent state (if necessary).
		/// </summary>
		public virtual void FixedUpdate() { }

		//Unused at the moment - need to sort
		public HashSet<T> CastGroup<T>(HashSet<object> _group)
		{
			return new HashSet<T>(_group.Cast<T>()); // Safely cast objects to the specified type T
		}

		/// <summary>
		/// Initialises a state change transition prior to first update cycle of the state. To be used in {InitializeMap()}.
		/// Attribute {m_switchStateMap[<SwitchState>]} can be passed as an argument so long as it is, at the least, assigned in {Awake()}.
		/// </summary>
		/// <param name="_state">Desired transition based on specified event attributes. Pass {typeof()}. Passing a class that doesn't inherit BaseState fails. </param>
		/// <param name="_type">Specified main order of event to trigger transition.</param>
		/// <param name="_subtype">Specified subtype of main order to trigger transition.</param>
		/// <param name="_flag">Specified flag of event to trigger transition. Optional argument. If not specified, {EventFlag.None} assumed.</param>
		protected void SetTransition(Type _state, EventArchetype _type, EventSubtype _subtype, EventFlag _flag = EventFlag.None)
		{
			if (typeof(BaseState).IsAssignableFrom(_state))
			{
				var key = (_type, _subtype, _flag);

				m_eventStateMap[key] = _state;
			}
			else
			{
				//someone tried to set transition to a class that doesn't derive from base state
			}
			
			return;
		}

		/// <summary>
		/// Generates an event of main order {EventType.Internal} using passed-in attributes and asyncrhonously queues it locally, bypassing Event Manager overhead.
		/// </summary>
		/// <param name="_subtype">Subtype of internal event.</param>
		/// <param name="_flag">Flag of internal event.</param>
		protected void InternalEvent(
			EventSubtype _subtype,
			EventFlag _flag = EventFlag.None
		)
		{
			SendEvent(
				EventPriority.Routine,
				EventArchetype.Internal,
				_subtype,
				_flag);

			return;
		}

		/// /// <summary>
		/// Generates an event using passed-in attributes and dispatches it to the Event Manager. Use {InternalEvent()} to bypass Event Manager overhead for local events.
		/// </summary>
		/// <param name="_priority">Priority order of event.</param>
		/// <param name="_type">Main order of event.</param>
		/// <param name="_subtype">Subtype of event.</param>
		/// <param name="_flag">Flag of event. Optional argument: if not passed, {EventFlag.None} assumed.</param>
		/// <param name="_subject">Subject of event. Optional argument: if not passed, {null} assumed.</param>
		/// <param name="_data">Data of event. Optional argument: if not passed, {null} assumed.</param>
		protected void SendEvent(
			EventPriority _priority,
			EventArchetype _type,
			EventSubtype _subtype,
			EventFlag _flag = EventFlag.None,
			object _subject = null,
			object _data = null
		)
		{
			GameEvent ev = new GameEvent(
				_type,
				_subtype,
				_priority,
				_flag,
				m_thisObject,
				_subject,
				_data
			);

			if (_type != EventArchetype.Internal)
			{
				m_thisObject.Handler.Dispatch(ev);
			}
			else
			{
				m_thisObject.Handler.Enqueue(ev);
			}

			return;
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
