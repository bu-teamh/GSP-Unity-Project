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
		GameInitialState,					//one day delete, doesnt need assigning in inspector
		UIInitialState,						//one day delete, doesnt need assigning in inspector
		UnboundMainCameraInitialState,		//legacy unbound camera, one day delete
		MandelaInitialState,
		PickupHealthState,
		PickupFuelState,
		PickupReviveState,
		PickupKeyState,
		PickupCogState,
		TooltipIdleState,
		WoodenDoorInitialState,
		PortcullisInitialState,
		GooInitialState,
		RingMenuInitialState,
		ScrapbookInitialState,
		TeleportFloatState,
		BossInitialState,
		AudioManagerInitialState,
		UnlockedWoodenDoorInitialState
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
				{ InitialState.UnboundMainCameraInitialState, typeof(UnboundMainCameraPassiveState) },		//legacy
				{ InitialState.EnemyInitialState, typeof(EnemyPatrolState) },
				{ InitialState.ProjectileInitialState, typeof(ProjectileIdleState) },
				{ InitialState.ChargeInitialState, typeof(ChargeIdleState) },
				{ InitialState.TeleportInitialState, typeof(TeleportListenState) },
				{ InitialState.GameInitialState, typeof(NormalGameplayState) },								//get rid
				{ InitialState.UIInitialState, typeof(UIClearState) },                                       //get rid
				{ InitialState.PickupHealthState, typeof(PickupHealthState) },
				{ InitialState.PickupFuelState, typeof(PickupFuelState) },
				{ InitialState.PickupReviveState, typeof(PickupReviveState) },
				{ InitialState.PickupKeyState, typeof(PickupKeyState) },
				{ InitialState.PickupCogState, typeof(PickupCogState) },
				{ InitialState.TooltipIdleState, typeof(TooltipIdleState) },
				{ InitialState.WoodenDoorInitialState, typeof(WoodenDoorInitialState) },
				{ InitialState.PortcullisInitialState, typeof(PortcullisInitialState) },
				{ InitialState.GooInitialState, typeof(GooIdleState) },
				{ InitialState.RingMenuInitialState, typeof(RingMenuIdleState) },
				{ InitialState.ScrapbookInitialState, typeof(ScrapbookIdleState) },
				{ InitialState.TeleportFloatState, typeof(TeleportFloatState) },
				{ InitialState.MandelaInitialState, typeof(MandelaIdleState) },
				{ InitialState.BossInitialState, typeof(BossIdleState) },
				{ InitialState.AudioManagerInitialState, typeof(AudioManagerPlayState) },
				{ InitialState.UnlockedWoodenDoorInitialState, typeof(WoodenDoorOpeningState) }
			};
	}
}

