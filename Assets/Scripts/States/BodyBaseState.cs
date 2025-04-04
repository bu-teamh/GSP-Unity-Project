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

namespace GSP.States
{
	//Replace "Entity" with game object name in the class name
	public class BodyBaseState : BaseState
	{
		//Define constant state attributes here (like health)

		protected float m_epsilon = 0.0001f;

		protected float m_gravity = -3.5f;
		protected float m_currentHeight;
		protected Vector3 m_yvelocity = Vector3.zero;

		//And your constant physics attributes

		//And any variables you need to store stuff to be persistent over state (like currentRot or something)

		//Define attributes for mediated objects listed in Inspector here

		//Constructor doesn't need touching
		public BodyBaseState(ControllerComponent _object) : base(_object) { }

		//Second constructor doesn't need touching
		public BodyBaseState(BaseState _state) : base(_state) { }

		//Here, assign the mediated objects like so
		protected override void GetMediations()
		{

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

			//Gravity simulation, always calculated
			m_currentHeight = m_transform.position.y;

			m_yvelocity.y += m_gravity * Time.fixedDeltaTime;

			m_yvelocity.y = Mathf.Clamp(m_yvelocity.y, m_gravity, 0.0f);

			m_characterController.Move(m_yvelocity);

			if (!((m_transform.position.y - m_currentHeight) < -m_epsilon))
			{
				m_yvelocity = Vector3.zero;
			}
		}
	}
}
