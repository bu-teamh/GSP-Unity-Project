using System;
using System.Collections.Generic;
using System.Reflection;
using GSP.Events;
using GSP.InputHandling;
using UnityEngine;

using GSP.Mediator;
using GSP.Controller;
using UnityEngine.UIElements;

namespace GSP.States
{
	public class CompanionIdleState : CompanionBaseState
	{
		public CompanionIdleState(ControllerComponent _object) : base(_object) { }

		public CompanionIdleState(BaseState _state) : base(_state) { }

		protected override void InitializeMap()
		{
			/*
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
			*/
		}

		public override void Update()
		{
			// does base class update method
			base.Update();

			//if block, if event = w, do x, else do y, nextstate = z
			// this state inherits from base state and theefore this should have functionality that should be only done during specific state
			// on top of general logic

			//you should not instruct the gameobject to go to a specific state from here:
			//if it is called for, you need to send an event like so:
			// GameEvent ev = new GameEvent(params);
			// m_gameObject.m_handler.Enqueue(ev)
			// and then add that event type to state map to react to that event in this state

			return;
		}

		public override void FixedUpdate()
		{
			//set velocity for float toward player
			base.FixedUpdate();

			float distance = Vector3.Distance(m_gameObject.transform.position, m_player.transform.position);

			Vector3 direction = (m_player.transform.position - m_gameObject.transform.position).normalized;

			if (distance > m_maxDist)
			{
				m_velocity += direction * m_accel * Time.deltaTime;
				m_velocity = Vector3.ClampMagnitude(m_velocity, m_maxSpeed);
			}
			else if (distance < m_minDist)
			{
				m_velocity -= direction * (m_accel * m_repelAccelMultplr) * Time.deltaTime;
				m_velocity = Vector3.ClampMagnitude(m_velocity, m_maxSpeed);
			}
			else
			{
				m_velocity = Vector3.Lerp(m_velocity, Vector3.zero, m_decel * Time.deltaTime);
			}

			//--------------------- move char controller
			//clamp on y plane
			//currently clamped to zero but at some point must consider factoring height from player height not ground height (otherwise on tall shit the lantern might clamp to ground if floats over edge)
			m_velocity.y = 0.0f;

			float vely = (m_hovHeight - m_gameObject.transform.position.y) * m_accel * Time.deltaTime;

			vely = Mathf.Clamp(vely, -m_maxSpeed, m_maxSpeed);

			m_velocity.y = vely;

			m_gameObject.m_chararacterController.Move(m_velocity * Time.deltaTime);

			return;
		}
	}
}
