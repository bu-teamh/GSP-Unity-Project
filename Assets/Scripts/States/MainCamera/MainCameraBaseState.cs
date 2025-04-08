using UnityEngine;

using GSP.Mediator;
using GSP.Controller;
using GSP.Events;

namespace GSP.States
{
	public class MainCameraBaseState : EntityBaseState
	{
		protected float m_heightMultiplier = 1.5f;
		protected float m_closestDist = 7.5f;
		protected float m_farthestDist = 14.0f;
		protected float m_lantMinDist = 2.5f;
		protected float m_lantMaxDist = 9.0f;
		protected float m_smoothPosSpeed = 5.0f;
		protected float m_smoothDistSpeed = 0.8f;

		protected float m_currentCamDist;

		protected ControllerComponent m_cameraTarget;
		protected ControllerComponent m_player;
		protected ControllerComponent m_companion;

		public MainCameraBaseState(ControllerComponent _object) : base(_object) { }

		public MainCameraBaseState(BaseState _state) : base(_state) { }

		protected override void GetMediations()
		{
			m_cameraTarget = (ControllerComponent)m_thisObject.m_mediatedObjects[MediatedObject.CameraTarget];
			m_player = (ControllerComponent)m_thisObject.m_mediatedObjects[MediatedObject.Player];
			m_companion = (ControllerComponent)m_thisObject.m_mediatedObjects[MediatedObject.Companion];
		}

		public override void Update()
		{

			return;
		}

		public override void React(GameEvent _event)
		{
			if (CompareEvent(_event, EventArchetype.Gameplay, EventSubtype.Teleport))
			{
				ControllerComponent teleport = (ControllerComponent)_event.m_subject;

				m_thisObject.transform.position = new Vector3(
					teleport.transform.position.x + m_currentCamDist,
					teleport.transform.position.y + m_currentCamDist * m_heightMultiplier,
					(teleport.transform.position.z + 1) - m_currentCamDist
				);

				m_thisObject.m_characterController.enabled = true;
			}
		}

		public override void FixedUpdate()
		{

		}
	}
}
