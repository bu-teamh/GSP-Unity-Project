using UnityEngine;

using GSP.Events;
using GSP.Controller;
using GSP.Timer;

namespace GSP.States
{
	public class EnemyDamagedState : EnemyBaseState
	{
		public EnemyDamagedState(ControllerComponent _object) : base(_object) { }

		public EnemyDamagedState(BaseState _state) : base(_state) { }

		protected override void InitializeMap()
		{
			SetTransition(typeof(EnemyChaseState), EventArchetype.Internal, EventSubtype.PlayerSpotted);
			SetTransition(typeof(EnemyPatrolState), EventArchetype.Internal, EventSubtype.PlayerLost);
			SetTransition(typeof(EnemyDieState), EventArchetype.Internal, EventSubtype.Death);
		}

		protected override void Awake()
		{
			base.Awake();
			m_waitTimer = new GameTimer(m_stunTime);
			m_thisObject.m_agent.ResetPath();

		}

		public override void Update()
		{
			base.Update();

			m_waitTimer.Start();
			m_waitTimer.Lock();

			if (m_waitTimer.Check())
			{
				m_attackEffect.Stop();
				InternalEvent(EventSubtype.PlayerSpotted);
				m_waitTimer.Unlock();
			}

			return;
		}

		public override void FixedUpdate()
		{ 
			base.FixedUpdate();

			m_attackEffect.Play();

			Vector3 direction = m_companion.transform.position - m_thisObject.transform.position;

			m_thisObject.m_agent.Move(direction * Time.deltaTime);

			return;
		}

	}
}
