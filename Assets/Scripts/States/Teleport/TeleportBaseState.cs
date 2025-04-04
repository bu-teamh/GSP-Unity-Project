using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Xml.Linq;

using UnityEngine;

//Include if this state listens out for input:
using GSP.InputHandling;

using GSP.Events;
using GSP.Mediator;
using GSP.Controller;
using GSP.Timer;

namespace GSP.States
{
	//Replace "Entity" with game object name in the class name
	public class TeleportBaseState : BaseState
	{
		//Constructor doesn't need touching
		public TeleportBaseState(ControllerComponent _object) : base(_object) { }

		//Second constructor doesn't need touching
		public TeleportBaseState(BaseState _state) : base(_state) { }

		//Here, assign the mediated objects like so
		protected override void GetMediations()
		{

		}

		protected override void InitializeTimers()
		{

		}

		public override void Update()
		{
			base.Update();

			return;
		}

		public override void FixedUpdate()
		{
			base.FixedUpdate();

			return;
		}
	}
}
