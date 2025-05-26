using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Xml.Linq;

using UnityEngine;


using GSP.Timer;
using GSP.Events;
using GSP.Mediator;
using GSP.Controller;
using Unity.VisualScripting;

namespace GSP.States
{
	//Rename Entity as your gameobject and Behaviour as your chosen state behaviour.
	public class MandelaIdleState : MandelaBaseState
	{
		//Constructor doesn't need touching.
		public MandelaIdleState(ControllerComponent _object) : base(_object) { }

		//Constructor doesn't need touching.
		public MandelaIdleState(BaseState _state) : base(_state) { }

		//The map where should you go from this state.
		//I might change this dictionary another time to something else as it's very annoying to format
		protected override void Awake()
		{
			base.Awake();
		}

		public override void Update()
		{
			//Does base state functionality. 
			base.Update();

			//See comments in "Base" template for what should be done here (but in this case it applies only to this state).
			m_lerpSpeed = 3f * Time.deltaTime;

			FillAmount();

			return;
		}

		public override void FixedUpdate()
		{
			//Does base state physics.
			base.FixedUpdate();

			if(m_thisObject.m_volume.profile.TryGet(out m_vignette))
			{
				m_vignette.intensity.Override(Mathf.Lerp((float)m_vignette.intensity, (float)m_gameStateManager.GetGlobalMaximum(GlobalValue.PlayerHealth) / (float)m_gameStateManager.GetGlobalValue(GlobalValue.PlayerHealth) - 1, m_lerpSpeed));
			}

			return;
		}
		void FillAmount()
		{
			m_image.fillAmount = Mathf.Lerp(m_image.fillAmount, (float)((m_gameStateManager.GetGlobalValue(GlobalValue.PlayerHealth)) / (float)(m_gameStateManager.GetGlobalMaximum(GlobalValue.PlayerHealth))), m_lerpSpeed);
		}
	}
}
