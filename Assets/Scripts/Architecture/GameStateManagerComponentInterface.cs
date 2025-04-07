using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace GSP.States
{
	public interface GameStateManagerComponentInterface
	{
		Type GetGameState();

		int? GetGlobalValue(GlobalValue _attribute);
	}
}
