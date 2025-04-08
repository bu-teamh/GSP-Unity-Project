using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Xml.Linq;

using UnityEngine;

//Include if this state listens out for input:
using GSP.InputHandling;

using GSP.Timer;
using GSP.Events;
using GSP.Mediator;
using GSP.Controller;
using Unity.VisualScripting;

namespace GSP.States
{
	//Rename Entity as your gameobject and Behaviour as your chosen state behaviour.
	public class TeleportListenState : TeleportBaseState
	{
		//Constructor doesn't need touching.
		public TeleportListenState(ControllerComponent _object) : base(_object) { }

		//Constructor doesn't need touching.
		public TeleportListenState(BaseState _state) : base(_state) { }

		//The map where should you go from this state.
		//I might change this dictionary another time to something else as it's very annoying to format
		protected override void InitializeMap()
		{
			
		}

		public override void Update()
		{
			//Does base state functionality. 
			base.Update();

			SendExternalEvent(
				this,
				EventPriority.Routine,
				EventArchetype.Gameplay,
				EventSubtype.Teleport,
				EventFlag.None,
				m_gameObject
			);

			m_gameObject.Disable();

			return;
		}

		public override void FixedUpdate()
		{
			//Does base state physics.
			base.FixedUpdate();

			//See comments in "Base" template for what should be done here (but in this case it only applies to this state).

			return;
		}

		//You can add your own methods if you need. Like i.e. DoBigMathsThing() But only this state will be able to use it, because the StateMachine/CurrentState is hidden/encapsulted.
		//I'm thinking of a workaround for get/set. 
	}
}
