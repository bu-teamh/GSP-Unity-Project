using UnityEngine;

using GSP.Controller;

namespace GSP.States
{
	public class UnboundMainCameraPassiveState : UnboundMainCameraBaseState
	{
		public UnboundMainCameraPassiveState(ControllerComponent _object) : base(_object) { }

		public UnboundMainCameraPassiveState(BaseState _state) : base(_state) { }

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

			float playerLanternDist = GetPercentDist();

			float rangeDist = m_farthestDist - m_closestDist;

			float targetCamDist = (rangeDist * playerLanternDist) + m_closestDist;

			m_currentCamDist = Mathf.Lerp(m_currentCamDist, targetCamDist, m_smoothDistSpeed * Time.fixedDeltaTime);

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

		private float GetPercentDist()
		{
			// Calculate the distance between the player and the lantern
			// private cam_target m_cameraTarget;
			//m_cameraTarget = m_mediator.GetObject(MediatedObject.CameraTarget, this) as cam_target;

			Vector3 playerPos = m_player.transform.position;
			Vector3 companionPos = m_companion.transform.position;
			float subjectDistance = Vector3.Distance(playerPos, companionPos);

			// Clamp the distance within the minimum and maximum distance range
			if (subjectDistance < m_lantMinDist)
			{
				subjectDistance = m_lantMinDist;
			}
			else if (subjectDistance > m_lantMaxDist)
			{
				subjectDistance = m_lantMaxDist;
			}

			// Normalize the distance to a range between 0 and 1
			subjectDistance -= m_lantMinDist;
			subjectDistance /= (m_lantMaxDist - m_lantMinDist);

			return subjectDistance;
		}
	}
}
