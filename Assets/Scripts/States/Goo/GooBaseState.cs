using System.Collections.Generic;

using UnityEngine;

using GSP.Mediator;
using GSP.Controller;

namespace GSP.States
{
	public class GooBaseState : EntityBaseState
	{
		protected ControllerComponent m_player;
		protected GameStateManagerComponentInterface m_gameStateManager;

		protected Material m_material;
		protected Light m_light;

		protected float m_stretchDist;
		protected float m_smoothness;
		protected float m_wobbleTime;

		public GooBaseState(ControllerComponent _object) : base(_object) { }

		public GooBaseState(BaseState _state) : base(_state) { }

		protected override void GetMediations()
		{
			m_player = (ControllerComponent)m_thisObject.m_mediatedObjects[MediatedObject.Player];
			m_gameStateManager = (GameStateManagerComponentInterface)m_thisObject.m_mediatedObjects[MediatedObject.GameStateManager];
		}

		protected override void Awake()
		{
			m_material = m_thisObject.GetComponentInChildren<Renderer>().material;
			m_light = m_thisObject.GetComponentInChildren<Light>();
			m_stretchDist = 7.5f;
			m_smoothness = 0.0f;
			m_wobbleTime = 0.0f;

			return;
		}
	}
}
