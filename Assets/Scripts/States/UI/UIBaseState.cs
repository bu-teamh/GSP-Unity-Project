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
using GSP.Interface;

namespace GSP.States
{
	//Replace "Entity" with game object name in the class name
	public class UIBaseState : BaseState
	{
		protected new UIManager m_thisObject;

		public UIBaseState(UIManager _object) : base(_object)
		{
			m_thisObject = _object;
		}

		public UIBaseState(BaseState _state) : base(_state) { }

		//Here, assign the mediated objects like so
		protected override void GetMediations()
		{

		}
	}
}
