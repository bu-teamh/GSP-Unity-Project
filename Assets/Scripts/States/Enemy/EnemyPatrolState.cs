using UnityEngine;

using GSP.Events;
using GSP.Controller;

namespace GSP.States
{
	public class EnemyPatrolState : EnemyBaseState
	{
		public EnemyPatrolState(ControllerComponent _object) : base(_object) { }

		public EnemyPatrolState(BaseState _state) : base(_state) { }

		protected override void InitializeMap()
		{
			SetTransition(typeof(EnemyChaseState), EventArchetype.Internal, EventSubtype.PlayerSpotted);
			SetTransition(typeof(EnemyDieState), EventArchetype.Internal, EventSubtype.Death);
		}

		public override void Update()
		{
			base.Update();

			if (!m_walkPointSet)
			{
				SearchWalkPoint();
			}
			if (m_walkPointSet)
			{
				m_thisObject.m_agent.SetDestination(m_walkPoint);
			}

			Vector3 distanceToWalkPoint = m_thisObject.transform.position - m_walkPoint;
			if (distanceToWalkPoint.magnitude < 1f)
			{
				m_walkPointSet = false;
			}

			if(Physics.CheckSphere(m_thisObject.transform.position, m_sightRange, m_thisObject.m_playerMask))
			{
				InternalEvent(EventSubtype.PlayerSpotted);
			}

			return;
		}

		public override void FixedUpdate()
		{
			base.FixedUpdate();

			return;
		}

		private void SearchWalkPoint()
		{
			float randomZ = UnityEngine.Random.Range(-m_walkPointRange, m_walkPointRange);
			float randomX = UnityEngine.Random.Range(-m_walkPointRange, m_walkPointRange);

			m_walkPoint = new Vector3(m_thisObject.transform.position.x + randomX, m_thisObject.transform.position.y, m_thisObject.transform.position.z + randomZ);

			if (Physics.Raycast(m_walkPoint, -m_thisObject.transform.up, 2f, m_thisObject.m_groundMask))
			{
				m_walkPointSet = true;
			}
		}
	}
}
