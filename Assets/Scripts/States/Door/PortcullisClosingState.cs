using System.Collections.Generic;

using UnityEngine;

using GSP.Mediator;
using GSP.Controller;
using GSP.Events;
using UnityEngine.UIElements;

namespace GSP.States
{
	public class PortcullisClosingState : PortcullisBaseState
	{
		public PortcullisClosingState(ControllerComponent _object) : base(_object) { }

		public PortcullisClosingState(BaseState _state) : base(_state) { }

		protected override void InitializeMap()
		{
			SetTransition(typeof(PortcullisOpeningState), EventArchetype.Gameplay, EventSubtype.Combat, EventFlag.Inactive);
			SetTransition(typeof(PortcullisClosedState), EventArchetype.Internal, EventSubtype.Transition, EventFlag.Finished);
		}

		public override void FixedUpdate()
		{
			if (Vector3.Distance(m_thisObject.transform.position, m_initPos) > 0.1f)
			{
				m_thisObject.transform.position = Vector3.Slerp(m_thisObject.transform.position, m_initPos, m_speed * Time.fixedDeltaTime);
			}
			else
			{
				InternalEvent(EventSubtype.Transition, EventFlag.Finished);
			}

			return;
		}
	}
}
