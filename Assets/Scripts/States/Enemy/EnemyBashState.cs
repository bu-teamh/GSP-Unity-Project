using UnityEngine;

using GSP.Events;
using GSP.Controller;
using GSP.Timer;

namespace GSP.States
{
	public class EnemyBashState : EnemyBaseState
	{
		public EnemyBashState(ControllerComponent _object) : base(_object) { }

		public EnemyBashState(BaseState _state) : base(_state) { }

		protected override void InitializeMap()
		{
			SetTransition(m_switchStateMap[SwitchState.Combat], EventArchetype.Internal, EventSubtype.Combat);
			SetTransition(typeof(EnemyDamagedState), EventArchetype.Internal, EventSubtype.Damaged);
		}

		protected override void Awake()
		{
			base.Awake();
			m_switchStateMap[SwitchState.Combat] = typeof(EnemyChaseState);

			m_waitTimer = new GameTimer(m_bashWaitTime);
			m_bashTimer = new GameTimer(m_bashTime);
			m_targetDirection = m_player.transform.position - m_thisObject.transform.position;
			m_thisObject.m_agent.ResetPath();
			m_thisObject.m_agent.isStopped = true;
			m_hitPlayer = false;
		}

		public override void Update()
		{
			base.Update();
			m_waitTimer.Start();
			m_waitTimer.Lock();

			if(m_waitTimer.Check())
			{
				m_isDashing = true;
				m_bashTimer.Start();
			}
			if(m_bashTimer.Check())
			{
				if(Physics.CheckSphere(m_thisObject.transform.position, m_sightRange, m_thisObject.m_playerMask))
				{
					m_switchStateMap[SwitchState.Combat] = typeof(EnemyChaseState);
				}
				m_thisObject.m_agent.isStopped = false;
				m_hitPlayer = false;
				InternalEvent(EventSubtype.Combat);
				m_isDashing = false;
				m_waitTimer.Unlock();
			}
			return;
		}

		public override void FixedUpdate()
		{
			base.FixedUpdate();

			if(m_isDashing)
			{
				m_thisObject.m_agent.Move(m_targetDirection.normalized * 15 * Time.fixedDeltaTime);

				if(!m_hitPlayer)
				{
					if (Physics.CheckSphere(m_thisObject.transform.position, 1f, m_thisObject.m_playerMask))
					{
						Debug.Log("bashed player yeahhh!!!");
						if(m_player.GetState() == typeof(PlayerDefendState))
						{
							Debug.Log("he defended that whaaaaat???");
							if(m_player.m_parry)
							{
								Debug.Log("Parried!!!!");
								m_gameStateManager.AddToGlobalValue(GlobalValue.PlayerCharge, 40);
							}
							else { m_gameStateManager.AddToGlobalValue(GlobalValue.PlayerHealth, -5); }
						}
						else
						{
							m_gameStateManager.AddToGlobalValue(GlobalValue.PlayerHealth, -10);
						}
						m_hitPlayer = true;
					}

				}
			}

			return;
		}

	}
}
