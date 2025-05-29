using System.Collections.Generic;

using UnityEngine;

using GSP.Mediator;
using GSP.Controller;
using GSP.Events;
using UnityEngine.UIElements;

namespace GSP.States
{
	public class WoodenDoorOpeningState : WoodenDoorBaseState
	{
		public WoodenDoorOpeningState(ControllerComponent _object) : base(_object) { }

		public WoodenDoorOpeningState(BaseState _state) : base(_state) { }

		protected override void InitializeMap()
		{
			SetTransition(typeof(WoodenDoorClosingState), EventArchetype.Gameplay, EventSubtype.Combat, EventFlag.Active);
			SetTransition(typeof(WoodenDoorOpenState), EventArchetype.Internal, EventSubtype.Transition, EventFlag.Finished);
		}

		public override void FixedUpdate()
		{
			if (Quaternion.Angle(m_thisObject.transform.rotation, m_openRot) > 0.1f)
			{
				m_thisObject.transform.rotation = Quaternion.Slerp(m_thisObject.transform.rotation, m_openRot, m_speed * Time.fixedDeltaTime);
			}
			else
			{
				InternalEvent(EventSubtype.Transition, EventFlag.Finished);
			}

			return;
		}
	}
}
