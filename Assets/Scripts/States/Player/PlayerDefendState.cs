using UnityEngine;

using GSP.Controller;
using GSP.Events;

namespace GSP.States
{
	public class PlayerDefendState : PlayerBaseState
	{
		public PlayerDefendState(ControllerComponent _object) : base(_object) { }

		public PlayerDefendState(BaseState _state) : base(_state) { }

		protected override void InitializeMap()
		{
			SetTransition(m_switchStateMap[SwitchState.Combat], EventArchetype.GameplayInput, EventSubtype.Defend, EventFlag.KeyUp);
		}

		protected override void Awake()
		{
			base.Awake();
			if(m_inputManager.DualAxisHeld(EventSubtype.Move))
			{
				m_switchStateMap[SwitchState.Combat] = typeof(PlayerMoveState);
			}
			else
			{
				m_switchStateMap[SwitchState.Combat] = typeof(PlayerIdleState);
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
			if (CompareEvent(_event, EventArchetype.GameplayInput, EventSubtype.Move, EventFlag.KeyDown))
			{
				m_switchStateMap[SwitchState.Combat] = typeof(PlayerMoveState);
			}
			if(CompareEvent(_event, EventArchetype.GameplayInput, EventSubtype.Move, EventFlag.KeyUp))
			{
				m_switchStateMap[SwitchState.Combat] = typeof(PlayerIdleState);
			}

		}

		public override void FixedUpdate()
		{
			base.FixedUpdate();

			m_defendSphereRenderer.enabled = true;

			return;
		}
	}
}
