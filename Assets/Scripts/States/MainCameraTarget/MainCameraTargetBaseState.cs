using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Xml.Linq;
using GSP.Events;
using GSP.InputHandling;
using UnityEngine;

using GSP.Mediator;
using GSP.Controller;
using System.Linq;

namespace GSP.States
{
	public class MainCameraTargetBaseState : BaseState
	{
		// --- --- --- ---
		// attributes for component state (health, etc) are defined here

		// physics attributes (pos, rot, speed etc) for fixed update

		protected float m_threshold = 6.0f;
		protected float m_accel = 20.0f;
		protected float m_decel = 5.0f;
		protected float m_maxSpeed = 15.0f;

		protected float m_damping = 0.8f;

		protected float m_playerWeight = 2.0f;
		protected float m_companionWeight = 1.0f;
		protected float m_enemyWeight = 1.5f;

		protected float m_entityRadius = 20.0f;

		//stored stuff

		protected Vector3 m_velocity = Vector3.zero;
		protected Vector3 m_targetPosition = Vector3.zero;
		protected Vector3 m_previousTarget = Vector3.zero;
		protected float m_previousDistance = 0.0f;

		protected HashSet<ControllerComponent> m_localEnemies = new HashSet<ControllerComponent>();

		//define attributes for mediated objects need to know about here

		protected ControllerComponent m_player;
		protected ControllerComponent m_companion;
		protected HashSet<ControllerComponent> m_enemies;

		// --- --- --- ---

		public MainCameraTargetBaseState(ControllerComponent _object) : base(_object) { }

		public MainCameraTargetBaseState(BaseState _state) : base(_state) { }

		protected override void GetMediations()
		{
			m_player = (ControllerComponent)m_gameObject.m_mediatedObjects[MediatedObject.Player];
			m_companion = (ControllerComponent)m_gameObject.m_mediatedObjects[MediatedObject.Companion];
			m_enemies = m_gameObject.m_mediatedGroups[MediatedGroup.Enemies];
		}

		public override void Update()
		{
			// this has functionality that should be done during ALL states
			//if block, if event = w, do x, else do y

			//this base class should never directly interrupt and change a state after doing logic, only manipulate attributes, otherwise there could be a conflict
			//if need to trigger state based on this logic
			//you should not instruct the gameobject to go to a specific state from here:
			//if it is called for, you need to send an event like so:
			// GameEvent ev = new GameEvent(params);
			// m_gameObject.m_handler.Enqueue(ev)
			// and then add that event type to state map to react to that event in this state

			//no physics to be done here!!

			return;
		}

		public override void FixedUpdate()
		{
			// regional check for enemies nearby, always updated
			Collider[] localObjects = Physics.OverlapSphere(m_player.transform.position, m_entityRadius);

			m_localEnemies.Clear();

			foreach (var collider in localObjects)
			{
				var controller = collider.GetComponentInParent<ControllerComponent>(); // << the enemey character controller does counts as a collider

				if (controller != null && m_enemies.Contains(controller))
				{
					m_localEnemies.Add(controller); // only add valid controllers that are in m_enemies
				}
			}

			//smooth translate
			Vector3 direction = (m_targetPosition - m_transform.position).normalized;
			float distance = Vector3.Distance(m_transform.position, m_targetPosition);

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

			m_transform.position += m_velocity * Time.fixedDeltaTime;

			return;
		}
	}
}
