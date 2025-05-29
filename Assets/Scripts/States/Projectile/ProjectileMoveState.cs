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

			HashSet<Collider> canCollideWith = new HashSet<Collider>(nearbyObjects);
			HashSet<Collider> projectiles = new HashSet<Collider>();
			HashSet<Collider> enemies = new HashSet<Collider>();

			foreach (ControllerComponent proj in m_projectiles)
			{
				projectiles.Add(proj.GetComponent<Collider>());		// Makes List of all Projectiles
			}

			foreach(ControllerComponent enemy in m_enemies)
			{
				enemies.Add(enemy.GetComponent<Collider>());
			}

			foreach (var near in nearbyObjects)		// Removes projectiles from collision list
			{
				if (projectiles.Contains(near) || enemies.Contains(near))
				{
					canCollideWith.Remove(near);
				}
			}

			foreach (Collider collider in canCollideWith)
			{
				if (collider.GetComponentInParent<ControllerComponent>() == m_player)
				{
					if (m_player.GetState() == typeof(PlayerDefendState))
					{
						Debug.Log("projectile Defended");

						if (m_player.m_parry)
						{
							Debug.Log("projectile Parried");
							m_gameStateManager.AddToGlobalValue(GlobalValue.PlayerCharge, 40);
						}
						else { m_gameStateManager.AddToGlobalValue(GlobalValue.PlayerHealth, -5); }
					}
					else
					{
						m_gameStateManager.AddToGlobalValue(GlobalValue.PlayerHealth, -10);
					}

					m_thisObject.Remove();
				}
				//else m_thisObject.Remove();
				
			}
			return;
		}
	}
}
