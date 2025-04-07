using System.Collections;
using System.Collections.Generic;
using System;
using GSP.Controller;
using GSP.Mediator;
using UnityEngine;

namespace GSP.States
{
	public interface GameStateManagerInterface
	{
		Type GetState();
		
		int? GetValue(GlobalValue _attribute);

		void Start();

		void Update();

		void FixedUpdate();
	}
}
