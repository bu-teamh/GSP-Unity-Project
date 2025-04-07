using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Xml.Linq;

using UnityEngine;

//Include if this state listens out for input:
using GSP.InputHandling;

using GSP.Events;
using GSP.Mediator;
using GSP.Controller;
using GSP.Timer;
using UnityEngine.UI;

namespace GSP.States
{
	//Replace "Entity" with game object name in the class name
	public class ChargeBaseState : BaseState
	{
		//Define constant state attributes here (like health)

		protected float m_current = 0.0f;
		protected float m_max = 100.0f;
		protected Color m_color;

		//And your constant physics attributes

		protected float m_lerpSpeed;
		protected Vector3 m_direction;

		//And any variables you need to store stuff to be persistent over state (like currentRot or something)

		protected Image m_image;

		//Define attributes for mediated objects listed in Inspector here

		protected ControllerComponent m_companion; // If it's a game object, it should be type ControllerComponent...
		protected GameObject m_mainCamera;

		//Constructor doesn't need touching
		public ChargeBaseState(ControllerComponent _object) : base(_object) { }

		//Second constructor doesn't need touching
		public ChargeBaseState(BaseState _state) : base(_state) { }

		//Here, assign the mediated objects like so
		protected override void GetMediations()
		{
			m_companion = (ControllerComponent)m_thisObject.m_mediatedObjects[MediatedObject.Companion];
		}

		protected override void Awake()
		{
			m_mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
			m_image = m_thisObject.GetComponentInChildren<Image>();
			m_current = 0.0f;
		}

		public override void Update()
		{
			//This function has functionality that should be executed across *all* states

			//This base method should never directly interrupt and change a state after doing logic, only manipulate attributes, otherwise there could be a conflict
			//If need to trigger a new state, tou need to send an event like so:
			// GameEvent ev = new GameEvent(params); << create your event, see that class for constructor arguments 
			// m_gameObject.m_handler.Enqueue(ev) << send it to this component's event queue 
			// and then add that event type to state map to react to that event in the states

			//no physics to be done here!!

			return;
		}

		public override void FixedUpdate()
		{
			//Physics for all states. Not often needed but for instance I used it to rotate the player to direction in which it's moving at all times.
			m_direction = (m_thisObject.transform.position - m_mainCamera.transform.position);
			m_thisObject.transform.rotation = Quaternion.LookRotation(m_direction);
		}
	}
}
