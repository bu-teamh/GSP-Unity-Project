using System.Collections.Generic;

using UnityEngine;

using GSP.Events;
using GSP.Mediator;
using GSP.Controller;

namespace GSP.States
{
	public class WoodenDoorInitialState : WoodenDoorBaseState
	{
		public WoodenDoorInitialState(ControllerComponent _object) : base(_object) { }

		public WoodenDoorInitialState(BaseState _state) : base(_state) { }

		protected override void InitializeMap()
		{
			SetTransition(typeof(WoodenDoorLockedState), EventArchetype.Internal, EventSubtype.Initialised);
		}

		protected override void Awake()
		{
			m_thisObject.m_tooltip.Remove();
			m_openRot = Quaternion.Euler(0, 90.0f, 0);
			m_speed = 3.5f;
			m_unlocked = false;
			m_inRange = false;
			m_tooltipped = false;

			return;
		}

		public override void Update()
		{
			InternalEvent(EventSubtype.Initialised);

			return;
		}
	}
}
