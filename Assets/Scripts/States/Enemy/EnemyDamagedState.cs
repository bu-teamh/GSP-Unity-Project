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

			m_waitTimer = new GameTimer(m_stunTime);

		}

		public override void Update()
		{
			base.Update();

			m_waitTimer.Start();
			m_waitTimer.Lock();

			if (m_waitTimer.Check())
			{
				InternalEvent(EventSubtype.PlayerSpotted);
				m_waitTimer.Unlock();
			}

			return;
		}

		public override void FixedUpdate()
		{ 
			base.FixedUpdate();

			Vector3 direction = m_companion.transform.position - m_thisObject.transform.position;

			//m_thisObject.transform.position = direction * Time.fixedDeltaTime;
			m_thisObject.m_characterController.Move(direction * Time.fixedDeltaTime);

			return;
		}

	}
}
