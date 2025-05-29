using System.Collections.Generic;

using UnityEngine;

using GSP.Mediator;
using GSP.Controller;
using GSP.Events;

namespace GSP.States
{
	public class MainCameraTargetBaseState : EntityBaseState
	{
		protected float m_threshold = 6.0f;
		protected float m_accel = 20.0f;
		protected float m_decel = 5.0f;
		protected float m_maxSpeed = 15.0f;

		protected float m_damping = 0.8f;

		protected float m_playerWeight = 2.0f;
		protected float m_companionWeight = 1.0f;
		protected float m_enemyWeight = 1.5f;

		protected float m_entityRadius = 20.0f;

		protected Vector3 m_velocity = Vector3.zero;
		protected Vector3 m_targetPosition = Vector3.zero;
		protected Vector3 m_previousTarget = Vector3.zero;
		protected float m_previousDistance = 0.0f;

		protected HashSet<ControllerComponent> m_localEnemies = new HashSet<ControllerComponent>();

		protected ControllerComponent m_player;
		protected ControllerComponent m_companion;
		protected HashSet<ControllerComponent> m_enemies;

		public MainCameraTargetBaseState(ControllerComponent _object) : base(_object) { }

		public MainCameraTargetBaseState(BaseState _state) : base(_state) { }

		protected override void GetMediations()
		{
			m_player = (ControllerComponent)m_thisObject.m_mediatedObjects[MediatedObject.Player];
			m_companion = (ControllerComponent)m_thisObject.m_mediatedObjects[MediatedObject.Companion];
			m_enemies = m_thisObject.m_mediatedGroups[MediatedGroup.Enemies];
		}

		public override void Update()
		{
			return;
		}

		public override void React(GameEvent _event)
		{
			if (CompareEvent(_event, EventArchetype.Gameplay, EventSubtype.Teleport))
			{
				ControllerComponent teleport = (ControllerComponent)_event.m_subject;

				m_targetPosition = teleport.transform.position;

				m_thisObject.transform.position = new Vector3(
					teleport.transform.position.x,
					teleport.transform.position.y,
					teleport.transform.position.z + 1
				);
			}
		}

		public override void FixedUpdate()
		{
			//smooth translate
			Vector3 direction = (m_targetPosition - m_thisObject.transform.position).normalized;
			float distance = Vector3.Distance(m_thisObject.transform.position, m_targetPosition);

			if (distance > m_threshold ||
				distance > m_previousDistance
			)
			{
				m_velocity = Vector3.Project(m_velocity, direction);
				m_velocity += direction * m_accel * Time.fixedDeltaTime;
				m_velocity = Vector3.ClampMagnitude(m_velocity, m_maxSpeed);
			}
			else
			{
				float factor = Mathf.Clamp01(distance / m_threshold);
				m_velocity = Vector3.Lerp(m_velocity, Vector3.zero, (1 - factor) * m_decel * Time.fixedDeltaTime);
			}

			m_previousDistance = distance;

			m_thisObject.transform.position += m_velocity * Time.fixedDeltaTime;

			return;
		}
	}
}
