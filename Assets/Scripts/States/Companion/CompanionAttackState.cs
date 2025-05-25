using UnityEngine;

using GSP.Events;
using GSP.Controller;
using Unity.VisualScripting.FullSerializer;
using GSP.Timer;

namespace GSP.States
{
	public class CompanionAttackState : CompanionBaseState
	{
		public CompanionAttackState(ControllerComponent _object) : base(_object) { }

		public CompanionAttackState(BaseState _state) : base(_state) { }

		protected override void Awake()
		{
			base.Awake();
			m_switchStateMap[SwitchState.Combat] = typeof(CompanionCombatState);
			m_timer = new GameTimer(m_attackTime);
			m_lineRenderer.enabled = false;
		}

		protected override void InitializeMap()
		{
			SetTransition(m_switchStateMap[SwitchState.Combat], EventArchetype.Internal, EventSubtype.Shoot, EventFlag.KeyUp);
		}

		public override void Update()
		{
			base.Update();
			m_timer.Start();
			m_timer.Lock();

			if(m_timer.Check())
			{
				InternalEvent(EventSubtype.Shoot, EventFlag.KeyUp);
				m_canAttack = false;
				m_timer.Unlock();
			}

			return;
		}

		public override void React(GameEvent _ev)
		{
			if (CompareEvent(_ev, EventArchetype.Gameplay, EventSubtype.Combat, EventFlag.Inactive))
			{
				m_switchStateMap[SwitchState.Combat] = typeof(CompanionFollowState);
			}
			else if (CompareEvent(_ev, EventArchetype.Gameplay, EventSubtype.Combat, EventFlag.Active))
			{
				m_switchStateMap[SwitchState.Combat] = typeof(CompanionCombatState);
			}
		}

		public override void FixedUpdate()
		{
			//Does base state physics.
			base.FixedUpdate();

			//See comments in "Base" template for what should be done here (but in this case it only applies to this state).

			//m_thisObject.m_volume.weight = 0f;
			Time.timeScale = 1.0f;
			m_thisObject.m_characterController.Move(m_thisObject.transform.forward * m_accel * 2 * Time.fixedDeltaTime);

			return;
		}
	}
}
