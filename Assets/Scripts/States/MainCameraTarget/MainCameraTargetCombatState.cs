using UnityEngine;

using GSP.Events;
using GSP.Controller;

namespace GSP.States
{
	public class MainCameraTargetCombatState : MainCameraTargetBaseState
	{
		public MainCameraTargetCombatState(ControllerComponent _object) : base(_object) { }

		public MainCameraTargetCombatState(BaseState _state) : base(_state) { }

		protected override void InitializeMap()
		{
			SetTransition(typeof(MainCameraTargetFollowState), EventArchetype.Gameplay, EventSubtype.Combat, EventFlag.Inactive);
		}

		public override void Update()
		{
			base.Update();

			return;
		}

		public override void FixedUpdate()
		{
			// regional check for enemies nearby, always updated
			Collider[] localObjects = Physics.OverlapSphere(m_player.transform.position, m_entityRadius);

			m_localEnemies.Clear();

			foreach (var collider in localObjects)
			{
				var controller = collider.GetComponentInParent<ControllerComponent>(); // << the enemey character controller does counts as a collider

				if (controller != null && m_enemies.Contains(controller))
				{
					m_localEnemies.Add(controller); // only add valid controllers that are in m_enemies
				}
			}

			Vector3 newTarget = Vector3.zero;

			newTarget += m_player.transform.position * m_playerWeight;

			foreach (ControllerComponent enemy in m_localEnemies)
			{
				newTarget += enemy.transform.position * m_enemyWeight;
			}

			newTarget /= (
				(
					m_localEnemies.Count *
					m_enemyWeight
				) +
				m_playerWeight
			);

			m_targetPosition = Vector3.Lerp(m_previousTarget, newTarget, Time.fixedDeltaTime * m_damping);

			m_previousTarget = m_targetPosition;

			base.FixedUpdate();

			return;
		}
	}
}
