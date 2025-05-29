using System.Collections.Generic;

using UnityEngine;

using GSP.Mediator;
using GSP.Controller;
using GSP.Events;
using UnityEngine.UIElements;

namespace GSP.States
{
	public class PortcullisOpenState : PortcullisBaseState
	{
		public PortcullisOpenState(ControllerComponent _object) : base(_object) { }

		public PortcullisOpenState(BaseState _state) : base(_state) { }

		protected override void InitializeMap()
		{
			SetTransition(typeof(PortcullisClosingState), EventArchetype.Gameplay, EventSubtype.Combat, EventFlag.Active);
		}
	}
}
