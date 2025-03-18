using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GSP.Events
{
    public enum EventArchetype
    {
        Input
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
        Up,
        Left,
        Down,
        Right,
        Interact,
        Defend,
        SwitchControl,
        ToggleMenu,
        Aim,
        Shoot
    }

    public enum EventFlag
    {
        KeyDown,
        KeyUp
    }
}

//now

