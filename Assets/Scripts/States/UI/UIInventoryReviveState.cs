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
	public class UIInventoryReviveState : UIBaseState
	{
		//Constructor doesn't need touching.
		public UIInventoryReviveState(UIManager _object) : base(_object) { }

		//Constructor doesn't need touching.
		public UIInventoryReviveState(BaseState _state) : base(_state) { }

		//The map where should you go from this state.
		//I might change this dictionary another time to something else as it's very annoying to format
		protected override void InitializeMap()
		{
			SetTransition(typeof(UIClearState), EventArchetype.UI, EventSubtype.Menu, EventFlag.Inactive);
			SetTransition(typeof(UIInventoryHealthState), EventArchetype.Internal, EventSubtype.Menu, EventFlag.Revive);
			SetTransition(typeof(UIClearState), EventArchetype.Internal, EventSubtype.Item, EventFlag.Use);
		}

		protected override void Awake()
		{

		}

		public override void Update()
		{
			base.Update();

			return;
		}

		public override void React(GameEvent _event)
		{
			if (CompareEvent(_event, EventArchetype.MenuInput, EventSubtype.Defend, EventFlag.KeyDown))
			{
				InternalEvent(EventSubtype.Menu, EventFlag.Health);
			}
			else if (
				CompareEvent(_event, EventArchetype.MenuInput, EventSubtype.Interact, EventFlag.KeyDown))
			{
				InternalEvent(EventSubtype.Item, EventFlag.Use);
				SendEvent(EventPriority.Routine, EventArchetype.Gameplay, EventSubtype.UseItem, EventFlag.Revive);
			}
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
