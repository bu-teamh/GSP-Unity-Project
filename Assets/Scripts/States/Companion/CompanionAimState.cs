using UnityEngine;

using GSP.Events;
using GSP.Controller;
using GSP.Mediator;

using GSP.InputHandling;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Xml.Linq;

using Unity.VisualScripting;

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
			SetTransition(typeof(CompanionAttackState), EventArchetype.GameplayInput, EventSubtype.Shoot, EventFlag.KeyDown);
			SetTransition(typeof(CompanionIdleState), EventArchetype.GameplayInput, EventSubtype.Aim, EventFlag.KeyUp);
		}

		protected override void Awake()
		{
			base.Awake();
		}

		public override void Update()
		{
			base.Update();

			return;
		}

		public override void FixedUpdate()
		{
			base.FixedUpdate();

			m_thisObject.m_volume.weight = 0.8f;
			Time.timeScale = 0.5f;

			//Poll the input mananger the current axis state of the mouse x/y (updates only when mouse is moved)
			System.Numerics.Vector2 axisState = m_inputManager.GetDualAxisState(EventSubtype.DirectAim);
			//Debug.Log("axis state:" + axisState);

			//Concvert to unity world vector
			Vector3 directAim = new Vector3(axisState.X, axisState.Y, 0.0f);
			//Debug.Log("mouse pos " + Input.mousePosition + " " + directAim);

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
				intersect.y = m_thisObject.transform.position.y;

				Vector3 m_direction = intersect - m_thisObject.transform.position;

				Debug.DrawLine(m_thisObject.transform.position, intersect);
				m_lineRenderer.enabled = true;

				m_lineRenderer.SetPosition(0, m_thisObject.transform.position);
				m_lineRenderer.SetPosition(1,intersect);

				m_targetRot = Quaternion.LookRotation(m_direction);


				//now you Quaternion.RotateTowards >>>> intersect vector
				Quaternion m_currentRot = m_thisObject.transform.rotation;
				m_thisObject.transform.rotation = Quaternion.RotateTowards(m_currentRot, m_targetRot, 1080.0f);
			}

			return;
		}
	}
}
