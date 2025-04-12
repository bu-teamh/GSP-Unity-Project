using System.Collections.Generic;

using UnityEngine;

using GSP.Mediator;
using GSP.Controller;
using GSP.Events;
using GSP.InputHandling;
using GSP.Timer;

namespace GSP.States
{
	public class PlayerBaseState : BodyBaseState
	{
		protected float m_acceleration = 40;
		protected float m_deceleration = 10;
		protected float m_maxSpeed = 15;
		protected float m_maxRotSpeed = 1080;
		protected float m_rotDamping = 5;
		protected float m_dampingThreshold = 10;

		protected bool m_combatActive;

		protected GameTimer m_dashTimer;
		protected float m_dashTime = 0.3f;
		protected bool m_hasDashed;

		protected MeshRenderer m_defendSphereRenderer;


		protected Vector3 m_velocity = Vector3.zero;
		protected Quaternion m_targetRot;

		protected InputManagerComponentInterface m_inputManager;
		protected HashSet<ControllerComponent> m_enemies;

		public PlayerBaseState(ControllerComponent _object) : base(_object) { }

		public PlayerBaseState(BaseState _state) : base(_state) { }

		protected override void GetMediations()
		{
			m_inputManager = (InputManagerComponentInterface)m_thisObject.m_mediatedObjects[MediatedObject.InputManager];
			m_enemies = m_thisObject.m_mediatedGroups[MediatedGroup.Enemies];
		}

		protected override void Awake()
		{
			GameObject m_defendSphere = GameObject.Find("Defend Sphere");
			m_defendSphereRenderer = m_defendSphere.GetComponent<MeshRenderer>();
		}

		public override void Update()
		{
			base.Update();

			return;
		}

		public override void React(GameEvent _event)
		{
			if (CompareEvent(_event, EventArchetype.Gameplay, EventSubtype.Teleport))
			{
				ControllerComponent teleport = (ControllerComponent)_event.m_subject;

				m_thisObject.m_characterController.enabled = false;
				m_targetRot = teleport.transform.rotation;
				m_thisObject.transform.rotation = teleport.transform.rotation;
				m_thisObject.transform.position = teleport.transform.position;
				m_thisObject.m_characterController.enabled = true;
			}
		}

		public override void FixedUpdate()
		{
			base.FixedUpdate();

			m_defendSphereRenderer.enabled = false;

			//rotation, always calculated
			if (m_velocity != Vector3.zero)
			{
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

			return;
		}
	}
}
