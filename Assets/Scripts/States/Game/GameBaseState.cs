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
using GSP.Timer;

namespace GSP.States
{
	//Replace "Entity" with game object name in the class name
	public class GameBaseState : BaseState
	{
		protected new GameStateManager m_thisObject;

		protected ControllerComponent m_player;
		protected ControllerComponent m_companion;

		protected HashSet<ControllerComponent> m_enemies;


		protected HashSet<ControllerComponent> m_localEnemies = new();
		protected float m_entityRadius = 20.0f;

		protected GameTimer m_cooldownTimer = new GameTimer(3.0f);

		public GameBaseState(GameStateManager _object) : base(_object)
		{
			m_thisObject = _object;
		}

		public GameBaseState(BaseState _state) : base(_state) { }

		//Here, assign the mediated objects like so
		protected override void GetMediations()
		{
			m_player = (ControllerComponent)m_thisObject.m_component.m_mediatedObjects[MediatedObject.Player];
			m_companion = (ControllerComponent)m_thisObject.m_component.m_mediatedObjects[MediatedObject.Player];
			m_enemies = m_thisObject.m_component.m_mediatedGroups[MediatedGroup.Enemies];
		}


	}
}
