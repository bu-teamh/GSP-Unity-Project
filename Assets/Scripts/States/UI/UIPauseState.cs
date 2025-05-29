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
using GSP.Interface;
using GSP.Controller;
using Unity.VisualScripting;

namespace GSP.States
{
	//Rename Entity as your gameobject and Behaviour as your chosen state behaviour.
	public class UIPauseState : UIBaseState
	{
		//Constructor doesn't need touching.
		public UIPauseState(UIManager _object) : base(_object) { }

		//Constructor doesn't need touching.
		public UIPauseState(BaseState _state) : base(_state) { }

		//The map where should you go from this state.
		//I might change this dictionary another time to something else as it's very annoying to format
		protected override void InitializeMap()
		{
			SetTransition(typeof(UIClearState), EventArchetype.UI, EventSubtype.Pause, EventFlag.Inactive);
		}

		protected override void Awake()
		{
			m_thisObject.m_component.m_menuLayer.SetActive(true);
		}

		public override void Update()
		{
			//Does base state functionality. 
			base.Update();

			return;
		}

		public override void FixedUpdate()
		{
			//Does base state physics.
			base.FixedUpdate();

			//See comments in "Base" template for what should be done here (but in this case it only applies to this state).

			return;
		}
	}
}
