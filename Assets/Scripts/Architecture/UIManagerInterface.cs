using System.Collections;
using System.Collections.Generic;
using GSP.Controller;
using GSP.Events;
using GSP.States;
using Unity.VisualScripting;
using UnityEngine;

namespace GSP.Interface
{
	public interface UIManagerInterface
	{
		void Start();

		void Update();

		void FixedUpdate();

		public LocalEventHandlerInterface Handler { get; }

		public InitialState InitialState { get; }
	}
}
