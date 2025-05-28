using System.Collections.Generic;

using UnityEngine;

using GSP.Mediator;
using GSP.Controller;

namespace GSP.States
{
	public class PickupBaseState : EntityBaseState
	{
		protected GlobalValue m_type;
		protected bool m_inRange;
		protected bool m_tooltipped;

		protected ControllerComponent m_player;
		protected GameStateManagerComponentInterface m_gameStateManager;

		protected float m_rotSpeed = 30.0f;

		public PickupBaseState(ControllerComponent _object) : base(_object) { }

		public PickupBaseState(BaseState _state) : base(_state) { }

		protected override void GetMediations()
		{
			m_player = (ControllerComponent)m_thisObject.m_mediatedObjects[MediatedObject.Player];
			m_gameStateManager = (GameStateManagerComponentInterface)m_thisObject.m_mediatedObjects[MediatedObject.GameStateManager];
		}

		protected override void Awake()
		{
			return;
		}

		public override void Update()
		{
			return;
		}

		public override void FixedUpdate()
		{
			return;
		}
	}
}
