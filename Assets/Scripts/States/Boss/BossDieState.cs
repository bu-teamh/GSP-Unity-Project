using UnityEngine;

using GSP.Events;
using GSP.Controller;

namespace GSP.States
{
	public class BossDieState : BossBaseState
	{
		public BossDieState(ControllerComponent _object) : base(_object) { }

		public BossDieState(BaseState _state) : base(_state) { }

		protected override void Awake()
		{
			m_animator.SetBool("IsDead", true);
		}

		public override void Update()
		{

			return;
		}

		public override void FixedUpdate()
		{

			Debug.Log("i died ahhhh");
			//m_thisObject.Remove();

			return;
		}
	}
}
