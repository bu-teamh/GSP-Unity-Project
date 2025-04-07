using System;
using System.Collections.Generic;
using System.Reflection;
using GSP.Events;
using GSP.InputHandling;
using UnityEngine;

using GSP.Mediator;
using GSP.Controller;
using UnityEngine.UIElements;
using Unity.VisualScripting;

namespace GSP.States
{
	public class CompanionIdleState : CompanionBaseState
	{
		public CompanionIdleState(ControllerComponent _object) : base(_object) { }

		public CompanionIdleState(BaseState _state) : base(_state) { }

		protected override void Awake()
		{
			SetTransition(typeof(CompanionAimState), EventArchetype.Input, EventSubtype.Aim, EventFlag.KeyDown);
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
			base.FixedUpdate();

			//set velocity for float toward player


			// Base Companion Stuff
			if (m_velocity != Vector3.zero)
			{
				// Calculate the target rotation based on the direction
				m_targetRot = Quaternion.LookRotation(m_velocity);

				// Extract the y-component of the target rotation
				m_targetRot = Quaternion.Euler(0, m_targetRot.eulerAngles.y, 0);

				Quaternion currentRot = m_thisObject.transform.rotation;

				// Apply damping to smooth out the final rotation
				if (Quaternion.Angle(currentRot, m_targetRot) < m_dampingThreshold)
				{
					m_thisObject.transform.rotation = Quaternion.Slerp(currentRot, m_targetRot, m_rotDamping);
				}
				else
				{
					m_thisObject.transform.rotation = Quaternion.RotateTowards(currentRot, m_targetRot, m_maxRotSpeed * Time.fixedDeltaTime);
				}
			}
			// ---------

			//--------------------- move char controller
			//clamp on y plane to player's height
			m_velocity.y = m_player.transform.position.y;

			float vely = ((m_player.transform.position.y + m_hovHeight) - m_thisObject.transform.position.y) * m_yaccel * Time.fixedDeltaTime;

			vely = Mathf.Clamp(vely, -m_maxSpeed, m_maxSpeed);

			m_velocity.y = vely;

			m_thisObject.m_characterController.Move(m_velocity * Time.fixedDeltaTime);

			return;
		}
	}
}
