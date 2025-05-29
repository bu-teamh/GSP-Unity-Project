using System.Collections.Generic;

using UnityEngine;

using GSP.Mediator;
using GSP.Controller;

namespace GSP.States
{
	public class TooltipBaseState : EntityBaseState
	{
		protected ControllerComponent m_camera;
		protected GameStateManagerComponentInterface m_gsm;

		public TooltipBaseState(ControllerComponent _object) : base(_object) { }

		public TooltipBaseState(BaseState _state) : base(_state) { }

		protected override void GetMediations()
		{
			m_camera = (ControllerComponent)m_thisObject.m_mediatedObjects[MediatedObject.MainCamera];
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
