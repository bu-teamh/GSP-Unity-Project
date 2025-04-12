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
			SetTransition(m_switchStateMap[SwitchState.Combat], EventArchetype.GameplayInput, EventSubtype.Dodge, EventFlag.KeyUp);
			SetTransition(typeof(PlayerDefendState), EventArchetype.GameplayInput, EventSubtype.Defend, EventFlag.KeyDown);
		}

		protected override void Awake()
		{
			base.Awake();

			if (m_inputManager.DualAxisHeld(EventSubtype.Move))
			{
				m_switchStateMap[SwitchState.Combat] = typeof(PlayerMoveState);
			}
			else
			{
				m_switchStateMap[SwitchState.Combat] = typeof(PlayerIdleState);
				m_velocity = m_thisObject.transform.forward;
			}
		}

		public override void Update()
		{
			base.Update();

			return;
		}

		public override void React(GameEvent _event)
		{
			base.React(_event);

			if (CompareEvent(_event, EventArchetype.Input, EventSubtype.Move, EventFlag.KeyDown))
			{
				m_switchStateMap[SwitchState.Combat] = typeof(PlayerMoveState);
			}
			else
			{
				m_switchStateMap[SwitchState.Combat] = typeof(PlayerIdleState);
			}
		}

		public override void FixedUpdate()
		{
			base.FixedUpdate();
			if (!m_hasDashed)
			{
				m_velocity += m_velocity * m_acceleration * Time.fixedDeltaTime;
				if (m_velocity.magnitude >= (m_maxSpeed * 2))
				{
					m_hasDashed = true;
				}
			}

			if (m_velocity.magnitude > m_maxSpeed && m_hasDashed)
			{
				m_velocity -= m_velocity * Time.fixedDeltaTime;
			}
			else
			{
				m_hasDashed = false;
			}
			m_thisObject.m_characterController.Move(m_velocity * Time.fixedDeltaTime);

			return;
		}
	}
}
