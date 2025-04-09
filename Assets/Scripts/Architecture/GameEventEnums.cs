using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GSP.Events
{
    public enum EventArchetype
    {
        Input, 
		Internal,
		Lifetime, //mediator's business atm
		Gameplay,
		Trigger,
		GameplayInput,
		MenuInput,
		UI
    }

    public enum EventPriority
    {
        Deferred, 
        Routine,
        Urgent,
        Critical
    }

    public enum EventSubtype
    {
        Move,
        DirectAim,
        ToggleCombat,
        Interact,
        Defend,
        SwitchControl,
        ToggleMenu,
        Aim,
        Shoot,
		Dodge,

		//Emeny emuns
		PlayerSpotted,
		PlayerInRange,
		PlayerOutRange,
		PlayerLost,
		Death,
		Damaged,

		//Lifetime enums
		Disable,

		//Gameplay enums
		Combat,
		Teleport,

		//Trigger
		Component,
		VolumeTrigger,

		//UI
		Fade,
		Clear
    }

    public enum EventFlag
    {
        KeyDown,
        KeyUp,
		None,

		//used for combat/gameplay
		Active,
		Inactive,

		//Trigger enums
		Activate,
		Deactivate,

		//UI
		Out,
		In
	}
}
