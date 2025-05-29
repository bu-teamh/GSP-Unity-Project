//#nullable enable

using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.AI;

using GSP.Mediator;
using GSP.Events;
using GSP.States;
using GSP.InputHandling;
using UnityEngine.Rendering;
using GSP.Triggers;

namespace GSP.Controller
{
	public class ControllerComponent : MonoBehaviour, ControllerComponentInterface, TriggerableInterface, StateBasedEntityInterface
	{
		private MediatorComponentInterface m_mediator;
		private LocalEventHandlerInterface m_handler;
		private StateMachineInterface m_stateMachine;
		//a sound-player that is injected into the animator
		//an animator

		public bool m_liveOnAwake;

		public Dictionary<MediatedObject, object> m_mediatedObjects = new Dictionary<MediatedObject, object>();
		public Dictionary<MediatedGroup, HashSet<ControllerComponent>> m_mediatedGroups = new Dictionary<MediatedGroup, HashSet<ControllerComponent>>();
		//public IReadOnlyDictionary<, EventSubtype> InputModeKeyMap => m_inputModeKeyMap;

		public MediatedObject m_declaredMediatedObject;
		public List<MediatedGroup> m_declaredMediatedGroups;
		public List<MediatedObject> m_requestedMediatedObjects;
		public List<MediatedGroup> m_requestedMediatedGroups;
		public List<EventArchetype> m_subscribedEvents;

		public InitialState m_initialState;

		public LocalEventHandlerInterface Handler => m_handler;

		public InitialState InitialState => m_initialState;

		public CharacterController m_characterController;
		public NavMeshAgent m_agent;
		public GameObject m_creator;

		public LayerMask m_playerMask;
		public LayerMask m_groundMask;
		public LayerMask m_enemyMask;
		public LayerMask m_ultMask;

		public bool m_enemyRanged;

		public GameObject m_AOE;

		public bool m_parry;

		public Volume m_volume;

		public ControllerComponent m_tooltip;

		private bool m_activated;
		private bool m_paused;

		public bool m_pausable = true;

		void Awake()
		{
			//need to check inside this function whether game object should be disabled at start
			//and if so, disable

			m_mediator = MediatorComponent.Instance;
			m_handler = new LocalEventHandler(this);
			m_stateMachine = new StateMachine(this);

			//only should do this if mediated object is delcared otherwise don't do this
			if (m_declaredMediatedObject != MediatedObject.Unmediated)
			{
				m_mediator.SetObject(m_declaredMediatedObject, this);
			}
			
			m_activated = m_liveOnAwake;
		}

		void Start()
		{
			Initialize();
		}


		void OnEnable()
		{
			Initialize();
		}

		void OnDisable()
		{
			//Unsubscribe from all events
			m_handler.Unsubscribe();

			//Get rid of any extraneous events still in the queue
			m_handler.PumpEvents();

			//Release the current state for garbage collection
			m_stateMachine.FreeState();

			//Send event to remove itself from mediator
			GameEvent ev = new GameEvent(
				EventArchetype.Lifetime,
				EventSubtype.Disable,
				EventPriority.Critical,
				EventFlag.None,
				this
			);

			m_handler.Dispatch(ev);
		}

		void Update()
		{
			m_handler.Listen();

			if (m_activated && !m_paused)
			{
				
				GameEvent ev = null;

				Debug.Log(this.name + " loop begin");
				if (m_handler.Dequeue(ref ev))
				{
					Debug.Log("Event dequeued for :" + this.name + " // was type " + ev.m_type + ev.m_subtype + ev.m_flag);

					m_stateMachine.Process(ev);
				}
				else
				{
					Debug.Log("No events in queue for :" + this.name);
				}
				Debug.Log(this.name + " loop end");


				//update attributes
				m_stateMachine.Update();

				//Pass current state to animator

				//
			}
			else
			{
				m_handler.PumpEvents();
			}
		}

		void FixedUpdate()
		{
			//do physics
			if (m_activated && !m_paused)
			{
				m_stateMachine.FixedUpdate();
			}
		}

		private void Initialize()
		{
			//Subscribe to events
			foreach (var archetype in new HashSet<EventArchetype>(m_subscribedEvents))
			{
				m_handler.Subscribe(archetype);
			}

			//Add object to any mediated groups to which it should belong (delcared in inspector)
			foreach (var mediatedGroup in m_declaredMediatedGroups)
			{
				m_mediator.AddToGroup(mediatedGroup, this);
			}

			//Get references to requested mediated unique objects
			foreach (var mediatedObject in new HashSet<MediatedObject>(m_requestedMediatedObjects))
			{
				//need to change this, MediatedObjects.unmediated is an abomination of a hack
				if (mediatedObject != MediatedObject.Unmediated)
				{
					m_mediatedObjects[mediatedObject] = m_mediator.GetObject(mediatedObject, this);
				}
			}

			//Get references to collections of mediated objects
			foreach (var mediatedGroup in new HashSet<MediatedGroup>(m_requestedMediatedGroups))
			{
				m_mediatedGroups[mediatedGroup] = m_mediator.GetGroup(mediatedGroup, this);
				//Debug.Log("medated group" + mediatedGroup + "called by" + gameObject.name);
			}

			//Initialise the state machine with the initial state
			m_stateMachine.Start(m_initialState);

			return;
		}

		public Type GetState()
		{
			return m_stateMachine.GetState();
		}

		public void Pause()
		{
			if (m_pausable)
			{
				m_paused = true;
			}
		}

		public void Unpause()
		{
			if (m_pausable)
			{
				m_paused = false;
			}
		}

		public void Enable()
		{
			m_activated = true;
			m_handler.StashEvents();
		}

		public void Disable()
		{
			m_activated = false;
			m_handler.UnstashEvents();
		}

		public void Spawn()
		{
			//pass in pos and rot later
			gameObject.SetActive(true);
		}

		public void Remove()
		{
			//reset pos and rot to 0,0,0 0,0,0
			gameObject.SetActive(false);
		}

		public void SetCreator(object _object)
		{
			m_creator = (GameObject)_object;
		}
	}
}

