using System.Collections.Generic;
using System;

namespace GSP.States
{
	public enum InitialState
	{
		PlayerInitialState,
		CompanionInitialState,
		MainCameraTargetInitialState,
		MainCameraInitialState,
		EnemyInitialState,
		ProjectileInitialState,
		ChargeInitialState,
		TeleportInitialState,
		GameInitialState,		//one day delete 
		UIInitialState			//one day delete
	}

	public class InitialStates
	{
		public static Dictionary<InitialState, Type> m_map
			= new Dictionary<InitialState, Type>
			{
				{ InitialState.PlayerInitialState, typeof(PlayerIdleState) },
				{ InitialState.CompanionInitialState, typeof(CompanionFollowState) },
				{ InitialState.MainCameraTargetInitialState, typeof(MainCameraTargetFollowState) },
				{ InitialState.MainCameraInitialState, typeof(MainCameraPassiveState) },
				{ InitialState.EnemyInitialState, typeof(EnemyPatrolState) },
				{ InitialState.ProjectileInitialState, typeof(ProjectileIdleState) },
				{ InitialState.ChargeInitialState, typeof(ChargeIdleState) },
				{ InitialState.TeleportInitialState, typeof(TeleportListenState) },
				{ InitialState.GameInitialState, typeof(NormalGameplayState) },
				{ InitialState.UIInitialState, typeof(UIClearState) }
			};
	}
}

