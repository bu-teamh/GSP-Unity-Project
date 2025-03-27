//#nullable enable

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using GSP.Mediator;
using GSP.Events;
using GSP.States;
using GSP.InputHandling;

namespace GSP.Controller
{
	public class ControllerComponent : MonoBehaviour, ControllerComponentInterface
	{
		private MediatorComponentInterface m_mediator;
		private LocalEventHandlerInterface m_handler;
		public StateMachineInterface m_stateMachine;
		//a sound-player that is injected into the animator
		//an animator

		public Dictionary<MediatedObject, object> m_mediations = new Dictionary<MediatedObject, object>();
		//public IReadOnlyDictionary<, EventSubtype> InputModeKeyMap => m_inputModeKeyMap;

		public MediatedObject m_declaredMediatedObject;
		public List<MediatedObject> m_mediatedObjects;
		public List<EventArchetype> m_subscribedEvents;
		public InitialState m_initialState;

		public CharacterController m_chararacterController;

		void Awake()
		{
			m_mediator = MediatorComponent.Instance;
			m_handler = new LocalEventHandler();
			m_stateMachine = new StateMachine(this);

			m_mediator.SetObject(m_declaredMediatedObject, this);

			foreach (var archetype in new HashSet<EventArchetype>(m_subscribedEvents))
			{
				m_handler.Subscribe(archetype);
			}

			Debug.Log("Awake called");
		}

		void Start()
		{
			foreach (var mediatedObject in new HashSet<MediatedObject>(m_mediatedObjects))
			{
				if (mediatedObject != MediatedObject.Unmediated)
				{
					m_mediations[mediatedObject] = m_mediator.GetObject(mediatedObject, this);
				}
			}

			m_stateMachine.Start(m_initialState);
		}

		void Update()
		{
			GameEvent ev = null;

			if (m_handler.Dequeue(ref ev))
			{
				m_stateMachine.Process(ev);
			}

			//update attributes
			m_stateMachine.Update();

			//Pass current state to animator

			//
		}

		void FixedUpdate()
		{
			//do physics
			m_stateMachine.FixedUpdate();
		}
	}
}

