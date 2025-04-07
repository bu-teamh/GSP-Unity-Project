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
	public class MainCameraBaseState : BaseState
	{
		// --- --- --- ---
		// attributes for component state (health, etc) are defined here

		// physics attributes (pos, rot, speed etc) for fixed update

		protected float m_heightMultiplier = 1.5f;
		protected float m_closestDist = 7.5f;
		protected float m_farthestDist = 14.0f;
		protected float m_lantMinDist = 2.5f;
		protected float m_lantMaxDist = 9.0f;
		protected float m_smoothPosSpeed = 5.0f;
		protected float m_smoothDistSpeed = 0.8f;

		//stored stuff
		protected float m_currentCamDist;

		//define attributes for mediated objects need to know about here

		protected ControllerComponent m_cameraTarget;
		protected ControllerComponent m_player;
		protected ControllerComponent m_companion;

		// --- --- --- ---

		public MainCameraBaseState(ControllerComponent _object) : base(_object) { }

		public MainCameraBaseState(BaseState _state) : base(_state) { }

		protected override void GetMediations()
		{
			m_cameraTarget = (ControllerComponent)m_thisObject.m_mediatedObjects[MediatedObject.CameraTarget];
			m_player = (ControllerComponent)m_thisObject.m_mediatedObjects[MediatedObject.Player];
			m_companion = (ControllerComponent)m_thisObject.m_mediatedObjects[MediatedObject.Companion];
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
