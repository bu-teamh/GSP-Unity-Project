using System.Collections.Generic;

using UnityEngine;

using GSP.Mediator;
using GSP.Controller;
using GSP.Events;
using GSP.Timer;

namespace GSP.States
{
	public class BossBaseState : EnemyBaseState
	{

		protected bool m_isAttacking;
		public BossBaseState(ControllerComponent _object) : base(_object) { }

		public BossBaseState(BaseState _state) : base(_state) { }

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
			m_animator = m_thisObject.GetComponentInChildren<Animator>();
			m_health = 1000;
		}

		public override void Update()
		{
			if(m_health <= 0)
			{
				InternalEvent(EventSubtype.Death);
			}

			if (m_thisObject.GetState() == typeof(BossChaseState))
			{
				m_animator.SetBool("IsMoving", true);
			}
			else
			{
				m_animator.SetBool("IsMoving", false);
			}

			if(!(m_thisObject.GetState() == typeof(BossAttackState)))
			{
				m_animator.SetBool("IsAttacking", false);
				m_isAttacking = false;
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
			}

			if(m_ultHit && m_companion.GetState() == typeof(CompanionUltAttackState) && !m_isHurt)
			{
				Debug.Log("ultimate hit me");
				m_isHurt = true;
				m_health -= 100;
			}


			if (!(m_companion.GetState() == typeof(CompanionAttackState)) || (m_companion.GetState() == typeof(CompanionUltAttackState)))
			{
				m_isHurt = false;
			}

		}
	}
}
