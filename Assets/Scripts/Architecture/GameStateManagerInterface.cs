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

		int? GetMaximum(GlobalValue _attribute);

		int? GetMinimum(GlobalValue _attribute);	

		void AddToValue(GlobalValue _attribute, int _value);

		void Start();

		void Update();

		void FixedUpdate();
	}
}
