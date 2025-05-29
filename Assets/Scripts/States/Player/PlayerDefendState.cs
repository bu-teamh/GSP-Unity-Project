using UnityEngine;

using GSP.Controller;
using GSP.Events;
using GSP.Timer;

namespace GSP.States
{
	public class PlayerDefendState : PlayerBaseState
	{
		public PlayerDefendState(ControllerComponent _object) : base(_object) { }

		public PlayerDefendState(BaseState _state) : base(_state) { }

		protected override void InitializeMap()
		{
			SetTransition(m_switchStateMap[SwitchState.Combat], EventArchetype.GameplayInput, EventSubtype.Defend, EventFlag.KeyUp);
			SetTransition(typeof(PlayerDeathState), EventArchetype.Internal, EventSubtype.Death);
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

			m_parryWindow = new GameTimer(m_parryTime);
			m_thisObject.m_parry = true;
			m_canDefend = false;

		}

		public override void Update()
		{
			base.Update();
			m_parryWindow.Start();
			m_parryWindow.Lock();

			if(m_parryWindow.Check())
			{
				m_thisObject.m_parry = false;
				m_parryWindow.Unlock();
			}

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
