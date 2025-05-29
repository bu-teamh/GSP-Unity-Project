using System.Collections.Generic;

using UnityEngine;

using GSP.Mediator;
using GSP.Controller;
using GSP.Events;
using UnityEngine.UIElements;

namespace GSP.States
{
	public class WoodenDoorOpenState : WoodenDoorBaseState
	{
		public WoodenDoorOpenState(ControllerComponent _object) : base(_object) { }

		public WoodenDoorOpenState(BaseState _state) : base(_state) { }

		protected override void InitializeMap()
		{
			SetTransition(typeof(WoodenDoorClosingState), EventArchetype.Gameplay, EventSubtype.Combat, EventFlag.Active);
		}
	}
}
