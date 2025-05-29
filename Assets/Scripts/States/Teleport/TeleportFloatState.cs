using GSP.Events;
using GSP.Controller;
using UnityEngine;
using GSP.Timer;

namespace GSP.States
{
	//Rename Entity as your gameobject and Behaviour as your chosen state behaviour.
	public class TeleportFloatState : TeleportBaseState
	{
		//Constructor doesn't need touching.
		public TeleportFloatState(ControllerComponent _object) : base(_object) { }

		public TeleportFloatState(BaseState _state) : base(_state) { }

		protected override void Awake()
		{
			m_fadeoutTimer = new GameTimer(m_fadeTime);
			m_blackoutTimer = new GameTimer(m_blackoutTime);
		}

		public override void Update()
		{
			//Does base state functionality. 
			base.Update();

			//start fadeout timer
			m_fadeoutTimer.Start();
			m_fadeoutTimer.Lock();

			//send ui fadeout
			SendEvent(
				EventPriority.Routine,
				EventArchetype.UI,
				EventSubtype.Fade,
				EventFlag.Out,
				null,
				m_fadeTime
			);

			if (m_fadeoutTimer.Check())
			{
				m_blackoutTimer.Start();
			}

			if (m_blackoutTimer.Check())
			{
				SendEvent(
					EventPriority.Routine,
					EventArchetype.Gameplay,
					EventSubtype.Teleport,
					EventFlag.None,
					m_thisObject
				);

				SendEvent(
					EventPriority.Routine,
					EventArchetype.UI,
					EventSubtype.Fade,
					EventFlag.In,
					null,
					m_fadeTime
				);

				m_fadeoutTimer.Unlock();
				m_thisObject.Disable();

				Debug.Log(m_thisObject.name + " sent a teleport event");
			}

			return;
		}

		public override void FixedUpdate()
		{
			//Does base state physics.
			base.FixedUpdate();

			//See comments in "Base" template for what should be done here (but in this case it only applies to this state).

			return;
		}
	}
}
