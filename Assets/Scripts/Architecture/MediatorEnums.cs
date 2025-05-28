using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GSP.Mediator
{
    public enum MediatedObject
    {
		Unmediated,
        Player,
        Companion,
        CameraTarget,
        InputManager,
		NavMesh,
		GameStateManager,
		MainCamera,
		RingMenu,
		Scrapbook
    }

	public enum MediatedGroup
	{
		Enemies,
		Projectiles
	}
}
