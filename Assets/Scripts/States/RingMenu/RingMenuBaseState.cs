using System.Collections.Generic;

using UnityEngine;

using GSP.Mediator;
using GSP.Controller;

namespace GSP.States
{
	public class RingMenuBaseState : EntityBaseState
	{
		protected float m_targetRot;
		protected float m_speed;

		public RingMenuBaseState(ControllerComponent _object) : base(_object) { }

		public RingMenuBaseState(BaseState _state) : base(_state) { }

		protected override void GetMediations()
		{

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
