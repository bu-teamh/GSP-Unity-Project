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
using UnityEngine.UIElements;

namespace GSP.States
{
	//Replace "Entity" with game object name in the class name
	public class EnemyBaseState : BaseState
	{
		//Define constant state attributes here (like health)


		//And your constant physics attributes

		protected float m_walkPointRange = 5.0f;
		//protected float m_attackDelay = 0.5f;
		protected float m_sightRange = 9.0f;
		protected float m_attackRange = 3.0f;


		//And any variables you need to store stuff to be persistent over state (like currentRot or something)

		protected Vector3 m_walkPoint;
		protected bool m_walkPointSet;
		protected bool m_alreadyAttacked;
		protected bool m_hasBuff = true;

		protected int m_timer = 0;
		protected int m_timerTime = 50;
		protected int m_attackType = UnityEngine.Random.Range(0, 2);

		//Define attributes for mediated objects listed in Inspector here

		protected ControllerComponent m_player; // If it's a game object, it should be type ControllerComponent...
		protected ControllerComponent m_companion;
		protected NavMeshComponent m_navMesh;
		protected GameObject m_projectilePrefab = Resources.Load<GameObject>("Projectile");
		protected HashSet<ControllerComponent> m_projectiles = new HashSet<ControllerComponent>();

		//Constructor doesn't need touching
		public EnemyBaseState(ControllerComponent _object) : base(_object) { }

		//Second constructor doesn't need touching
		public EnemyBaseState(BaseState _state) : base(_state) { }

		//Here, assign the mediated objects like so
		protected override void GetMediations()
		{
			m_player = (ControllerComponent)m_gameObject.m_mediatedObjects[MediatedObject.Player];
			m_companion = (ControllerComponent)m_gameObject.m_mediatedObjects[MediatedObject.Companion];
			m_navMesh = (NavMeshComponent)m_gameObject.m_mediatedObjects[MediatedObject.NavMesh];
			m_projectiles = m_gameObject.m_mediatedGroups[MediatedGroup.Projectiles];
		}

		protected override void Awake()
		{
			if(m_attackType == 1) { m_attackRange = m_sightRange; }
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
