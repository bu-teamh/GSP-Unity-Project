using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using GSP.Controller;
using System;
using GSP.Mediator;
using UnityEngine;

namespace GSP.States
{
	public class GameStateManagerComponent : MonoBehaviour, GameStateManagerComponentInterface
	{
		private MediatorComponentInterface m_mediator;

		private GameStateManagerInterface m_manager;

		public Dictionary<MediatedObject, object> m_mediatedObjects = new();
		public Dictionary<MediatedGroup, HashSet<ControllerComponent>> m_mediatedGroups = new();

		void Awake()
		{
			m_mediator = MediatorComponent.Instance;
			m_mediator.SetObject(MediatedObject.GameStateManager, this);

			m_manager = new GameStateManager(this);
		}

		// Start is called before the first frame update
		void Start()
		{
			m_mediatedObjects[MediatedObject.Player] = m_mediator.GetObject(MediatedObject.Player, this);

			m_mediatedObjects[MediatedObject.Companion] = m_mediator.GetObject(MediatedObject.Companion, this);

			m_mediatedGroups[MediatedGroup.Enemies] = m_mediator.GetGroup(MediatedGroup.Enemies, this);

			m_manager.Start();
		}

		// Update is called once per frame
		void Update()
		{
			m_manager.Update();
		}

		void FixedUpdate()
		{
			m_manager.FixedUpdate();
		}

		public Type GetGameState()
		{
			return m_manager.GetState();
		}

		#nullable enable
		public int? GetGlobalValue(GlobalValue _attribute)
		{
			int? value = m_manager.GetValue(_attribute);

			return value;
		}
		#nullable disable
	}
}
