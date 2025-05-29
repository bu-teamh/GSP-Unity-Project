using System.Collections.Generic;

using UnityEngine;

using GSP.Mediator;
using GSP.Controller;
using GSP.Events;
using UnityEngine.UIElements;

namespace GSP.States
{
	public class RingMenuIdleState : RingMenuBaseState
	{
		public RingMenuIdleState(ControllerComponent _object) : base(_object) { }

		public RingMenuIdleState(BaseState _state) : base(_state) { }

		protected override void Awake()
		{
			m_targetRot = 45.0f;
			m_speed = 10.0f;
			return;
		}

		public override void Update()
		{
			return;
		}

		public override void React(GameEvent _event)
		{
			if (CompareEvent(_event, EventArchetype.MenuInput, EventSubtype.Defend, EventFlag.KeyDown))
			{
				m_targetRot += 120.0f;
			}
		}

		public override void FixedUpdate()
		{
			Debug.Log("ring menu fixed update loop executes");

			if (Quaternion.Angle(m_thisObject.transform.rotation, Quaternion.Euler(0.0f, m_targetRot, 0.0f)) > 0.1f)
			{
				m_thisObject.transform.rotation = Quaternion.Slerp(
					m_thisObject.transform.rotation,
					Quaternion.Euler(0.0f, m_targetRot, 0.0f),
					m_speed * Time.fixedDeltaTime
					);
			}

			return;
		}
	}
}
