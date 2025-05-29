using UnityEngine;

using GSP.Controller;
using GSP.Events;

namespace GSP.States
{
	public class CompanionCombatState : CompanionIdleState
	{
		public CompanionCombatState(ControllerComponent _object) : base(_object) { }

		public CompanionCombatState(BaseState _state) : base(_state) { }

		protected override void Awake()
		{
			base.Awake();
			m_maxDist = 4.0f;
			m_minDist = 2.0f;
			m_maxSpeed = 27.5f;
		}

		protected override void InitializeMap()
		{
			SetTransition(typeof(CompanionFollowState), EventArchetype.Gameplay, EventSubtype.Combat, EventFlag.Inactive);
			SetTransition(typeof(CompanionAimState), EventArchetype.Internal, EventSubtype.Aim, EventFlag.KeyDown);
			SetTransition(typeof(CompanionUltAimState), EventArchetype.Internal, EventSubtype.Ult, EventFlag.KeyDown);
		}

		public override void React(GameEvent _event)
		{
			base.React(_event);
			if(CompareEvent(_event, EventArchetype.GameplayInput, EventSubtype.Aim, EventFlag.KeyDown))
			{
				if (m_canAttack)
				{
					InternalEvent(EventSubtype.Aim, EventFlag.KeyDown);
				}
				else Debug.Log("Cant Attack Yet");
			}

			if(CompareEvent(_event, EventArchetype.GameplayInput, EventSubtype.Ult, EventFlag.KeyDown))
			{
				if (m_gameStateManager.GetGlobalValue(GlobalValue.PlayerCharge) >= m_gameStateManager.GetGlobalMaximum(GlobalValue.PlayerCharge))
				{
					InternalEvent(EventSubtype.Ult, EventFlag.KeyDown);
				}
				else Debug.Log("No charge for ult");
			}
		}

		public override void Update()
		{
			base.Update();

			return;
		}

		public override void FixedUpdate()
		{
			base.FixedUpdate();

			float distance = Vector3.Distance(m_thisObject.transform.position, m_player.transform.position);

			Vector3 direction = (m_player.transform.position - m_thisObject.transform.position).normalized;

			if (distance > m_maxDist)
			{
				m_velocity = Vector3.Project(m_velocity, direction);
				m_velocity += direction * m_accel * Time.fixedDeltaTime;
				m_velocity = Vector3.ClampMagnitude(m_velocity, m_maxSpeed);
			}
			else if (distance < m_minDist)
			{
				m_velocity -= direction * (m_accel * m_repelAccelMultplr) * Time.fixedDeltaTime;
				m_velocity = Vector3.ClampMagnitude(m_velocity, m_maxSpeed);
			}
			else
			{
				m_velocity = Vector3.Lerp(m_velocity, Vector3.zero, m_decel * Time.fixedDeltaTime);
			}

			return;
		}
	}
}
