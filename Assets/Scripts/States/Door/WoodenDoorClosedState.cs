using System.Collections.Generic;

using UnityEngine;

using GSP.Mediator;
using GSP.Controller;
using GSP.Events;
using UnityEngine.UIElements;

namespace GSP.States
{
	public class WoodenDoorClosedState : WoodenDoorBaseState
	{
		public WoodenDoorClosedState(ControllerComponent _object) : base(_object) { }

		public WoodenDoorClosedState(BaseState _state) : base(_state) { }

		protected override void InitializeMap()
		{
			SetTransition(typeof(WoodenDoorOpeningState), EventArchetype.Gameplay, EventSubtype.Combat, EventFlag.Inactive);
		}
	}
}
