using System.Collections.Generic;

using UnityEngine;

using GSP.Mediator;
using GSP.Controller;
using GSP.Events;
using UnityEngine.UIElements;

namespace GSP.States
{
	public class GooIdleState : GooBaseState
	{
		public GooIdleState(ControllerComponent _object) : base(_object) { }

		public GooIdleState(BaseState _state) : base(_state) { }

		protected override void Awake()
		{
			base.Awake();

			m_material.SetFloat("_MaxStretchDistance", m_stretchDist);
			m_material.SetFloat("_Smoothness", m_smoothness);

			m_material.SetVector("_LightDir", -m_light.transform.forward);
			m_material.SetColor("_LightColor", m_light.color);

			return;
		}

		public override void Update()
		{
			m_wobbleTime += Time.deltaTime; // Accumulates time naturally

			m_material.SetVector("_WorldPoint", m_player.transform.position);
			m_material.SetFloat("_WobbleTime", m_wobbleTime); // Pass time to shader

			return;
		}

		public override void FixedUpdate()
		{
			Collider[] nearby = Physics.OverlapSphere(m_thisObject.transform.position, m_stretchDist/2);

			foreach (Collider collider in nearby)
			{
				if (collider.GetComponentInParent<ControllerComponent>() == m_player)
				{
					m_gameStateManager.AddToGlobalValue(GlobalValue.PlayerHealth, -1);
				}
			}

			return;
		}
	}
}
