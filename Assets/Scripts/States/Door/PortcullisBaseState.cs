using System.Collections.Generic;

using UnityEngine;

using GSP.Mediator;
using GSP.Controller;

namespace GSP.States
{
	public class PortcullisBaseState : DoorBaseState
	{
		protected Vector3 m_initPos;
		protected Vector3 m_openPos;

		public PortcullisBaseState(ControllerComponent _object) : base(_object) { }

		public PortcullisBaseState(BaseState _state) : base(_state) { }
	}
}
