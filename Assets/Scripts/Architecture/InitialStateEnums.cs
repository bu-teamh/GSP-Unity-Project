using System.Collections.Generic;
using System;

namespace GSP.States
{
	public enum InitialState
	{
		Player,
		Companion
	}

	public class InitialStates
	{
		public static Dictionary<InitialState, Type> m_map
			= new Dictionary<InitialState, Type>
			{
				{ InitialState.Player, typeof(PlayerIdleState) },
				{ InitialState.Companion, typeof(PlayerIdleState) }
			};
	}
}

