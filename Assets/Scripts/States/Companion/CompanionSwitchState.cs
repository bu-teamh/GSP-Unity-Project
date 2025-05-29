using UnityEngine;

using GSP.Controller;
using GSP.Events;

namespace GSP.States
{
	public class CompanionSwitchState : CompanionBaseState
	{
		public CompanionSwitchState(ControllerComponent _object) : base(_object) { }

		public CompanionSwitchState(BaseState _state) : base(_state) { }

		protected override void InitializeMap()
		{

		}

		protected override void Awake()
		{
			base.Awake();
		}

		public override void Update()
		{
			base.Update();

			return;
		}

		public override void FixedUpdate()
		{
			base.FixedUpdate();

			return;
		}
	}
}
