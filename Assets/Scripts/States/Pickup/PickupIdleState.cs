using System.Collections.Generic;

using UnityEngine;

using GSP.Mediator;
using GSP.Controller;
using GSP.Events;

namespace GSP.States
{
	public class PickupIdleState : PickupBaseState
	{
		public PickupIdleState(ControllerComponent _object) : base(_object) { }

		public PickupIdleState(BaseState _state) : base(_state) { }

		protected override void Awake()
		{
			m_inRange = false;
			m_thisObject.m_tooltip.Remove();

			return;
		}

		public override void Update()
		{
			m_thisObject.transform.rotation *= Quaternion.Euler(0, m_rotSpeed * Time.deltaTime, 0);

			if (m_inRange && !m_tooltipped)
			{
				m_thisObject.m_tooltip.Spawn();
				m_tooltipped = true;
			}
			else if (!m_inRange && m_tooltipped)
			{
				m_thisObject.m_tooltip.Remove();
				m_tooltipped= false;
			}

			return;
		}

		public override void React(GameEvent _event)
		{
			if (CompareEvent(_event, EventArchetype.GameplayInput, EventSubtype.Interact, EventFlag.KeyDown))
			{
				if (m_inRange)
				{
					m_gameStateManager.AddToGlobalValue(m_type, 1);

					m_thisObject.Remove();
				}
			}	
		}

		public override void FixedUpdate()
		{
			bool near = false;

			Collider[] nearby = Physics.OverlapSphere(m_thisObject.transform.position, 2.0f);

			foreach (Collider collider in nearby)
			{
				if (collider.GetComponentInParent<ControllerComponent>() == m_player)
				{
					near = true;
				}
			}

			m_inRange = near;

			return;
		}
	}
}
