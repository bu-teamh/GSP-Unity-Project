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
	public class CompanionBaseState : BaseState
	{
		// --- --- --- ---
		// attributes for component state (health, etc) are defined here


		// physics attributes (pos, rot, speed etc) for fixed update
		public LayerMask groundLayer;

		protected float m_maxDist = 9.0f;
		protected float m_minDist = 2.5f;
		protected float m_maxSpeed = 15.0f;
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
		//protected float m_attackRange = 5.0f;  

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
			m_player = (ControllerComponent)m_gameObject.m_mediatedObjects[MediatedObject.Player];
			m_inputManager = (InputManagerComponentInterface)m_gameObject.m_mediatedObjects[MediatedObject.InputManager];

			m_lineRenderer = m_gameObject.GetComponent<LineRenderer>();
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
		}
	}
}
