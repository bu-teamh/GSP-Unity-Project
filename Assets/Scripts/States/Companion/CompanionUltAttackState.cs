using UnityEngine;

using GSP.Events;
using GSP.Controller;
using Unity.VisualScripting.FullSerializer;
using GSP.Timer;

namespace GSP.States
{
	public class CompanionUltAttackState : CompanionBaseState
	{
		public CompanionUltAttackState(ControllerComponent _object) : base(_object) { }

		public CompanionUltAttackState(BaseState _state) : base(_state) { }

		protected override void Awake()
		{
			base.Awake();
			m_switchStateMap[SwitchState.Combat] = typeof(CompanionCombatState);
			m_timer = new GameTimer(m_attackTime / 2);
			//m_thisObject.m_AOE.SetActive(false);
			m_AOEeffect.Play();
			m_gameStateManager.AddToGlobalValue(GlobalValue.PlayerCharge, (int)-(m_gameStateManager.GetGlobalMaximum(GlobalValue.PlayerCharge)));
			m_soundFX.PlaySound3();
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

			if (m_timer.Check())
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
			Debug.Log("im ult attacking now waaaaahhhh");
			//m_thisObject.m_volume.weight = 0f;
			Time.timeScale = 1.0f;

			return;
		}
	}
}
