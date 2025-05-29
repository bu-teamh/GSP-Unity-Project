using System.Collections.Generic;

using UnityEngine;

using GSP.Mediator;
using GSP.Controller;
using GSP.Events;
using UnityEngine.UIElements;

namespace GSP.States
{
	public class PortcullisClosedState : PortcullisBaseState
	{
		public PortcullisClosedState(ControllerComponent _object) : base(_object) { }

		public PortcullisClosedState(BaseState _state) : base(_state) { }

		protected override void InitializeMap()
		{
			SetTransition(typeof(PortcullisOpeningState), EventArchetype.Gameplay, EventSubtype.Combat, EventFlag.Inactive);
		}
	}
}
