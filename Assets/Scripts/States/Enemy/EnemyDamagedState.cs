using UnityEngine;

using GSP.Events;
using GSP.Controller;

namespace GSP.States
{
	public class EnemyDamagedState : EnemyBaseState
	{
		public EnemyDamagedState(ControllerComponent _object) : base(_object) { }

		public EnemyDamagedState(BaseState _state) : base(_state) { }

		protected override void InitializeMap()
		{
			SetTransition(typeof(EnemyChaseState), EventArchetype.Internal, EventSubtype.PlayerSpotted);
			SetTransition(typeof(EnemyPatrolState), EventArchetype.Internal, EventSubtype.PlayerLost);
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

			Vector3 direction = m_thisObject.transform.position - m_companion.transform.position;

			m_thisObject.transform.position = direction * Time.fixedDeltaTime;

			return;
		}

	}
}
