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
	public class MainCameraTargetPassiveState : MainCameraTargetBaseState
	{
		public MainCameraTargetPassiveState(ControllerComponent _object) : base(_object) { }

		public MainCameraTargetPassiveState(BaseState _state) : base(_state) { }

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

			Vector3 playerPos = m_player.transform.position;
			Vector3 companionPos = m_companion.transform.position;

			m_gameObject.transform.position = new Vector3(
				(((m_playerWeight * playerPos.x) + (m_companionWeight * companionPos.x)) / (m_playerWeight + m_companionWeight)),
				(((m_playerWeight * playerPos.y) + (m_companionWeight * companionPos.y)) / (m_playerWeight + m_companionWeight)),
				(((m_playerWeight * playerPos.z) + (m_companionWeight * companionPos.z)) / (m_playerWeight + m_companionWeight))
				);

			return;
		}
	}
}
