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
        DirectAim, //<<controller aim
        //ToggleCombat, << legacy, delete
        Interact,
        Defend,
        //SwitchControl, << legacy, delete
        ToggleMenu,
        Aim,
        Shoot,
		Dodge,
		TogglePause,
		Unimplemented,

		/*
		AGREED CONTROLS
		Dodge = space
		Aim = R click
		Shoot = L click 
		Menu = tab
		Pause = esc
		Interact = E
		Defend = q
		*/

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
		Pause,
		UseItem,
		Normal,
		Translate,
		Revive,

		//Trigger
		Component,
		VolumeTrigger,

		//Door
		Initialised,
		Transition,

		//UI
		Fade,
		Clear,
		Menu,
		Item
    }

    public enum EventFlag
    {
        KeyDown,
        KeyUp,
		None,

		//used for combat/gameplay and menu/gameplay
		Active,
		Inactive,

		//Trigger enums
		Activate,
		Deactivate,

		//Door
		Open,
		Close,
		Finished,

		//UI
		Out,
		In,

		//Inventory
		Health,
		Fuel,
		Revive,
		Use
	}
}
