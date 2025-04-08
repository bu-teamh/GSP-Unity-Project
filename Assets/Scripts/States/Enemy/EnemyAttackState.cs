using UnityEngine;

using GSP.Events;
using GSP.Controller;

namespace GSP.States
{
	public class EnemyAttackState : EnemyBaseState
	{
		public EnemyAttackState(ControllerComponent _object) : base(_object) { }

		public EnemyAttackState(BaseState _state) : base(_state) { }

		protected override void InitializeMap()
		{
			SetTransition(typeof(EnemyChaseState), EventArchetype.Internal, EventSubtype.PlayerOutRange);
			SetTransition(typeof(EnemyDieState), EventArchetype.Internal, EventSubtype.Death);
		}

		public override void Update()
		{
			base.Update();

			return;
		}

		public override void FixedUpdate()
		{
			base.FixedUpdate();

			if(!Physics.CheckSphere(m_thisObject.transform.position, m_attackRange, m_thisObject.m_playerMask))
			{
				InternalEvent(EventSubtype.PlayerOutRange);
			}

			m_thisObject.m_agent.SetDestination(m_thisObject.transform.position);
			m_thisObject.transform.LookAt(m_player.transform);

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
				Vector3 m_projectilePos = m_thisObject.transform.position;
				m_projectilePos.z += 2;

				GameObject prefab = GameObject.Instantiate(m_projectilePrefab, m_projectilePos, m_thisObject.transform.rotation);
			}
		}
	}
}
