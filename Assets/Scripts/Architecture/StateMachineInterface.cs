using System.Collections;
using System.Collections.Generic;
using GSP.Events;
using UnityEngine;

namespace GSP.States
{
	public interface StateMachineInterface
	{
		void Update(GameEvent _ev);

		bool HasEvents();

		GameEvent Dequeue();
	}
}
