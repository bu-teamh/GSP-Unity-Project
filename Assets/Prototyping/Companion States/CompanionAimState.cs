using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Xml.Linq;

using UnityEngine;

//Include if this state listens out for input:
using GSP.InputHandling;

using GSP.Events;
using GSP.Mediator;
using GSP.Controller;

namespace GSP.States
{
	//Rename Entity as your gameobject and Behaviour as your chosen state behaviour.
	public class CompanionAimState : CompanionBaseState
	{
		//Constructor doesn't need touching.
		public CompanionAimState(ControllerComponent _object) : base(_object) { }

		//Constructor doesn't need touching.
		public CompanionAimState(BaseState _state) : base(_state) { }

		//The map where should you go from this state.
		//I might change this dictionary another time to something else as it's very annoying to format
		protected override void InitializeMap()
		{
			SetTransition(typeof(CompanionIdleState), EventArchetype.Input, EventSubtype.Aim, EventFlag.KeyUp);
			SetTransition(typeof(CompanionAttackState), EventArchetype.Input, EventSubtype.Shoot, EventFlag.KeyDown);
		}

		public override void Update()
		{
			//Does base state functionality. 
			base.Update();

			//See comments in "Base" template for what should be done here (but in this case it applies only to this state).
			Debug.Log("Aim");
			return;
		}

		public override void FixedUpdate()
		{
			//Does base state physics.
			base.FixedUpdate();

			//See comments in "Base" template for what should be done here (but in this case it only applies to this state).
			m_gameObject.m_volume.weight = 0.8f;
			Time.timeScale = 0.5f;

			//Poll the input mananger the current axis state of the mouse x/y (updates only when mouse is moved)
			System.Numerics.Vector2 axisState = m_inputManager.GetDualAxisState(EventSubtype.DirectAim);

			//Concvert to unity world vector
			Vector3 directAim = new Vector3(axisState.X, axisState.Y, 0.0f);
			Debug.Log("mouse pos " + Input.mousePosition + " " + directAim);

			// Create a ray from the camera to the mouse position
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

			//Create the hit 
			RaycastHit hit;

			// Perform the raycast
			if (Physics.Raycast(ray, out hit))
			{
				//Get the intersection point
				Vector3 intersect = hit.point;

				//normalise y to current height of companion
				intersect.y = m_gameObject.transform.position.y;

				Vector3 m_direction = intersect - m_gameObject.transform.position;

				m_targetRot = Quaternion.LookRotation(m_direction);


				//now you Quaternion.RotateTowards >>>> intersect vector
				Quaternion m_currentRot = m_gameObject.transform.rotation;
				m_gameObject.transform.rotation = Quaternion.RotateTowards(m_currentRot, m_targetRot, 1080.0f);
			}

			return;
		}

		//You can add your own methods if you need. Like i.e. DoBigMathsThing() But only this state will be able to use it, because the StateMachine/CurrentState is hidden/encapsulted.
		//I'm thinking of a workaround for get/set. 
	}
}
