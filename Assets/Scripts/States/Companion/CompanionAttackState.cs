using UnityEngine;

using GSP.Events;
using GSP.Controller;

namespace GSP.States
{
	public class CompanionAttackState : CompanionBaseState
	{
		public CompanionAttackState(ControllerComponent _object) : base(_object) { }

		public CompanionAttackState(BaseState _state) : base(_state) { }

		protected override void Awake()
		{
			m_switchStateMap[SwitchState.Combat] = typeof(CompanionCombatState);
		}

		protected override void InitializeMap()
		{
			SetTransition(m_switchStateMap[SwitchState.Combat], EventArchetype.GameplayInput, EventSubtype.Shoot, EventFlag.KeyUp);
		}

		public override void Update()
		{
			base.Update();

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
			//Collider [] EnemiesInRange = Physics.OverlapSphere(m_gameObject.transform.position, m_attackRange, m_gameObject.m_enemyMask);
			//foreach (Collider collider in EnemiesInRange)
			//{
			//	Debug.Log(collider.name);
			//}
			m_lineRenderer.enabled = false;
			m_thisObject.m_volume.weight = 0f;
			Time.timeScale = 1.0f;
			m_thisObject.m_characterController.Move(m_thisObject.transform.forward * m_accel * 2 * Time.fixedDeltaTime);

			return;
		}
	}
}
