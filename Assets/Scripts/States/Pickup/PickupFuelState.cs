using System.Collections.Generic;

using UnityEngine;

using GSP.Mediator;
using GSP.Controller;
using GSP.Events;

namespace GSP.States
{
	public class PickupFuelState : PickupIdleState
	{
		public PickupFuelState(ControllerComponent _object) : base(_object) { }

		public PickupFuelState(BaseState _state) : base(_state) { }

		protected override void Awake()
		{
			base.Awake();

			m_type = GlobalValue.InventoryFuel;

			return;
		}

		public override void Update()
		{
			base.Update();

			return;
		}

		public override void React(GameEvent _event)
		{
			base.React(_event);

			return;
		}

		public override void FixedUpdate()
		{
			base.FixedUpdate();

			return;
		}
	}
}
