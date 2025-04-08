using GSP.Events;
using GSP.Controller;
using UnityEngine;

namespace GSP.States
{
	//Rename Entity as your gameobject and Behaviour as your chosen state behaviour.
	public class TeleportListenState : TeleportBaseState
	{
		//Constructor doesn't need touching.
		public TeleportListenState(ControllerComponent _object) : base(_object) { }

		public TeleportListenState(BaseState _state) : base(_state) { }

		protected override void Awake()
		{
			Ray ray = new Ray(m_thisObject.transform.position, Vector3.down);
			RaycastHit hit;

			if (Physics.Raycast(ray, out hit, Mathf.Infinity, LayerMask.GetMask("GROUND")))
			{
				Vector3 targetPosition = hit.point + Vector3.up * m_heightAboveGround;

				//snap to the calculated position
				m_thisObject.transform.position = targetPosition;
			}
		}

		public override void Update()
		{
			//Does base state functionality. 
			base.Update();

			SendEvent(
				EventPriority.Routine,
				EventArchetype.Gameplay,
				EventSubtype.Teleport,
				EventFlag.None,
				m_thisObject
			);

			m_thisObject.Disable();

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
