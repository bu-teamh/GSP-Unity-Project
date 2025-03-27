using System.Collections.Generic;
using System;

namespace GSP.States
{
	public enum InitialState
	{
		PlayerInitialState,
		CompanionInitialState,
		MainCameraTargetInitialState,
		MainCameraInitialState
	}

	public class InitialStates
	{
		public static Dictionary<InitialState, Type> m_map
			= new Dictionary<InitialState, Type>
			{
				{ InitialState.PlayerInitialState, typeof(PlayerIdleState) },
				{ InitialState.CompanionInitialState, typeof(CompanionIdleState) },
				{ InitialState.MainCameraTargetInitialState, typeof(MainCameraTargetPassiveState) },
				{ InitialState.MainCameraInitialState, typeof(MainCameraPassiveState) }
			};
	}
}

