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
			SetTransition(typeof(EnemyChaseState), EventArchetype.Internal, EventSubtype.PlayerOutRange);
			SetTransition(typeof(EnemyDamagedState), EventArchetype.Internal, EventSubtype.Damaged);
		}

		protected override void Awake()
		{
			m_waitTimer = new GameTimer(m_bashWaitTime);
			m_bashTimer = new GameTimer(m_bashTime);
			m_targetDirection = m_player.transform.position - m_thisObject.transform.position;
			m_thisObject.m_agent.SetDestination(m_player.transform.position);	
			m_thisObject.m_agent.speed = 0;
		}

		public override void Update()
		{
			base.Update();
			m_waitTimer.Start();
			m_waitTimer.Lock();

			if(m_waitTimer.Check())
			{
				m_dashed = true;
				m_bashTimer.Start();
			}
			if(m_bashTimer.Check())
			{
				m_thisObject.m_agent.acceleration = 8;
				m_thisObject.m_agent.speed = 10;
				InternalEvent(EventSubtype.PlayerOutRange);
				m_waitTimer.Unlock();
			}
			return;
		}

		public override void FixedUpdate()
		{
			base.FixedUpdate();

			if(m_dashed)
			{ 
				m_thisObject.m_agent.speed = 20;
				m_thisObject.m_agent.acceleration = 15;
				if (Physics.CheckSphere(m_thisObject.transform.position, 1f, m_thisObject.m_playerMask))
				{
					Debug.Log("bashed player yeahhh!!!");
					m_gameStateManager.AddToGlobalValue(GlobalValue.PlayerHealth, -10);
				}
			}

			return;
		}

	}
}
