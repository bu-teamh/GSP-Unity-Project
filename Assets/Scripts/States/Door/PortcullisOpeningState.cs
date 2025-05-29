using System.Collections.Generic;

using UnityEngine;

using GSP.Mediator;
using GSP.Controller;
using GSP.Events;
using UnityEngine.UIElements;

namespace GSP.States
{
	public class PortcullisOpeningState : PortcullisBaseState
	{
		public PortcullisOpeningState(ControllerComponent _object) : base(_object) { }

		public PortcullisOpeningState(BaseState _state) : base(_state) { }

		protected override void InitializeMap()
		{
			SetTransition(typeof(PortcullisClosingState), EventArchetype.Gameplay, EventSubtype.Combat, EventFlag.Active);
			SetTransition(typeof(PortcullisOpenState), EventArchetype.Internal, EventSubtype.Transition, EventFlag.Finished);
		}

		public override void FixedUpdate()
		{
			Vector3 targetPos = m_initPos + m_openPos;

			if (Vector3.Distance(m_thisObject.transform.position, targetPos) > 0.1f)
			{
				m_thisObject.transform.position = Vector3.Slerp(m_thisObject.transform.position, targetPos, m_speed * Time.fixedDeltaTime);
			}
			else
			{
				InternalEvent(EventSubtype.Transition, EventFlag.Finished);
			}

			return;
		}
	}
}
