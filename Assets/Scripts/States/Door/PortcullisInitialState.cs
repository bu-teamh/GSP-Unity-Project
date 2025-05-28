using System.Collections.Generic;

using UnityEngine;

using GSP.Mediator;
using GSP.Controller;
using GSP.Events;

namespace GSP.States
{
	public class PortcullisInitialState : PortcullisBaseState
	{
		public PortcullisInitialState(ControllerComponent _object) : base(_object) { }

		public PortcullisInitialState(BaseState _state) : base(_state) { }

		protected override void InitializeMap()
		{
			SetTransition(typeof(PortcullisLockedState), EventArchetype.Internal, EventSubtype.Initialised);
		}

		protected override void Awake()
		{
			m_thisObject.m_tooltip.Remove();
			m_openPos = new Vector3(0, 3.0f, 0);
			m_initPos = m_thisObject.transform.position;
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
