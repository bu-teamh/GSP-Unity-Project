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
using GSP.Timer;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

namespace GSP.States
{
	public class PlayerBaseState : BodyBaseState
	{
		// --- --- --- ---
		// attributes for component state (health, etc) are defined here


		// physics attributes (pos, rot, speed etc) for fixed update

		protected float m_acceleration = 40;
		protected float m_deceleration = 10;
		protected float m_maxSpeed = 15;
		protected float m_maxRotSpeed = 1080;
		protected float m_rotDamping = 5;
		protected float m_dampingThreshold = 10;

		//stored stuff

		//this bool will eventually be handled by the gamestate manager
		protected bool m_combatActive;

		protected Vector3 m_velocity = Vector3.zero;
		protected Quaternion m_targetRot;

		//define attributes for mediated objects need to know about here

		protected InputManagerComponentInterface m_inputManager;
		protected HashSet<ControllerComponent> m_enemies;

		// --- --- --- ---

		public PlayerBaseState(ControllerComponent _object) : base(_object) { }

		public PlayerBaseState(BaseState _state) : base(_state) { }

		protected override void GetMediations()
		{
			m_inputManager = (InputManagerComponentInterface)m_thisObject.m_mediatedObjects[MediatedObject.InputManager];
			m_enemies = m_thisObject.m_mediatedGroups[MediatedGroup.Enemies];
		}

		protected override void Awake()
		{
			//m_timerMap[TimerType.CombatOver] = new GameTimer(5.0f);
		}

		public override void Update()
		{
			base.Update();

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

			//send combat event

			return;
		}

		public override void React(GameEvent _event)
		{
			if (CompareEvent(_event, EventArchetype.Gameplay, EventSubtype.Teleport))
			{
				ControllerComponent teleport = (ControllerComponent)_event.m_subject;

				m_thisObject.m_characterController.enabled = false;
				//m_thisObject.m_characterController.Move(teleport.transform.position);
				m_thisObject.transform.rotation = teleport.transform.rotation;
				m_thisObject.transform.position = teleport.transform.position;
				m_thisObject.m_characterController.enabled = true;
				
			}
		}

		public override void FixedUpdate()
		{
			base.FixedUpdate();

			//rotation, always calculated
			if (m_velocity != Vector3.zero)
			{
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

			return;
		}
	}
}
