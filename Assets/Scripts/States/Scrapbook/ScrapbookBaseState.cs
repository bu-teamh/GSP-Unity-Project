using System.Collections.Generic;

using UnityEngine;

using GSP.Mediator;
using GSP.Controller;

namespace GSP.States
{
	public class ScrapbookBaseState : EntityBaseState
	{
		protected ControllerComponent m_camera;

		public ScrapbookBaseState(ControllerComponent _object) : base(_object) { }

		public ScrapbookBaseState(BaseState _state) : base(_state) { }

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
