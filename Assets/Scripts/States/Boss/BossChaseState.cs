using UnityEngine;

using GSP.Events;
using GSP.Controller;

namespace GSP.States
{
	public class BossChaseState : BossBaseState
	{
		public BossChaseState(ControllerComponent _object) : base(_object) { }

		public BossChaseState(BaseState _state) : base(_state) { }

		protected override void InitializeMap()
		{
			SetTransition(typeof(BossAttackState), EventArchetype.Internal, EventSubtype.PlayerInRange);
			SetTransition(typeof(BossDieState), EventArchetype.Internal, EventSubtype.Death);
		}

		public override void Update()
		{
			base.Update();

			m_thisObject.m_agent.SetDestination(m_player.transform.position);

			if (Physics.CheckSphere(m_thisObject.transform.position, m_attackRange, m_thisObject.m_playerMask))
			{
				InternalEvent(EventSubtype.PlayerInRange);
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
