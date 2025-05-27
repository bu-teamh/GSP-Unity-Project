using System.Collections.Generic;

using UnityEngine;

using GSP.Mediator;
using GSP.Controller;

namespace GSP.States
{
	public class ProjectileBaseState : EntityBaseState
	{
		protected float m_speed = 2.0f;
		protected Rigidbody m_rigidbody;
		protected Vector3 m_direction;

		protected float lifeTime;

		protected ControllerComponent m_player;
		protected HashSet<ControllerComponent> m_enemies;
		protected HashSet<ControllerComponent> m_projectiles;

		protected GameStateManagerComponentInterface m_gameStateManager;

	

		public ProjectileBaseState(ControllerComponent _object) : base(_object) { }

		public ProjectileBaseState(BaseState _state) : base(_state) { }

		protected override void GetMediations()
		{
			m_gameStateManager = (GameStateManagerComponentInterface)m_thisObject.m_mediatedObjects[MediatedObject.GameStateManager];
			m_player = (ControllerComponent)m_thisObject.m_mediatedObjects[MediatedObject.Player];
			m_enemies = m_thisObject.m_mediatedGroups[MediatedGroup.Enemies];
			m_projectiles = m_thisObject.m_mediatedGroups[MediatedGroup.Projectiles];
		}

		protected override void Awake()
		{
			m_rigidbody = m_thisObject.GetComponent<Rigidbody>();
			m_rigidbody.excludeLayers = m_thisObject.m_enemyMask;
			m_direction = m_player.transform.position - m_thisObject.transform.position;
			lifeTime = Time.time + m_speed;
		}

		public override void Update()
		{
			if(Time.time > lifeTime) { m_thisObject.Remove(); }
			return;
		}

		public override void FixedUpdate()
		{
			m_rigidbody.velocity = m_direction.normalized * 10 * m_speed;
			return;
		}
	}
}
