#nullable enable

using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Xml.Linq;
using GSP.Events;
using GSP.InputHandling;
using UnityEngine;

using GSP.Mediator;
using GSP.Controller;

namespace GSP.States
{
	public class BaseState
	{
		protected static ControllerComponent m_gameObject;

		protected Dictionary<
			EventArchetype,
			Dictionary<
				EventSubtype,
				Dictionary<
					EventFlag,
					Type
					>
				>
			> ? m_eventStateMap;

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

		protected virtual void InitialiseMap() { }

		public virtual void Update() { }

		public virtual void FixedUpdate() { }

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
	}

	public class PlayerBaseState :  BaseState
	{
		// --- --- --- ---
		// attributes for player are defined here - the child class, as mentioend earlier

		protected float m_acceleration = 40;
		protected float m_deceleration = 10;
		protected float m_maxSpeed = 15;
		protected float m_maxSpeedRot = 1080;
		protected float m_rotDapming = 5;
		protected float m_dampingThreshold = 10;

		protected Vector3 m_velocity = Vector3.zero;
		protected Quaternion m_targetRot;

		protected InputManagerComponentInterface m_inputManager = (InputManagerComponentInterface)m_gameObject.m_mediations[MediatedObject.InputManager];

		// --- --- --- ---

		public PlayerBaseState(ControllerComponent _object) : base(_object) { }

		public PlayerBaseState(BaseState _state) : base(_state) { }

		protected override void InitialiseMap() { }

		public override void Update()
		{
			//this base class should never return a state after doing logic, only manipulate attributes, otherwise there could be a conflict

			// this has functionality that should be done during ALL states
			//if block, if event = w, do x, else do y

			return;
		}

		public override void FixedUpdate() { }
	}

	public class PlayerIdleState : PlayerBaseState
	{

		public PlayerIdleState(ControllerComponent _object) : base(_object)
		{
			InitialiseMap();
		}

		public PlayerIdleState(BaseState _state) : base(_state)
		{
			InitialiseMap();
		}

		protected override void InitialiseMap()
		{
			//Event
			m_eventStateMap[EventArchetype.Input] = new Dictionary<EventSubtype, Dictionary<EventFlag, Type>>
			{
				//Subtypes
				{
					EventSubtype.Move, new Dictionary<EventFlag, Type>
					{
						{ EventFlag.KeyDown, typeof(PlayerMoveState) }
					}
				}
			};
		}

		public override void Update()
		{
			// does base class update method
			base.Update();

			//if block, if event = w, do x, else do y, nextstate = z
			// this state inherits from base state and has functionality that should be only done during specific state

			//you should not instruct the gameobject to go to a specific state from here:
			//if it is called for, you need to send an event like so:
			// GameEvent ev = new GameEvent(params);
			// m_gameObject.m_handler.Enqueue(ev)
			// and then add that event type to state map to react to that event in this state

			m_velocity = Vector3.Lerp(m_velocity, Vector3.zero, m_deceleration * Time.deltaTime);

			m_gameObject.m_chararacterController.Move(m_velocity * Time.deltaTime);

			return;
		}

		public override void FixedUpdate()
		{

			return;
		}
	}

	public class PlayerMoveState : PlayerBaseState
	{

		public PlayerMoveState(ControllerComponent _object) : base(_object)
		{
			InitialiseMap();
		}

		public PlayerMoveState(BaseState _state) : base(_state)
		{
			InitialiseMap();
		}

		protected override void InitialiseMap()
		{
			//Event
			m_eventStateMap[EventArchetype.Input] = new Dictionary<EventSubtype, Dictionary<EventFlag, Type>>
			{
				//Subtypes
				{
					EventSubtype.Move, new Dictionary<EventFlag, Type>
					{
						{ EventFlag.KeyUp, typeof(PlayerIdleState) }
					}
				}
			};
		}

		public override void Update()
		{
			base.Update();

			return;
		}

		public override void FixedUpdate()
		{
			System.Numerics.Vector2 axisState = m_inputManager.GetDualAxisState(EventSubtype.Move);

			Vector3 rawDirection = new Vector3(-axisState.X, 0.0f, axisState.Y);

			Quaternion rotation = Quaternion.Euler(0.0f, -45.0f, 0.0f);

			Vector3 offsetDirection = rotation * rawDirection;

			m_velocity += offsetDirection * m_acceleration * Time.deltaTime;
			m_velocity = Vector3.ClampMagnitude(m_velocity, m_maxSpeed);

			//clamp on y plane
			m_velocity.y = 0.0f;

			m_targetRot = Quaternion.LookRotation(m_velocity);

			m_gameObject.m_chararacterController.Move(m_velocity * Time.deltaTime);

			return;
		}
	}
}
