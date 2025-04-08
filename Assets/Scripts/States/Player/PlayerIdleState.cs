using UnityEngine;

using GSP.Controller;
using GSP.Events;

namespace GSP.States
{
	public class PlayerIdleState : PlayerBaseState
	{
		public PlayerIdleState(ControllerComponent _object) : base(_object) { }

		public PlayerIdleState(BaseState _state) : base(_state) { }

		protected override void InitializeMap()
		{
			SetTransition(typeof(PlayerMoveState), EventArchetype.Input, EventSubtype.Move, EventFlag.KeyDown);
			SetTransition(typeof(PlayerDashState), EventArchetype.Input, EventSubtype.Dodge, EventFlag.KeyDown);
		}

		public override void Update()
		{
			base.Update();

			return;
		}

		public override void FixedUpdate()
		{
			base.FixedUpdate();

			m_velocity = Vector3.Lerp(m_velocity, Vector3.zero, m_deceleration * Time.deltaTime);

			m_thisObject.m_characterController.Move(m_velocity * Time.deltaTime);

			return;
		}
	}
}
