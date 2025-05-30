using System.Collections.Generic;

using UnityEngine;

using GSP.Mediator;
using GSP.Controller;
using GSP.Events;
using UnityEngine.UIElements;

namespace GSP.States
{
	public class WoodenDoorLockedState : WoodenDoorBaseState
	{
		public WoodenDoorLockedState(ControllerComponent _object) : base(_object) { }

		public WoodenDoorLockedState(BaseState _state) : base(_state) { }

		protected override void InitializeMap()
		{
			SetTransition(typeof(WoodenDoorOpeningState), EventArchetype.Internal, EventSubtype.Transition, EventFlag.Open);
		}

		public override void Update()
		{
			if (m_inRange && !m_tooltipped && typeof(NormalGameplayState).IsAssignableFrom(m_gameStateManager.GetGameState()))
			{
				m_thisObject.m_tooltip.Spawn();
				m_tooltipped = true;
			}
			else if (!m_inRange && m_tooltipped)
			{
				m_thisObject.m_tooltip.Remove();
				m_tooltipped = false;
			}

			return;
		}

		public override void React(GameEvent _event)
		{
			if (CompareEvent(_event, EventArchetype.GameplayInput, EventSubtype.Interact, EventFlag.KeyDown))
			{
				if (m_inRange &&
					typeof(NormalGameplayState).IsAssignableFrom(m_gameStateManager.GetGameState()) &&
					m_gameStateManager.GetGlobalValue(GlobalValue.InventoryKey) == 1
					)
				{
					m_unlocked = true;
					m_thisObject.m_tooltip.Remove();

					m_gameStateManager.AddToGlobalValue(GlobalValue.InventoryKey, -1);

					InternalEvent(EventSubtype.Transition, EventFlag.Open);
				}
			}

			return;
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
