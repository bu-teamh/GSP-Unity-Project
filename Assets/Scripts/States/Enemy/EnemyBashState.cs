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
		}

		protected override void Awake()
		{
			base.Awake();
			m_Timer = new GameTimer(m_bashWaitTime);
			m_thisObject.m_agent.SetDestination(m_player.transform.position);
			m_thisObject.m_agent.speed = 0;
		}


		public override void Update()
		{
			base.Update();
			m_Timer.Start();
			m_Timer.Lock();

			if(m_Timer.Check())
			{ 
				m_isDashing = true;

				m_Timer.Unlock();
			}
			return;
		}

		public override void FixedUpdate()
		{
			base.FixedUpdate();

			if(m_isDashing)
			{
				m_thisObject.m_agent.speed = 10;
			}

			if (Physics.CheckSphere(m_thisObject.transform.position, 1.0f, m_thisObject.m_playerMask))
			{
				Debug.Log("i hit player ok");
				m_isDashing = false;
				//m_thisObject.m_agent.speed /= 2;
				InternalEvent(EventSubtype.PlayerOutRange);
			}


			return;
		}

	}
}
