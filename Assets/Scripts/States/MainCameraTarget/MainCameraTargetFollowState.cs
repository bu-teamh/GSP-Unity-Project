using UnityEngine;

using GSP.Events;
using GSP.Controller;

namespace GSP.States
{
	public class MainCameraTargetFollowState : MainCameraTargetBaseState
	{
		public MainCameraTargetFollowState(ControllerComponent _object) : base(_object) { }

		public MainCameraTargetFollowState(BaseState _state) : base(_state) { }

		protected override void InitializeMap()
		{
			//SetTransition(typeof(MainCameraTargetCombatState), EventArchetype.Gameplay, EventSubtype.Combat, EventFlag.Active);
			SetTransition(typeof(MainCameraTargetMenuState), EventArchetype.UI, EventSubtype.Menu, EventFlag.Active);
		}

		public override void Update()
		{
			base.Update();

			return;
		}

		public override void FixedUpdate()
		{
			Vector3 newTarget = Vector3.zero;

			newTarget += m_player.transform.position * m_playerWeight;
			newTarget += m_companion.transform.position * m_companionWeight;

			newTarget /= (m_playerWeight + m_companionWeight);

			m_targetPosition = newTarget;

			base.FixedUpdate();

			return;
		}
	}
}
