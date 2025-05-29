using System.Collections.Generic;

using UnityEngine;

using GSP.Mediator;
using GSP.Controller;
using GSP.Events;
using UnityEngine.UIElements;

namespace GSP.States
{
	public class WoodenDoorClosingState : WoodenDoorBaseState
	{
		public WoodenDoorClosingState(ControllerComponent _object) : base(_object) { }

		public WoodenDoorClosingState(BaseState _state) : base(_state) { }

		protected override void InitializeMap()
		{
			SetTransition(typeof(WoodenDoorOpeningState), EventArchetype.Gameplay, EventSubtype.Combat, EventFlag.Inactive);
			SetTransition(typeof(WoodenDoorClosedState), EventArchetype.Internal, EventSubtype.Transition, EventFlag.Finished);
		}

		public override void FixedUpdate()
		{
			if (Quaternion.Angle(m_thisObject.transform.rotation, Quaternion.Euler(0, 0, 0)) > 0.1f)
			{
				m_thisObject.transform.rotation = Quaternion.Slerp(m_thisObject.transform.rotation, Quaternion.Euler(0, 0, 0), m_speed * Time.fixedDeltaTime);
			}
			else
			{
				InternalEvent(EventSubtype.Transition, EventFlag.Finished);
			}

			return;
		}
	}
}
