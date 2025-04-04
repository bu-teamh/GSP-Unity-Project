using System;
using System.Collections.Generic;
using System.Reflection;
using GSP.Events;
using GSP.InputHandling;
using UnityEngine;

using GSP.Mediator;
using GSP.Controller;
using UnityEngine.UIElements;
using Unity.VisualScripting;

namespace GSP.States
{
	public class CompanionFollowState : CompanionIdleState
	{
		public CompanionFollowState(ControllerComponent _object) : base(_object) { }

		public CompanionFollowState(BaseState _state) : base(_state) { }

		protected override void InitializeMap()
		{
			SetTransition(typeof(CompanionCombatState), EventArchetype.Gameplay, EventSubtype.Combat, EventFlag.Active);
		}

		protected override void Awake()
		{
			m_maxDist = 9.0f;
			m_minDist = 2.5f;
			m_maxSpeed = 15.0f;
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

			float distance = Vector3.Distance(m_transform.position, m_player.transform.position);

			Vector3 direction = (m_player.transform.position - m_transform.position).normalized;

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
