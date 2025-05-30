using System.Collections.Generic;

using UnityEngine;

using GSP.Mediator;
using GSP.Controller;
using GSP.Events;
using GSP.Timer;

namespace GSP.States
{
	public class EnemyBaseState : BodyBaseState
	{
		protected float m_walkPointRange = 10.0f;
		protected float m_attackDelay = 1.5f;
		protected float m_sightRange = 15.0f;
		protected float m_attackRange = 10.0f;
		protected GameTimer m_attackTimer;
		protected bool m_hitPlayer;

		protected float m_health = 100;

		protected float m_stunTime = 1.0f;
		protected float m_bashWaitTime = 0.8f;
		protected float m_bashTime = 0.4f;
		protected GameTimer m_waitTimer;
		protected GameTimer m_bashTimer;
		protected Vector3 m_pushDirection;
		protected Vector3 m_targetDirection;


		protected int m_confidenceLevel = UnityEngine.Random.Range(0,3); // 0: Coward, 1: Wary, 2: Confident

		protected ParticleSystem m_attackEffect;

		protected Animator m_animator;


		protected LayerMask m_companionMask = LayerMask.GetMask("Companion");
		protected LayerMask m_ultMask = LayerMask.GetMask("Ult");

		protected Vector3 m_walkPoint;
		protected bool m_walkPointSet;

		protected bool m_hasBuff = true;

		protected bool m_isHurt = false;
		protected bool m_isDashing = false;

		protected ControllerComponent m_player;
		protected ControllerComponent m_companion;
		protected NavMeshComponent m_navMesh;
		protected GameObject m_projectilePrefab = Resources.Load<GameObject>("Projectile");
		protected HashSet<ControllerComponent> m_projectiles = new HashSet<ControllerComponent>();
		protected GameStateManagerComponentInterface m_gameStateManager;

		public EnemyBaseState(ControllerComponent _object) : base(_object) { }

		public EnemyBaseState(BaseState _state) : base(_state) { }

		protected override void GetMediations()
		{
			m_player = (ControllerComponent)m_thisObject.m_mediatedObjects[MediatedObject.Player];
			m_companion = (ControllerComponent)m_thisObject.m_mediatedObjects[MediatedObject.Companion];
			m_navMesh = (NavMeshComponent)m_thisObject.m_mediatedObjects[MediatedObject.NavMesh];
			m_projectiles = m_thisObject.m_mediatedGroups[MediatedGroup.Projectiles];
			m_gameStateManager = (GameStateManagerComponentInterface)m_thisObject.m_mediatedObjects[MediatedObject.GameStateManager];
		}

		protected override void Awake()
		{
			if (m_thisObject.m_enemyRanged) { m_attackRange = m_sightRange; }
			else m_attackDelay = 0.8f;
			m_attackEffect = m_thisObject.GetComponent<ParticleSystem>();
			m_attackEffect.Stop();
			m_animator = m_thisObject.GetComponentInChildren<Animator>();
		}

		public override void Update()
		{
			if(m_health <= 0)
			{
				InternalEvent(EventSubtype.Death);
			}

			if (m_thisObject.GetState() == typeof(EnemyPatrolState) || m_thisObject.GetState() == typeof(EnemyChaseState))
			{
				m_animator.SetBool("IsMoving", true);
			}
			else
			{
				m_animator.SetBool("IsMoving", false);
			}

			if(!(m_thisObject.GetState() == typeof(EnemyAttackState)) || !(m_thisObject.GetState() == typeof(EnemyBashState)))
			{
				m_animator.SetBool("IsAttacking", false);
			}

			return;
		}

		public override void FixedUpdate()
		{
			bool m_compHit = Physics.CheckSphere(m_thisObject.transform.position, 3.0f, m_companionMask);
			bool m_ultHit = Physics.CheckSphere(m_thisObject.transform.position, 1.0f, m_ultMask);
			if (m_compHit && m_companion.GetState() == typeof(CompanionAttackState) && !m_isHurt)
			{
				m_isHurt = true;
				m_health -= 20;
				InternalEvent(EventSubtype.Damaged);
			}

			if(m_ultHit && m_companion.GetState() == typeof(CompanionUltAttackState) && !m_isHurt)
			{
				Debug.Log("ultimate hit me");
				m_isHurt = true;
				m_health -= 100;
				InternalEvent(EventSubtype.Damaged);
			}


			if (!(m_companion.GetState() == typeof(CompanionAttackState)) || (m_companion.GetState() == typeof(CompanionUltAttackState)))
			{
				m_isHurt = false;
			}

		}
	}
}
