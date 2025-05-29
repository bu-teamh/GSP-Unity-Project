using UnityEngine;

using GSP.Events;
using GSP.Controller;

namespace GSP.States
{
	public class MainCameraTargetMenuState : MainCameraTargetBaseState
	{
		public MainCameraTargetMenuState(ControllerComponent _object) : base(_object) { }

		public MainCameraTargetMenuState(BaseState _state) : base(_state) { }

		protected override void InitializeMap()
		{
			SetTransition(typeof(MainCameraTargetFollowState), EventArchetype.UI, EventSubtype.Menu, EventFlag.Inactive);
		}

		public override void Update()
		{
			base.Update();

			return;
		}

		public override void FixedUpdate()
		{
			m_targetPosition = m_player.transform.position;

			base.FixedUpdate();

			return;
		}
	}
}
