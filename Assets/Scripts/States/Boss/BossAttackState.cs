using UnityEngine;

using GSP.Events;
using GSP.Controller;
using GSP.Timer;

namespace GSP.States
{
	public class BossAttackState : BossBaseState
	{
		public BossAttackState(ControllerComponent _object) : base(_object) { }

		public BossAttackState(BaseState _state) : base(_state) { }

		protected override void InitializeMap()
		{
			SetTransition(typeof(BossChaseState), EventArchetype.Internal, EventSubtype.PlayerOutRange);
			SetTransition(typeof(BossDieState), EventArchetype.Internal, EventSubtype.Death);
		}
		protected override void Awake()
		{
			m_attackTimer = new GameTimer(m_attackDelay);
		}


		public override void Update()
		{
			m_attackTimer.Start();
			m_attackTimer.Lock();

			if (m_attackTimer.Check())
			{
				Debug.Log("attacking now");
				m_animator.SetBool("IsAttacking", true);
				m_isAttacking = true;
				m_attackTimer.Unlock();
			}

			return;
		}

		public override void FixedUpdate()
		{
			base.FixedUpdate();

			if (!Physics.CheckSphere(m_thisObject.transform.position, m_attackRange, m_thisObject.m_playerMask))
			{
				InternalEvent(EventSubtype.PlayerOutRange);
			}

			m_thisObject.m_agent.SetDestination(m_thisObject.transform.position);
			m_thisObject.transform.LookAt(m_player.transform);

			if (m_isAttacking)
			{
				if (Physics.CheckSphere(m_thisObject.transform.position, 3.0f, m_thisObject.m_playerMask))
				{
					if (m_player.GetState() == typeof(PlayerDefendState))
					{
						if (m_player.m_parry)
						{
							Debug.Log("Parried");
							m_gameStateManager.AddToGlobalValue(GlobalValue.PlayerCharge, 40);
						}
						else
						{
							Debug.Log("Defended");
							m_gameStateManager.AddToGlobalValue(GlobalValue.PlayerHealth, -5);
						}
					}
					else
					{
						Debug.Log("hit player");
						m_gameStateManager.AddToGlobalValue(GlobalValue.PlayerHealth, -10);
					}
				}
				m_isAttacking = false;
			}
			return;
		}
	}
}
