using UnityEngine;

using GSP.Events;
using GSP.Controller;

namespace GSP.States
{
	public class EnemyDieState : EnemyBaseState
	{
		public EnemyDieState(ControllerComponent _object) : base(_object) { }

		public EnemyDieState(BaseState _state) : base(_state) { }

		protected override void Awake()
		{
			m_animator.SetBool("IsDead", true);
		}

		public override void Update()
		{
			base.Update();

			return;
		}

		public override void FixedUpdate()
		{
			base.FixedUpdate();

			Debug.Log("i died ahhhh");
			//m_thisObject.Remove();

			return;
		}
	}
}
