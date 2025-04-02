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
	public class EnemyAttackState : EnemyBaseState
	{
		//Constructor doesn't need touching.
		public EnemyAttackState(ControllerComponent _object) : base(_object) { }

		//Constructor doesn't need touching.
		public EnemyAttackState(BaseState _state) : base(_state) { }

		//The map where should you go from this state.
		//I might change this dictionary another time to something else as it's very annoying to format
		protected override void InitializeMap()
		{
			SetTransition(typeof(EnemyChaseState), EventArchetype.Internal, EventSubtype.PlayerOutRange);
			SetTransition(typeof(EnemyDieState), EventArchetype.Internal, EventSubtype.Death);
		}

		public override void Update()
		{
			//Does base state functionality. 
			base.Update();

			//See comments in "Base" template for what should be done here (but in this case it applies only to this state).

			return;
		}

		public override void FixedUpdate()
		{
			//Does base state physics.
			base.FixedUpdate();

			//See comments in "Base" template for what should be done here (but in this case it only applies to this state).

			if(!Physics.CheckSphere(m_transform.position, m_attackRange, m_gameObject.m_playerMask))
			{
				SendInternalEvent(this, EventSubtype.PlayerOutRange);
			}

			m_gameObject.m_agent.SetDestination(m_transform.position);
			m_gameObject.transform.LookAt(m_player.transform);

			if (!m_alreadyAttacked)
			{
				AttackType();

				m_alreadyAttacked = true;
			}

			if (m_alreadyAttacked && m_timer < m_timerTime)
			{
				m_timer++;
			}
			else
			{
				m_timer = 0;
				m_alreadyAttacked = false;
			}

			return;
		}

		//You can add your own methods if you need. Like i.e. DoBigMathsThing() But only this state will be able to use it, because the StateMachine/CurrentState is hidden/encapsulted.
		//I'm thinking of a workaround for get/set.
		private void ResetAttack()
		{
			m_alreadyAttacked = false;
		}

		private void AttackType()
		{
			// Creates a Random 0 - 1 Value (Melee = 0, Ranged = 1)
			if(m_attackType == 0)
			{
				Debug.Log("Melee Attack");
			}
			else
			{
				Debug.Log("Ranged Attack");
				Vector3 m_projectilePos = m_transform.position;
				m_projectilePos.z += 2;

				GameObject prefab = GameObject.Instantiate(m_projectilePrefab, m_projectilePos, m_transform.rotation);
			}
		}
	}
}
