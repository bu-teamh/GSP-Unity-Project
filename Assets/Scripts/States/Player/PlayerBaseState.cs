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
using UnityEngine.Rendering;

namespace GSP.States
{
	public class PlayerBaseState : BaseState
	{
		// --- --- --- ---
		// attributes for component state (health, etc) are defined here


		// physics attributes (pos, rot, speed etc) for fixed update

		protected float m_epsilon = 0.0001f;

		protected float m_gravity = -3.5f;
		protected float m_currentHeight;
		protected Vector3 m_yvelocity = Vector3.zero;

		protected float m_acceleration = 40;
		protected float m_deceleration = 10;
		protected float m_maxSpeed = 15;
		protected float m_maxRotSpeed = 1080;
		protected float m_rotDamping = 5;
		protected float m_dampingThreshold = 10;

		protected Vector3 m_velocity = Vector3.zero;
		protected Quaternion m_targetRot;

		//define attributes for mediated objects need to know about here

		protected InputManagerComponentInterface m_inputManager;

		// --- --- --- ---

		public PlayerBaseState(ControllerComponent _object) : base(_object) { }

		public PlayerBaseState(BaseState _state) : base(_state) { }

		protected override void GetMediations()
		{
			m_inputManager = (InputManagerComponentInterface)m_gameObject.m_mediatedObjects[MediatedObject.InputManager];
		}

		public override void Update()
		{
			// this has functionality that should be done during ALL states
			//if block, if event = w, do x, else do y

			//this base class should never directly interrupt and change a state after doing logic, only manipulate attributes, otherwise there could be a conflict
			//if need to trigger state based on this logic
			//you should not instruct the gameobject to go to a specific state from here:
			//if it is called for, you need to send an event like so:
			// GameEvent ev = new GameEvent(params);
			// m_gameObject.m_handler.Enqueue(ev)
			// and then add that event type to state map to react to that event in this state

			//no physics to be done here!!

			return;
		}

		public override void FixedUpdate()
		{
			if (m_velocity != Vector3.zero)
			{
				Quaternion currentRot = m_gameObject.transform.rotation;

				// Apply damping to smooth out the final rotation
				if (Quaternion.Angle(currentRot, m_targetRot) < m_dampingThreshold)
				{
					m_gameObject.transform.rotation = Quaternion.Slerp(currentRot, m_targetRot, m_rotDamping);
				}
				else
				{
					m_gameObject.transform.rotation = Quaternion.RotateTowards(currentRot, m_targetRot, m_maxRotSpeed * Time.deltaTime);
					//playerModelTransform.rotation = Quaternion.Slerp(playerModelTransform.rotation, targetRotation, maxRotSpeed * Time.deltaTime);
				}
			}

			//Gravity simulation
			m_currentHeight = m_gameObject.transform.position.y;

			m_yvelocity.y += m_gravity * Time.fixedDeltaTime;

			m_yvelocity.y = Mathf.Clamp(m_yvelocity.y, m_gravity, 0.0f);

			m_gameObject.m_characterController.Move(m_yvelocity);

			if (!((m_gameObject.transform.position.y - m_currentHeight) < -m_epsilon))
			{
				m_yvelocity = Vector3.zero;
			}

			return;
		}
	}
}
