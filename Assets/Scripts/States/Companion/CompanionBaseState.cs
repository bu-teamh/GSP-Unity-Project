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

namespace GSP.States
{
	public class CompanionBaseState : EntityBaseState
	{
		public LayerMask groundLayer;

		protected float m_maxDist;
		protected float m_minDist;
		protected float m_maxSpeed;
		protected float m_maxRotSpeed = 360.0f;
		protected float m_accel = 33.0f;
		protected float m_yaccel = 100.0f;
		protected float m_decel =3.5f;
		protected float m_repelAccelMultplr = 1.6f;
		protected float m_dampingThreshold = 5.0f;
		protected float m_rotDamping = 10.0f;
		protected float m_mouseAccel = 200.0f;
		protected float m_mouseDecel = 35.0f;
		protected float m_mouseFollowSpeed = 75.0f;
		protected float m_mouseDecelThreshold = 1.0f;
		protected float m_hovHeight = 2.0f;

		// this is for companion physics sphere (WIP)
		protected float m_attackRange = 5.0f;
		protected float m_attackTime = 0.3f;
		protected GameTimer m_timer;
		protected GameTimer m_attackDelayTimer;
		protected float m_attackDelayTime = 1.0f;

		protected bool m_canAttack = true;

		protected GameTimer m_phoebusTimer;
		protected float m_phoebusTime = 15.0f;

		protected ParticleSystem m_particleSystem;
		protected ParticleSystem.LightsModule m_lightsModule;

		protected ParticleSystem m_AOEeffect;

		//stored stuff (physics, not globals)
		protected Vector3 m_velocity = Vector3.zero;
		protected Quaternion m_targetRot;
		protected Vector3 m_lastMousePos;
		protected UnityEngine.LineRenderer m_lineRenderer;

		//define attributes for mediated objects need to know about here

		protected ControllerComponent m_player;
		protected InputManagerComponentInterface m_inputManager;
		protected GameStateManagerComponentInterface m_gameStateManager;


		// --- --- --- ---

		public CompanionBaseState(ControllerComponent _object) : base(_object) { }

		public CompanionBaseState(BaseState _state) : base(_state) { }

		protected override void GetMediations()
		{
			m_player = (ControllerComponent)m_thisObject.m_mediatedObjects[MediatedObject.Player];
			m_inputManager = (InputManagerComponentInterface)m_thisObject.m_mediatedObjects[MediatedObject.InputManager];
			m_gameStateManager = (GameStateManagerComponentInterface)m_thisObject.m_mediatedObjects[MediatedObject.GameStateManager];

		}
		protected override void Awake()
		{
			m_lineRenderer = m_thisObject.GetComponent<LineRenderer>();
			m_particleSystem = m_thisObject.GetComponentInChildren<ParticleSystem>();
			m_lightsModule = m_particleSystem.lights;
			m_attackDelayTimer = new GameTimer(m_attackDelayTime);
			m_AOEeffect = m_thisObject.m_AOE.GetComponentInChildren<ParticleSystem>();
		}

		public override void Update()
		{
			m_attackDelayTimer.Start();
			m_attackDelayTimer.Lock();

			if(m_attackDelayTimer.Check())
			{
				if(!m_canAttack)
				{
					m_canAttack = true;
				}
				m_attackDelayTimer.Unlock();
			}
			return;
		}

		public override void React(GameEvent _event)
		{
			if (CompareEvent(_event, EventArchetype.Gameplay, EventSubtype.Teleport))
			{
				ControllerComponent teleport = (ControllerComponent)_event.m_subject;

				m_thisObject.m_characterController.enabled = false;
				m_targetRot = teleport.transform.rotation;
				m_thisObject.transform.rotation = teleport.transform.rotation;

				m_thisObject.transform.position = new Vector3(
					teleport.transform.position.x,
					teleport.transform.position.y + m_hovHeight,
					teleport.transform.position.z + 3
				);

				m_thisObject.m_characterController.enabled = true;
			}
		}

		public override void FixedUpdate()
		{
		}
	}
}
