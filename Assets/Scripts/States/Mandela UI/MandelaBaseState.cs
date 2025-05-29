using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Xml.Linq;

using UnityEngine;


using GSP.Events;
using GSP.Mediator;
using GSP.Controller;
using GSP.Timer;
using UnityEngine.UI;
using UnityEngine.Rendering.Universal;

namespace GSP.States
{
	//Replace "Entity" with game object name in the class name
	public class MandelaBaseState : EntityBaseState
	{
		//Define constant state attributes here (like health)
		protected Color m_color;

		//And your constant physics attributes

		protected float m_lerpSpeed = 0.0f;
		protected Vector3 m_direction;

		//And any variables you need to store stuff to be persistent over state (like currentRot or something)

		protected Image m_image;
		protected Vignette m_vignette;

		//Define attributes for mediated objects listed in Inspector here

		protected ControllerComponent m_player; // If it's a game object, it should be type ControllerComponent...
		protected GameStateManagerComponentInterface m_gameStateManager;
		protected ControllerComponent m_mainCamera;

		//Constructor doesn't need touching
		public MandelaBaseState(ControllerComponent _object) : base(_object) { }

		//Second constructor doesn't need touching
		public MandelaBaseState(BaseState _state) : base(_state) { }

		//Here, assign the mediated objects like so
		protected override void GetMediations()
		{
			m_player = (ControllerComponent)m_thisObject.m_mediatedObjects[MediatedObject.Player];
			m_gameStateManager = (GameStateManagerComponentInterface)m_thisObject.m_mediatedObjects[MediatedObject.GameStateManager];
			m_mainCamera = (ControllerComponent)m_thisObject.m_mediatedObjects[MediatedObject.MainCamera];
		}

		protected override void Awake()
		{
			m_image = m_thisObject.GetComponentInChildren<Image>();
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
		}
	}
}
