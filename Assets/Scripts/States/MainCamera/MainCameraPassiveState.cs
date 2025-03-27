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
	public class MainCameraPassiveState : MainCameraBaseState
	{
		public MainCameraPassiveState(ControllerComponent _object) : base(_object) { }

		public MainCameraPassiveState(BaseState _state) : base(_state) { }

		protected override void InitializeMap()
		{
			/*
			//Event
			m_eventStateMap[EventArchetype.Input] = new Dictionary<EventSubtype, Dictionary<EventFlag, Type>>
			{
				//Subtypes
				{
					EventSubtype.Move, new Dictionary<EventFlag, Type>
					{
						{ EventFlag.KeyDown, typeof(PlayerMoveState) }
					}
				}
			};
			*/
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

			return;
		}

		public override void FixedUpdate()
		{
			base.FixedUpdate();

			float playerLanternDist = GetPercentDist();

			float rangeDist = m_farthestDist - m_closestDist;

			float targetCamDist = (rangeDist * playerLanternDist) + m_closestDist;

			m_currentCamDist = Mathf.Lerp(m_currentCamDist, targetCamDist, m_smoothDistSpeed * Time.deltaTime);

			Vector3 targetCoords = m_cameraTarget.transform.position;

			Vector3 targetPos = new Vector3(
				targetCoords.x + m_currentCamDist,
				targetCoords.y + m_currentCamDist * m_heightMultiplier,
				targetCoords.z - m_currentCamDist
				);

			m_gameObject.transform.position = Vector3.Lerp(m_gameObject.transform.position, targetPos, m_smoothPosSpeed * Time.deltaTime);

			Vector3 direction = m_cameraTarget.transform.position - m_gameObject.transform.position;

			direction.Normalize();

			Quaternion rotation = Quaternion.LookRotation(direction);

			m_gameObject.transform.rotation = rotation;

			return;
		}

		private float GetPercentDist()
		{
			// Calculate the distance between the player and the lantern
			// private cam_target m_cameraTarget;
			//m_cameraTarget = m_mediator.GetObject(MediatedObject.CameraTarget, this) as cam_target;

			Vector3 playerPos = m_player.transform.position;
			Vector3 companionPos = m_companion.transform.position;
			float subjectDistance = Vector3.Distance(playerPos, companionPos);

			// Clamp the distance within the minimum and maximum distance range
			if (subjectDistance < m_lantMinDist)
			{
				subjectDistance = m_lantMinDist;
			}
			else if (subjectDistance > m_lantMaxDist)
			{
				subjectDistance = m_lantMaxDist;
			}

			// Normalize the distance to a range between 0 and 1
			subjectDistance -= m_lantMinDist;
			subjectDistance /= (m_lantMaxDist - m_lantMinDist);

			return subjectDistance;
		}
	}
}
