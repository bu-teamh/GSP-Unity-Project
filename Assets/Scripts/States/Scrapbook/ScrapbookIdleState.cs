using System.Collections.Generic;

using UnityEngine;

using GSP.Mediator;
using GSP.Controller;
using GSP.Events;
using UnityEngine.UIElements;

namespace GSP.States
{
	public class ScrapbookIdleState : ScrapbookBaseState
	{
		public ScrapbookIdleState(ControllerComponent _object) : base(_object) { }

		public ScrapbookIdleState(BaseState _state) : base(_state) { }

		protected override void Awake()
		{
			return;
		}

		public override void Update()
		{

			return;
		}

		public override void React(GameEvent _event)
		{

		}

		public override void FixedUpdate()
		{
			Vector3 direction = m_camera.transform.position - m_thisObject.transform.position;
			m_thisObject.transform.rotation = Quaternion.LookRotation(direction);

			return;
		}
	}
}
