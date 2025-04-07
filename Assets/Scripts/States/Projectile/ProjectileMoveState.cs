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
using System.Linq;

namespace GSP.States
{
	//Rename Entity as your gameobject and Behaviour as your chosen state behaviour.
	public class ProjectileMoveState : ProjectileBaseState
	{
		//Constructor doesn't need touching.
		public ProjectileMoveState(ControllerComponent _object) : base(_object) { }

		//Constructor doesn't need touching.
		public ProjectileMoveState(BaseState _state) : base(_state) { }

		//The map where should you go from this state.
		//I might change this dictionary another time to something else as it's very annoying to format
		protected override void Awake()
		{
			SetTransition(typeof(ProjectileIdleState), EventArchetype.Internal, EventSubtype.Move, EventFlag.Inactive); // << Like this now!
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

			Collider[] nearbyObjects = Physics.OverlapSphere(m_thisObject.transform.position, (m_rigidbody.transform.localScale.magnitude / 2));

			HashSet<Collider> nearbyNotProj = new HashSet<Collider>(nearbyObjects);
			HashSet<Collider> projectiles = new HashSet<Collider>();

			foreach (ControllerComponent proj in m_projectiles)
			{
				projectiles.Add(proj.GetComponent<Collider>());
			}

			foreach (var near in nearbyObjects)
			{
				if (projectiles.Contains(near))
				{
					nearbyNotProj.Remove(near);
				}
			}

			foreach (Collider collider in nearbyNotProj)
			{
				if(collider.GetComponentInParent<ControllerComponent>() == m_player)
				{
					Debug.Log("Hit Player");
				}
				
			}
			if(nearbyNotProj.Count > 0)
			{
				m_thisObject.Destroy();
				Debug.Log("destroyed projectile + " + m_thisObject.name);

				foreach (Collider collider in nearbyNotProj)
				{
					Debug.Log("this" + m_thisObject.GetInstanceID() + "collided with " + collider.name + " " + collider.GetInstanceID());
				}
			}

			m_rigidbody.velocity = m_thisObject.transform.forward * m_speed;

			return;
		}

		//You can add your own methods if you need. Like i.e. DoBigMathsThing() But only this state will be able to use it, because the StateMachine/CurrentState is hidden/encapsulted.
		//I'm thinking of a workaround for get/set.

	}
}
