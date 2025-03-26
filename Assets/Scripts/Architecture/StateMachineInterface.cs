using System.Collections;
using System.Collections.Generic;
using GSP.Events;
using UnityEngine;

namespace GSP.States
{
	public interface StateMachineInterface
	{
		public void Process(GameEvent _ev);

		void Update();

		void FixedUpdate();

		GameEvent Dequeue();
	}
}
