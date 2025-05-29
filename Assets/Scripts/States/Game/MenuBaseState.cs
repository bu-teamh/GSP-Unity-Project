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

namespace GSP.States
{
	//Replace "Entity" with game object name in the class name
	public class MenuBaseState : GameBaseState
	{
		//Define constant state attributes here (like health)

		//Constructor doesn't need touching
		public MenuBaseState(GameStateManager _object) : base(_object) { }

		//Second constructor doesn't need touching
		public MenuBaseState(BaseState _state) : base(_state) { }

		//Here, assign the mediated objects like so
		protected override void GetMediations()
		{

		}

		protected override void InitializeMap()
		{

		}

		protected override void Awake()
		{
			SendEvent(EventPriority.Urgent, EventArchetype.Gameplay, EventSubtype.Pause, EventFlag.Active);
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
