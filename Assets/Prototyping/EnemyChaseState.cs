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
	public class EnemyChaseState : EnemyBaseState
	{
		//Constructor doesn't need touching.
		public EnemyChaseState(ControllerComponent _object) : base(_object) { }

		//Constructor doesn't need touching.
		public EnemyChaseState(BaseState _state) : base(_state) { }

		//The map where should you go from this state.
		//I might change this dictionary another time to something else as it's very annoying to format
		protected override void InitializeMap()
		{
			//Event type (this one is Input events)
			m_eventStateMap[EventArchetype.Internal] = new Dictionary<EventSubtype, Dictionary<EventFlag, Type>>
			{
				//Event subtype for chosen type (Input)
				{
					//I.e. Input.Move...
					EventSubtype.PlayerInRange, new Dictionary<EventFlag, Type>
					{
						//Event flags
						//I.e. Input.Move.KeyDown...
						{ EventFlag.Empty, typeof(EnemyAttackState) }
					}
				},

				{
					//I.e. Input.Move...
					EventSubtype.PlayerLost, new Dictionary<EventFlag, Type>
					{
						//Event flags
						//I.e. Input.Move.KeyDown...
						{ EventFlag.Empty, typeof(EnemyPatrolState) }
					}
				}
			};
		}

		public override void Update()
		{
			//Does base state functionality. 
			base.Update();

			//See comments in "Base" template for what should be done here (but in this case it applies only to this state).
			m_gameObject.m_agent.SetDestination(m_player.transform.position);

			if (Physics.CheckSphere(m_gameObject.transform.position, m_attackRange, m_gameObject.m_playerMask))
			{
				GameEvent ev = new GameEvent(EventArchetype.Internal, EventSubtype.PlayerInRange, EventPriority.Urgent, EventFlag.Empty, this);
				m_gameObject.m_handler.Enqueue(ev);
			}

			if(!Physics.CheckSphere(m_gameObject.transform.position, m_sightRange, m_gameObject.m_playerMask))
			{
				GameEvent ev = new GameEvent(EventArchetype.Internal, EventSubtype.PlayerLost, EventPriority.Urgent, EventFlag.Empty, this);
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
	}
}
