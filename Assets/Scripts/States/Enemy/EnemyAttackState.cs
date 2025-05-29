using UnityEngine;

using GSP.Events;
using GSP.Controller;
using GSP.Timer;

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
			SetTransition(typeof(EnemyDamagedState), EventArchetype.Internal, EventSubtype.Damaged);
			SetTransition(typeof(EnemyBashState), EventArchetype.Internal, EventSubtype.Shoot);
		}
		protected override void Awake()
		{
			base.Awake();
			m_attackTimer = new GameTimer(m_attackDelay);
		}


		public override void Update()
		{
			base.Update();
			m_attackTimer.Start();
			m_attackTimer.Lock();

			if(m_attackTimer.Check())
			{
				Debug.Log("attacking now");
				m_animator.SetBool("IsAttacking", true);
				AttackType();
				m_attackTimer.Unlock();
			}

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


			return;
		}

		private void AttackType()
		{
			// Creates a Random 0 - 1 Value (Melee = 0, Ranged = 1)
			if(!m_thisObject.m_enemyRanged)
			{
				//Debug.Log("Melee Attack");
				InternalEvent(EventSubtype.Shoot);
			}
			else
			{
				Vector3 m_projectilePos = m_thisObject.transform.position;

				GameObject prefab = GameObject.Instantiate(m_projectilePrefab, m_projectilePos, m_thisObject.transform.rotation);
			}
		}
	}
}
