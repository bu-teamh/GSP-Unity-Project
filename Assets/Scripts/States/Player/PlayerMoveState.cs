using UnityEngine;

using GSP.Controller;
using GSP.Events;

namespace GSP.States
{
	public class PlayerMoveState : PlayerBaseState
	{

		public PlayerMoveState(ControllerComponent _object) : base(_object) { }

		public PlayerMoveState(BaseState _state) : base(_state) { }

		protected override void InitializeMap()
		{
			SetTransition(typeof(PlayerIdleState), EventArchetype.GameplayInput, EventSubtype.Move, EventFlag.KeyUp);
			SetTransition(typeof(PlayerDashState), EventArchetype.GameplayInput, EventSubtype.Dodge, EventFlag.KeyDown);
		}

		public override void Update()
		{
			base.Update();

			return;
		}

		public override void FixedUpdate()
		{
			base.FixedUpdate();

			System.Numerics.Vector2 axisState = m_inputManager.GetDualAxisState(EventSubtype.Move);

			Vector3 rawDirection = new Vector3(-axisState.X, 0.0f, axisState.Y);

			Quaternion rotation = Quaternion.Euler(0.0f, -45.0f, 0.0f);

			Vector3 offsetDirection = rotation * rawDirection;

			m_velocity += offsetDirection * m_acceleration * Time.deltaTime;
			m_velocity = Vector3.ClampMagnitude(m_velocity, m_maxSpeed);

			//clamp on y plane
			m_velocity.y = 0.0f;

			m_targetRot = Quaternion.LookRotation(m_velocity);

			m_thisObject.m_characterController.Move(m_velocity * Time.deltaTime);

			return;
		}
	}
}
