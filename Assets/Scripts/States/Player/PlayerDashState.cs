using System;
using System.Collections.Generic;
using System.Reflection;
using GSP.Events;
using GSP.InputHandling;
using UnityEngine;

using GSP.Mediator;
using GSP.Controller;

namespace GSP.States
{
	public class PlayerDashState : PlayerBaseState
	{
		public PlayerDashState(ControllerComponent _object) : base(_object) { }

		public PlayerDashState(BaseState _state) : base(_state) { }

		protected override void InitializeMap()
		{
			SetTransition(typeof(PlayerMoveState), EventArchetype.Input, EventSubtype.Move, EventFlag.KeyDown);
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

			m_velocity = Vector3.Lerp(m_velocity, Vector3.zero, m_deceleration * Time.deltaTime);

			m_characterController.Move(m_velocity * Time.deltaTime);

			return;
		}
	}
}
