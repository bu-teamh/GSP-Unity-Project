using System.Collections.Generic;
using UnityEngine;

using GSP.Events;
using GSP.Controller;

namespace GSP.States
{
	public class ProjectileMoveState : ProjectileBaseState
	{
		public ProjectileMoveState(ControllerComponent _object) : base(_object) { }

		public ProjectileMoveState(BaseState _state) : base(_state) { }

		protected override void Awake()
		{
			base.Awake();
		}

		protected override void InitializeMap()
		{
			SetTransition(typeof(ProjectileIdleState), EventArchetype.Internal, EventSubtype.Move, EventFlag.Inactive);
		}

		public override void Update()
		{
			base.Update();

			return;
		}

		public override void FixedUpdate()
		{
			base.FixedUpdate();

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
					if(m_player.GetState() == typeof(PlayerDefendState))
					{
						Debug.Log("Defended");
					}
					else
					{
						m_gameStateManager.AddToGlobalValue(GlobalValue.PlayerHealth, 20);
					}
					//Debug.Log("Hit Player");
				}
				
			}
			if(nearbyNotProj.Count > 0)
			{
				m_thisObject.Remove();

				foreach (Collider collider in nearbyNotProj)
				{
					//Debug.Log("this" + m_thisObject.GetInstanceID() + "collided with " + collider.name + " " + collider.GetInstanceID());
				}
			}

			m_rigidbody.velocity = m_thisObject.transform.forward * m_speed;

			return;
		}
	}
}
