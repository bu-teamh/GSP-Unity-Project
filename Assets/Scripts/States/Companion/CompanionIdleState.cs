using UnityEngine;

using GSP.Controller;
using GSP.Events;
using GSP.Timer;

namespace GSP.States
{
	public class CompanionIdleState : CompanionBaseState
	{
		public CompanionIdleState(ControllerComponent _object) : base(_object) { }

		public CompanionIdleState(BaseState _state) : base(_state) { }

		protected override void InitializeMap()
		{
			//SetTransition(typeof(CompanionAimState), EventArchetype.GameplayInput, EventSubtype.Aim, EventFlag.KeyDown);
		}

		protected override void Awake()
		{
			base.Awake();
			m_thisObject.m_volume.weight = 1.0f;
			Time.timeScale = 1.0f;
			m_lineRenderer.enabled = false;
			m_thisObject.m_AOE.transform.localPosition = Vector3.zero;
			m_thisObject.m_AOE.SetActive(false);
		}

		public override void Update()
		{
			base.Update();

			return;
		}

		public override void FixedUpdate()
		{
			base.FixedUpdate();

			//set velocity for float toward player


			// Base Companion Stuff
			if (m_velocity != Vector3.zero)
			{
				// Calculate the target rotation based on the direction
				m_targetRot = Quaternion.LookRotation(m_velocity);

				// Extract the y-component of the target rotation
				m_targetRot = Quaternion.Euler(0, m_targetRot.eulerAngles.y, 0);

				Quaternion currentRot = m_thisObject.transform.rotation;

				// Apply damping to smooth out the final rotation
				if (Quaternion.Angle(currentRot, m_targetRot) < m_dampingThreshold)
				{
					m_thisObject.transform.rotation = Quaternion.Slerp(currentRot, m_targetRot, m_rotDamping);
				}
				else
				{
					m_thisObject.transform.rotation = Quaternion.RotateTowards(currentRot, m_targetRot, m_maxRotSpeed * Time.fixedDeltaTime);
				}
			}
			// ---------

			//--------------------- move char controller
			//clamp on y plane to player's height
			m_velocity.y = m_player.transform.position.y;

			float vely = ((m_player.transform.position.y + m_hovHeight) - m_thisObject.transform.position.y) * m_yaccel * Time.fixedDeltaTime;

			vely = Mathf.Clamp(vely, -m_maxSpeed, m_maxSpeed);

			m_velocity.y = vely;

			m_thisObject.m_characterController.Move(m_velocity * Time.fixedDeltaTime);

			return;
		}
	}
}
