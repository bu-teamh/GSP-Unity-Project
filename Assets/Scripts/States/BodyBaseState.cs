using UnityEngine;

using GSP.Controller;

namespace GSP.States
{
	public class BodyBaseState : EntityBaseState
	{

		protected float m_epsilon = 0.0001f;

		protected float m_gravity = -3.5f;
		protected float m_currentHeight;
		protected Vector3 m_yvelocity = Vector3.zero;

		public BodyBaseState(ControllerComponent _object) : base(_object) { }

		public BodyBaseState(BaseState _state) : base(_state) { }

		public override void Update()
		{
			return;
		}

		public override void FixedUpdate()
		{
			//Gravity simulation.
			m_currentHeight = m_thisObject.transform.position.y;

			m_yvelocity.y += m_gravity * Time.fixedDeltaTime;

			m_yvelocity.y = Mathf.Clamp(m_yvelocity.y, m_gravity, 0.0f);

			m_thisObject.m_characterController.Move(m_yvelocity);

			if (!((m_thisObject.transform.position.y - m_currentHeight) < -m_epsilon))
			{
				m_yvelocity = Vector3.zero;
			}
		}
	}
}
