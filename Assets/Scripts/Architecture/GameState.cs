using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GSP.States
{
	public class GameState
	{
		private object m_gameObject;



		public virtual GameState Update()
		{
			//this is the base class
			return new GameState();
		}
	}

	public class PlayerBaseState : GameState
	{
		public override GameState Update()
		{
			// this has functionality that should be done during ALL states

			GameState nextstate = new PlayerBaseState();

			//if block, if event = w, do x, else do y, nextstate = z

			return nextstate;
		}
	}

	public class PlayerMoveState : PlayerBaseState
	{
		public override GameState Update()
		{
			// does base class update method
			base.Update();

			// this state inherits from base state and has functionality that should be only done during specific state

			GameState nextstate = new PlayerMoveState();

			//if block, if event = w, do x, else do y, nextstate = z

			return nextstate;
		}
	
	}
}
