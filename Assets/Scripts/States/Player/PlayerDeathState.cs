using UnityEngine;

using GSP.Controller;
using GSP.Events;

namespace GSP.States
{
	public class PlayerDeathState : PlayerBaseState
	{
		public PlayerDeathState(ControllerComponent _object) : base(_object) { }

		public PlayerDeathState(BaseState _state) : base(_state) { }

		protected override void InitializeMap()
		{

		}

		protected override void Awake()
		{
			base.Awake();

			m_animator.SetBool("IsDead", true);
			
		}

		public override void Update()
		{
			base.Update();

			Debug.Log("player died");

			return;
		}

		public override void React(GameEvent _event)
		{
			base.React(_event);

		}

		public override void FixedUpdate()
		{
			base.FixedUpdate();

			return;
		}
	}
}
