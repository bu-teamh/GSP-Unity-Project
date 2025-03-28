using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GSP.Events
{
    public enum EventArchetype
    {
        Input,
		Internal
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
		PlayerSpotted,
		PlayerInRange,
		PlayerOutRange,
		PlayerLost,
		Death
    }

    public enum EventFlag
    {
        KeyDown,
        KeyUp,
		Empty
    }
}
