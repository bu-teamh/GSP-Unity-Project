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
		protected float m_hovHeight = 3.0f;

		// this is for companion physics sphere (WIP)
		protected float m_attackRange = 5.0f;
		protected float m_attackTime = 0.3f;
		protected GameTimer m_timer;

		//stored stuff (physics, not globals)
		protected Vector3 m_velocity = Vector3.zero;
		protected Quaternion m_targetRot;
		protected Vector3 m_lastMousePos;
		protected UnityEngine.LineRenderer m_lineRenderer;

		//define attributes for mediated objects need to know about here

		protected ControllerComponent m_player;
		protected InputManagerComponentInterface m_inputManager;


		// --- --- --- ---

		public CompanionBaseState(ControllerComponent _object) : base(_object) { }

		public CompanionBaseState(BaseState _state) : base(_state) { }

		protected override void GetMediations()
		{
			m_player = (ControllerComponent)m_thisObject.m_mediatedObjects[MediatedObject.Player];
			m_inputManager = (InputManagerComponentInterface)m_thisObject.m_mediatedObjects[MediatedObject.InputManager];

		}
		protected override void Awake()
		{
			m_lineRenderer = m_thisObject.GetComponent<LineRenderer>();
		}

		public override void Update()
		{

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
