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
		//protected float m_attackDelay = 0.5f;
		protected float m_sightRange = 15.0f;
		protected float m_attackRange = 5.0f;

		protected float m_health = 100;

		protected float m_stunTime = 2.0f;
		protected float m_bashWaitTime = 1.0f;
		protected float m_bashTime = 2.0f;
		protected GameTimer m_waitTimer;
		protected GameTimer m_bashTimer;
		protected Vector3 m_pushDirection;
		protected Vector3 m_targetDirection;


		protected int m_confidenceLevel = UnityEngine.Random.Range(0,3); // 0: Coward, 1: Wary, 2: Confident

		protected LayerMask m_companionMask = LayerMask.GetMask("Companion");

		protected Vector3 m_walkPoint;
		protected bool m_walkPointSet;
		protected bool m_alreadyAttacked;
		protected bool m_hasBuff = true;

		protected bool m_isHurt = false;
		protected bool m_dashed = false;

		protected int m_timer = 0;
		protected int m_timerTime = 50;
		protected int m_attackType = UnityEngine.Random.Range(0, 2);

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
			if(m_attackType == 1) { m_attackRange = m_sightRange;
				m_thisObject.name = (m_thisObject.name + " Ranged");
			}
		}

		public override void Update()
		{
			if(m_health <= 0)
			{
				InternalEvent(EventSubtype.Death);
			}
			return;
		}

		public override void FixedUpdate()
		{
			bool m_compHit = Physics.CheckSphere(m_thisObject.transform.position, 3.0f, m_companionMask);
			if(m_compHit && m_companion.GetState() == typeof(CompanionAttackState) && !m_isHurt)
			{
				m_isHurt = true;
				m_health -= 20;
				InternalEvent(EventSubtype.Damaged);
			}
			else if (!(m_companion.GetState() == typeof(CompanionAttackState)))
			{
				m_isHurt = false;
			}

		}
	}
}
