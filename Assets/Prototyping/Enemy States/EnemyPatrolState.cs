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
	//Rename Entity as your gameobject and Behaviour as your chosen state behaviour.
	public class EnemyPatrolState : EnemyBaseState
	{
		//Constructor doesn't need touching.
		public EnemyPatrolState(ControllerComponent _object) : base(_object) { }

		//Constructor doesn't need touching.
		public EnemyPatrolState(BaseState _state) : base(_state) { }

		//The map where should you go from this state.
		//I might change this dictionary another time to something else as it's very annoying to format
		protected override void InitializeMap()
		{
			SetTransition(typeof(EnemyChaseState), EventArchetype.Internal, EventSubtype.PlayerSpotted);
			SetTransition(typeof(EnemyDieState), EventArchetype.Internal, EventSubtype.Death);
		}

		public override void Update()
		{
			//Does base state functionality. 
			base.Update();

			//See comments in "Base" template for what should be done here (but in this case it applies only to this state).
			//Debug.Log("is patrol");

			if (!m_walkPointSet)
			{
				SearchWalkPoint();
			}
			if (m_walkPointSet)
			{
			m_gameObject.m_agent.SetDestination(m_walkPoint);
			}

			Vector3 distanceToWalkPoint = m_gameObject.transform.position - m_walkPoint;
			if (distanceToWalkPoint.magnitude < 1f)
			{
				m_walkPointSet = false;
			}

			if(Physics.CheckSphere(m_gameObject.transform.position, m_sightRange, m_gameObject.m_playerMask))
			{
				GameEvent ev = new GameEvent(EventArchetype.Internal, EventSubtype.PlayerSpotted, EventPriority.Urgent, EventFlag.None, this);
				m_gameObject.m_handler.Enqueue(ev);
			}

			return;
		}

		public override void FixedUpdate()
		{
			//Does base state physics.
			base.FixedUpdate();

			//See comments in "Base" template for what should be done here (but in this case it only applies to this state).

			return;
		}

		//You can add your own methods if you need. Like i.e. DoBigMathsThing() But only this state will be able to use it, because the StateMachine/CurrentState is hidden/encapsulted.
		//I'm thinking of a workaround for get/set.
		private void SearchWalkPoint()
		{
			float randomZ = UnityEngine.Random.Range(-m_walkPointRange, m_walkPointRange);
			float randomX = UnityEngine.Random.Range(-m_walkPointRange, m_walkPointRange);

			m_walkPoint = new Vector3(m_gameObject.transform.position.x + randomX, m_gameObject.transform.position.y, m_gameObject.transform.position.z + randomZ);

			if (Physics.Raycast(m_walkPoint, -m_gameObject.transform.up, 2f, m_gameObject.m_groundMask))
			{
				m_walkPointSet = true;
			}
		}
	}
}
