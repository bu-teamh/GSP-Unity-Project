using System.Collections.Generic;

using UnityEngine;

using GSP.Mediator;
using GSP.Controller;

namespace GSP.States
{
	public class WoodenDoorBaseState : DoorBaseState
	{
		protected Quaternion m_openRot;

		public WoodenDoorBaseState(ControllerComponent _object) : base(_object) { }

		public WoodenDoorBaseState(BaseState _state) : base(_state) { }
	}
}
