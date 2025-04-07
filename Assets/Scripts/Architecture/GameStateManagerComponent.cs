using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using GSP.Events;
using GSP.Mediator;

namespace GSP.States
{
	public class GameStateManagerComponent : MonoBehaviour, GameStateManagerComponentInterface
	{
		public EventManagerComponentInterface m_eventManager;
		public MediatorComponentInterface m_mediator;

		void Awake()
		{
			m_eventManager = EventManagerComponent.Instance;
			m_mediator = MediatorComponent.Instance;
		}

		// Start is called before the first frame update
		void Start()
		{

		}

		// Update is called once per frame
		void Update()
		{

		}
	}
}
