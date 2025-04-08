using UnityEngine;

using GSP.Controller;
using GSP.Events;

namespace GSP.States
{
	public class PlayerDashState : PlayerBaseState
	{
		public PlayerDashState(ControllerComponent _object) : base(_object) { }

		public PlayerDashState(BaseState _state) : base(_state) { }

		protected override void InitializeMap()
		{
			SetTransition(typeof(PlayerMoveState), EventArchetype.Input, EventSubtype.Dodge, EventFlag.KeyUp);
			SetTransition(typeof(PlayerIdleState), EventArchetype.Input, EventSubtype.Move, EventFlag.KeyUp);
		}

		public override void Update()
		{
			base.Update();

			return;
		}

		public override void FixedUpdate()
		{
			base.FixedUpdate();
			m_thisObject.m_characterController.Move(m_velocity * 3.0f * Time.deltaTime);

			return;
		}
	}
}
