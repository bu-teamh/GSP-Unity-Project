using UnityEngine;

using GSP.Controller;
using GSP.Events;

namespace GSP.States
{
	public class CompanionFollowState : CompanionIdleState
	{
		public CompanionFollowState(ControllerComponent _object) : base(_object) { }

		public CompanionFollowState(BaseState _state) : base(_state) { }

		protected override void Awake()
		{
			base.Awake();
			m_maxDist = 9.0f;
			m_minDist = 2.5f;
			m_maxSpeed = 15.0f;
		}

		protected override void InitializeMap()
		{
			SetTransition(typeof(CompanionCombatState), EventArchetype.Gameplay, EventSubtype.Combat, EventFlag.Active);
			SetTransition(typeof(CompanionPhoebusState), EventArchetype.Input, EventSubtype.Aim);
		}

		public override void React(GameEvent _event)
		{
			base.React(_event);
		}

		public override void Update()
		{
			base.Update();

			Debug.Log("companion following");

			return;
		}

		public override void FixedUpdate()
		{
			base.FixedUpdate();

			float distance = Vector3.Distance(m_thisObject.transform.position, m_player.transform.position);

			Vector3 direction = (m_player.transform.position - m_thisObject.transform.position).normalized;

			if (distance > m_maxDist)
			{
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
