using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Xml.Linq;

using UnityEngine;

//Include if this state listens out for input:
using GSP.InputHandling;

using GSP.Timer;
using GSP.Events;
using GSP.Mediator;
using GSP.Controller;
using UnityEngine.UI;

namespace GSP.States
{
	//Rename Entity as your gameobject and Behaviour as your chosen state behaviour.
	public class ChargeIdleState : ChargeBaseState
	{
		//Constructor doesn't need touching.
		public ChargeIdleState(ControllerComponent _object) : base(_object) { }

		//Constructor doesn't need touching.
		public ChargeIdleState(BaseState _state) : base(_state) { }

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
			if (m_current > m_max)
			{
				m_current = m_max;
			}
			if (m_current < 0)
			{
				m_current = 0;
			}

			m_lerpSpeed = 3f * Time.deltaTime;

			FillCharge();
			ColorChange();

			return;
		}

		public override void FixedUpdate()
		{
			//Does base state physics.
			base.FixedUpdate();

			//See comments in "Base" template for what should be done here (but in this case it only applies to this state).
			m_current += 0.1f;

			return;
		}

		//You can add your own methods if you need. Like i.e. DoBigMathsThing() But only this state will be able to use it, because the StateMachine/CurrentState is hidden/encapsulted.
		//I'm thinking of a workaround for get/set.

		public void FillCharge()
		{
			m_image.fillAmount = Mathf.Lerp(m_image.fillAmount, m_current / m_max, m_lerpSpeed);
		}

		public void ColorChange()
		{
			m_color = Color.Lerp(Color.red, Color.green, (m_current / m_max));

			m_image.color = m_color;
		}
	}
}
