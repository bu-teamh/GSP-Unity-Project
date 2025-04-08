using System.Collections.Generic;

using UnityEngine;

using GSP.Mediator;
using GSP.Controller;

namespace GSP.States
{
	public class EnemyBaseState : BodyBaseState
	{
		protected float m_walkPointRange = 10.0f;
		//protected float m_attackDelay = 0.5f;
		protected float m_sightRange = 15.0f;
		protected float m_attackRange = 5.0f;

		protected Vector3 m_walkPoint;
		protected bool m_walkPointSet;
		protected bool m_alreadyAttacked;
		protected bool m_hasBuff = true;

		protected int m_timer = 0;
		protected int m_timerTime = 50;
		protected int m_attackType = UnityEngine.Random.Range(0, 2);

		protected ControllerComponent m_player;
		protected ControllerComponent m_companion;
		protected NavMeshComponent m_navMesh;
		protected GameObject m_projectilePrefab = Resources.Load<GameObject>("Projectile");
		protected HashSet<ControllerComponent> m_projectiles = new HashSet<ControllerComponent>();

		public EnemyBaseState(ControllerComponent _object) : base(_object) { }

		public EnemyBaseState(BaseState _state) : base(_state) { }

		protected override void GetMediations()
		{
			m_player = (ControllerComponent)m_thisObject.m_mediatedObjects[MediatedObject.Player];
			m_companion = (ControllerComponent)m_thisObject.m_mediatedObjects[MediatedObject.Companion];
			m_navMesh = (NavMeshComponent)m_thisObject.m_mediatedObjects[MediatedObject.NavMesh];
			m_projectiles = m_thisObject.m_mediatedGroups[MediatedGroup.Projectiles];
		}

		protected override void Awake()
		{
			if(m_attackType == 1) { m_attackRange = m_sightRange; }
		}

		public override void Update()
		{

			return;
		}

		public override void FixedUpdate()
		{

		}
	}
}
