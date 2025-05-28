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
	public class UIInventoryState : UIBaseState
	{
		//Constructor doesn't need touching.
		public UIInventoryState(UIManager _object) : base(_object) { }

		//Constructor doesn't need touching.
		public UIInventoryState(BaseState _state) : base(_state) { }

		//The map where should you go from this state.
		//I might change this dictionary another time to something else as it's very annoying to format
		protected override void InitializeMap()
		{
			SetTransition(typeof(UIClearState), EventArchetype.UI, EventSubtype.Menu, EventFlag.Inactive);
			SetTransition(typeof(UIInventoryHealthState), EventArchetype.Internal, EventSubtype.Menu, EventFlag.Health);
		}

		protected override void Awake()
		{
			m_thisObject.m_component.m_menuLayer.SetActive(true);
			m_ringMenu.Spawn();
		}

		public override void Update()
		{
			base.Update();

			InternalEvent(EventSubtype.Menu, EventFlag.Health);

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
