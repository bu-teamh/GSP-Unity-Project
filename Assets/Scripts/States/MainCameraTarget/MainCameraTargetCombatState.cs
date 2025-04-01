using System;
using System.Collections.Generic;
using System.Reflection;
using GSP.Events;
using GSP.InputHandling;
using UnityEngine;

using GSP.Mediator;
using GSP.Controller;
using UnityEngine.UIElements;

namespace GSP.States
{
	public class MainCameraTargetCombatState : MainCameraTargetBaseState
	{
		public MainCameraTargetCombatState(ControllerComponent _object) : base(_object) { }

		public MainCameraTargetCombatState(BaseState _state) : base(_state) { }

		protected override void InitializeMap()
		{
			SetTransition(typeof(MainCameraTargetFollowState), EventArchetype.Internal, EventSubtype.CombatCam, EventFlag.Inactive);
		}

		public override void Update()
		{
			// does base class update method
			base.Update();

			//if block, if event = w, do x, else do y, nextstate = z
			// this state inherits from base state and theefore this should have functionality that should be only done during specific state
			// on top of general logic

			//you should not instruct the gameobject to go to a specific state from here:
			//if it is called for, you need to send an event like so:
			// GameEvent ev = new GameEvent(params);
			// m_gameObject.m_handler.Enqueue(ev)
			// and then add that event type to state map to react to that event in this state

			if (m_localEnemies.Count == 0)
			{
				GameEvent ev = new GameEvent(EventArchetype.Internal, EventSubtype.CombatCam, EventPriority.Urgent, EventFlag.Inactive, this);

				m_gameObject.m_handler.Enqueue(ev);
			}

			return;
		}

		public override void FixedUpdate()
		{
			Vector3 newTarget = Vector3.zero;

			newTarget += m_player.transform.position * m_playerWeight;

			foreach (ControllerComponent enemy in m_localEnemies)
			{
				newTarget += enemy.transform.position * m_enemyWeight;
			}

			newTarget /= (
				(
					m_localEnemies.Count *
					m_enemyWeight
				) +
				m_playerWeight
			);

			m_targetPosition = Vector3.Lerp(m_previousTarget, newTarget, Time.deltaTime * m_damping);

			m_previousTarget = m_targetPosition;

			base.FixedUpdate();

			return;
		}
	}
}
