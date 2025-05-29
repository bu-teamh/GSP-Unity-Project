using UnityEngine;

using GSP.Controller;
using GSP.Events;

namespace GSP.States
{
	public class MainCameraMenuState : MainCameraBaseState
	{
		public MainCameraMenuState(ControllerComponent _object) : base(_object) { }

		public MainCameraMenuState(BaseState _state) : base(_state) { }

		protected override void InitializeMap()
		{
			SetTransition(typeof(MainCameraPassiveState), EventArchetype.UI, EventSubtype.Menu, EventFlag.Inactive);
		}

		protected override void Awake()
		{
			
		}

		public override void Update()
		{
			base.Update();

			return;
		}

		public override void FixedUpdate()
		{
			base.FixedUpdate();

			m_currentCamDist = Mathf.Lerp(m_currentCamDist, m_menuCamDist, m_smoothDistSpeed * Time.fixedDeltaTime);

			Vector3 targetCoords = m_cameraTarget.transform.position;

			Vector3 targetPos = new Vector3(
				targetCoords.x + m_currentCamDist,
				targetCoords.y + m_currentCamDist * m_heightMultiplier,
				targetCoords.z - m_currentCamDist
				);

			m_thisObject.transform.position = Vector3.Lerp(m_thisObject.transform.position, targetPos, m_smoothPosSpeed * Time.fixedDeltaTime);

			Vector3 direction = m_cameraTarget.transform.position - m_thisObject.transform.position;

			direction.Normalize();

			Quaternion rotation = Quaternion.LookRotation(direction);

			m_thisObject.transform.rotation = rotation;

			return;
		}
	}
}
