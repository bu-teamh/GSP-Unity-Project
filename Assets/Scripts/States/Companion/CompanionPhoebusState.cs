using UnityEngine;

using GSP.Controller;
using GSP.Events;
using GSP.Timer;

namespace GSP.States
{
	public class CompanionPhoebusState : CompanionFollowState
	{
		public CompanionPhoebusState(ControllerComponent _object) : base(_object) { }

		public CompanionPhoebusState(BaseState _state) : base(_state) { }

		protected override void InitializeMap()
		{
			SetTransition(typeof(CompanionCombatState), EventArchetype.Gameplay, EventSubtype.Combat, EventFlag.Active);
		}

		protected override void Awake()
		{
			base.Awake();
			m_phoebusTimer = new GameTimer(m_phoebusTime);
			Debug.Log("in phoebus state rn");
			m_particleSystem.startColor = Color.magenta;
			m_lightsModule.rangeMultiplier += 2;
			m_lightsModule.intensityMultiplier += 2;
		}

		public override void Update()
		{
			base.Update();

			m_phoebusTimer.Start();
			m_phoebusTimer.Lock();


			if(m_phoebusTimer.Check())
			{
				m_particleSystem.startColor = Color.yellow;
				m_lightsModule.rangeMultiplier -= 2;
				m_lightsModule.intensityMultiplier -= 2;
				m_phoebusTimer.Unlock();
			}

			return;
		}

		public override void FixedUpdate()
		{
			base.FixedUpdate();

			return;
		}
	}
}
