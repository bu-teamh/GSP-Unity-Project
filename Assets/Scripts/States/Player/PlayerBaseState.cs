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
		protected float m_rotDamping = 1;
		protected float m_dampingThreshold = 1;

		protected bool m_combatActive;

		protected GameTimer m_dashTimer;
		protected float m_dashTime = 0.3f;
		protected bool m_hasDashed;

		protected MeshRenderer m_defendSphereRenderer;
		protected GameTimer m_parryWindow;
		protected float m_parryTime = 1.0f;

		protected bool m_canDefend = true;
		protected GameTimer m_defendTimer;
		protected float m_defendResetTime = 0.6f;

		protected Animator m_animator;
		protected SoundFX m_soundFXManager;

		protected Vector3 m_velocity = Vector3.zero;
		protected Quaternion m_targetRot;

		protected InputManagerComponentInterface m_inputManager;
		protected HashSet<ControllerComponent> m_enemies;
		protected GameStateManagerComponentInterface m_gameStateManager;
		public PlayerBaseState(ControllerComponent _object) : base(_object) { }

		public PlayerBaseState(BaseState _state) : base(_state) { }

		protected override void GetMediations()
		{
			m_inputManager = (InputManagerComponentInterface)m_thisObject.m_mediatedObjects[MediatedObject.InputManager];
			m_enemies = m_thisObject.m_mediatedGroups[MediatedGroup.Enemies];
			m_gameStateManager = (GameStateManagerComponentInterface)m_thisObject.m_mediatedObjects[MediatedObject.GameStateManager];
		}

		protected override void Awake()
		{
			GameObject m_defendSphere = GameObject.Find("Defend Sphere");
			m_defendSphereRenderer = m_defendSphere.GetComponent<MeshRenderer>();
			m_defendSphereRenderer.enabled = false;
			m_animator = m_thisObject.GetComponentInChildren<Animator>();
			m_defendTimer = new GameTimer(m_defendResetTime);
			m_soundFXManager = m_thisObject.GetComponent<SoundFX>();
		}

		public override void Update()
		{
			m_defendTimer.Start();
			m_defendTimer.Lock();

			if(m_defendTimer.Check())
			{
				if(!m_canDefend)
				{
					m_canDefend = true;
				}
				m_defendTimer.Unlock();
			}

			if(m_thisObject.GetState() == typeof(PlayerIdleState))
			{
				m_animator.SetBool("IsMoving", false);
			}
			else if(m_thisObject.GetState() == typeof(PlayerMoveState))
			{
				m_animator.SetBool("IsMoving", true);
				m_soundFXManager.PlayFX(SoundFX.SoundType.Footsteps);
			}

			if(!(m_thisObject.GetState() == typeof(PlayerDashState)))
			{
				m_animator.SetBool("IsDashing", false) ;
			}


			if(m_gameStateManager.GetGlobalValue(GlobalValue.PlayerHealth) <= 0)
			{
				Debug.Log("Player Died now");
				InternalEvent(EventSubtype.Death);
			}

			//Debug.Log(m_gameStateManager.GetGlobalValue(GlobalValue.PlayerHealth));

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

			//Rotate for menu, but not working atm because event gets pumped before comp controller can react
			/*
			if (CompareEvent(_event, EventArchetype.UI, EventSubtype.Menu, EventFlag.Active))
			{
				m_thisObject.transform.rotation = Quaternion.Euler(0.0f, -45.0f, 0.0f);
			}
			*/

			if(CompareEvent(_event, EventArchetype.GameplayInput, EventSubtype.Defend, EventFlag.KeyDown))
			{
				if (m_canDefend)
				{
					InternalEvent(EventSubtype.Defend, EventFlag.KeyDown);
				}
				else Debug.Log("cant defend yet");
			}
		}

		public override void FixedUpdate()
		{
			base.FixedUpdate();

			//m_defendSphereRenderer.enabled = false;

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
