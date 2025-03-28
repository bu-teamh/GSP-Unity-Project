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
			//need to check inside this function whether game object should be disabled at start
			//and if so, disable

			m_mediator = MediatorComponent.Instance;
			m_handler = new LocalEventHandler();
			m_stateMachine = new StateMachine(this);

			//only should do this if mediated object is delcared otherwise don't do this
			m_mediator.SetObject(m_declaredMediatedObject, this);

			Debug.Log("Awake called");
		}

		void OnEnable()
		{
			foreach (var archetype in new HashSet<EventArchetype>(m_subscribedEvents))
			{
				m_handler.Subscribe(archetype);
			}

			m_stateMachine.Start(m_initialState);

			//needs to add to mediated collection
		}

		void OnDisable()
		{
			m_handler.Unsubscribe();
			m_handler.PumpEvents();

			//Release state
			m_stateMachine.FreeState();

			//needs to remove from mediated collection

			//
		}

		void Start()
		{
			foreach (var mediatedObject in new HashSet<MediatedObject>(m_mediatedObjects))
			{
				//need to change this, MediatedObjects.unmediated is an abomination of a hack
				if (mediatedObject != MediatedObject.Unmediated)
				{
					m_mediations[mediatedObject] = m_mediator.GetObject(mediatedObject, this);
				}
			}
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

		public void Trigger()
		{
			gameObject.SetActive(true);
		}

		public void Disable()
		{
			gameObject.SetActive(false);
		}
	}
}

