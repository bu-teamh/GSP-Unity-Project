using System;
using System.Collections;
using System.Collections.Generic;
using GSP.Events;
using UnityEngine;

namespace GSP.States
{
	public interface StateMachineInterface
	{
		void Start(InitialState _initial);

		void FreeState();

		void Process(GameEvent _ev);

		void Update();

		void FixedUpdate();

		Type GetState();
	}
}
