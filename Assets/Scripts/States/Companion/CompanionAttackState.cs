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
	public class CompanionAttackState : CompanionBaseState
	{
		//Constructor doesn't need touching.
		public CompanionAttackState(ControllerComponent _object) : base(_object) { }

		//Constructor doesn't need touching.
		public CompanionAttackState(BaseState _state) : base(_state) { }

		//The map where should you go from this state.
		//I might change this dictionary another time to something else as it's very annoying to format
		protected override void InitializeMap()
		{
			SetTransition(typeof(CompanionIdleState), EventArchetype.Input, EventSubtype.Shoot, EventFlag.KeyUp);
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
			//Collider [] EnemiesInRange = Physics.OverlapSphere(m_gameObject.transform.position, m_attackRange, m_gameObject.m_enemyMask);
			//foreach (Collider collider in EnemiesInRange)
			//{
			//	Debug.Log(collider.name);
			//}
			m_lineRenderer.enabled = false;
			Debug.Log("Attack");
			m_gameObject.m_volume.weight = 0f;
			Time.timeScale = 1.0f;
			m_characterController.Move(m_transform.forward * m_accel * 2 * Time.fixedDeltaTime);

			return;
		}

		//You can add your own methods if you need. Like i.e. DoBigMathsThing() But only this state will be able to use it, because the StateMachine/CurrentState is hidden/encapsulted.
		//I'm thinking of a workaround for get/set. 
	}
}
