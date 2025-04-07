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
	public class GameplayBaseState : GameBaseState
	{
		//Define constant state attributes here (like health)

		//And your constant physics attributes

		protected float m_entityRadius = 20.0f;

		//And any variables you need to store stuff to be persistent over state (like currentRot or something)

		protected HashSet<ControllerComponent> m_localEnemies = new();

		//Define attributes for mediated objects listed in Inspector here

		//Constructor doesn't need touching
		public GameplayBaseState(GameStateManager _object) : base(_object) { }

		//Second constructor doesn't need touching
		public GameplayBaseState(BaseState _state) : base(_state) { }

		//Here, assign the mediated objects like so

		public override void Update()
		{
			//This function has functionality that should be executed across *all* states

			//This base method should never directly interrupt and change a state after doing logic, only manipulate attributes, otherwise there could be a conflict
			//If need to trigger a new state, tou need to send an event like so:
			// GameEvent ev = new GameEvent(params); << create your event, see that class for constructor arguments 
			// m_gameObject.m_handler.Enqueue(ev) << send it to this component's event queue 
			// and then add that event type to state map to react to that event in the states

			//no physics to be done here!

			return;
		}

		public override void FixedUpdate()
		{
			// regional check for enemies nearby, always updated
			Collider[] localObjects = Physics.OverlapSphere(m_player.transform.position, m_entityRadius);

			m_localEnemies.Clear();

			foreach (var collider in localObjects)
			{
				var controller = collider.GetComponentInParent<ControllerComponent>(); // << the enemey character controller does counts as a collider

				if (controller != null && m_enemies.Contains(controller))
				{
					m_localEnemies.Add(controller); // only add valid controllers that are in m_enemies
				}
			}
		}
	}
}
