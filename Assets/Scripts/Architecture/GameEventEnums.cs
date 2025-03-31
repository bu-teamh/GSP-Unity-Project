using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GSP.Events
{
    public enum EventArchetype
    {
        Input,
		Internal,
		Lifetime
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

		//Emeny emuns
		PlayerSpotted,
		PlayerInRange,
		PlayerOutRange,
		PlayerLost,
		Death,

		//Lifetime enums
		Disable
    }

    public enum EventFlag
    {
        KeyDown,
        KeyUp,
		None
    }
}
