using UnityEngine;

using GSP.Events;
using GSP.Controller;

namespace GSP.States
{
	public class EnemyChaseState : EnemyBaseState
	{
		public EnemyChaseState(ControllerComponent _object) : base(_object) { }

		public EnemyChaseState(BaseState _state) : base(_state) { }

		protected override void InitializeMap()
		{
			SetTransition(typeof(EnemyAttackState), EventArchetype.Internal, EventSubtype.PlayerInRange);
			SetTransition(typeof(EnemyPatrolState), EventArchetype.Internal, EventSubtype.PlayerLost);
		}

		public override void Update()
		{
			base.Update();

			m_thisObject.m_agent.SetDestination(m_player.transform.position);

			if (Physics.CheckSphere(m_thisObject.transform.position, m_attackRange, m_thisObject.m_playerMask))
			{
				InternalEvent(EventSubtype.PlayerInRange);
			}

			if(!Physics.CheckSphere(m_thisObject.transform.position, m_sightRange, m_thisObject.m_playerMask))
			{
				InternalEvent(EventSubtype.PlayerLost);
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
