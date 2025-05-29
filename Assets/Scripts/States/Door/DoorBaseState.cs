using System.Collections.Generic;

using UnityEngine;

using GSP.Mediator;
using GSP.Controller;

namespace GSP.States
{
	public class DoorBaseState : EntityBaseState
	{
		protected ControllerComponent m_player;
		protected GameStateManagerComponentInterface m_gameStateManager;

		protected bool m_unlocked;
		protected bool m_inRange;
		protected bool m_tooltipped;
		protected float m_speed;

		public DoorBaseState(ControllerComponent _object) : base(_object) { }

		public DoorBaseState(BaseState _state) : base(_state) { }

		protected override void GetMediations()
		{
			m_player = (ControllerComponent)m_thisObject.m_mediatedObjects[MediatedObject.Player];
			m_gameStateManager = (GameStateManagerComponentInterface)m_thisObject.m_mediatedObjects[MediatedObject.GameStateManager];
		}
	}
}
