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
			m_Timer = new GameTimer(m_bashTime);
		}


		public override void Update()
		{
			base.Update();
			m_Timer.Start();
			m_Timer.Lock();

			if(m_Timer.Check())
			{
				m_thisObject.m_agent.speed *= 2;
				m_thisObject.m_agent.SetDestination(m_player.transform.position);

			}
			return;
		}

		public override void FixedUpdate()
		{
			base.FixedUpdate();


			return;
		}

	}
}
