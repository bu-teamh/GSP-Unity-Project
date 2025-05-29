using UnityEngine;

using GSP.Events;
using GSP.Controller;

namespace GSP.States
{
	public class BossIdleState : BossBaseState
	{
		public BossIdleState(ControllerComponent _object) : base(_object) { }

		public BossIdleState(BaseState _state) : base(_state) { }

		protected override void InitializeMap()
		{
			SetTransition(typeof(BossChaseState), EventArchetype.Internal, EventSubtype.PlayerSpotted);
			SetTransition(typeof(BossDieState), EventArchetype.Internal, EventSubtype.Death);
		}

		public override void Update()
		{
			base.Update();

			if (Physics.CheckSphere(m_thisObject.transform.position, m_sightRange, m_thisObject.m_playerMask))
			{
				InternalEvent(EventSubtype.PlayerSpotted);
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
