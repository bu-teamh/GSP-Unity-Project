#nullable enable

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using GSP.Mediator;
using GSP.Events;
using GSP.States;

public class ControllerComponent : MonoBehaviour
{
	private MediatorComponentInterface m_mediator;
	private LocalEventHandlerInterface m_handler;
	private StateMachineInterface m_stateMachine;
	//an animator
	//a sound-player that is injected into the animator

	private Dictionary<MediatedObject, object> m_mediations;

	public MediatedObject m_declaredMediatedObject;
	public List<MediatedObject> m_mediatedObjects;
	public List<EventArchetype> m_subscribedEvents;
	public InitialState m_initialState;

	void Awake()
	{
		m_mediator = MediatorComponent.Instance;
		m_handler = new LocalEventHandler();
		m_stateMachine = new StateMachine(this, m_initialState);

		m_mediator.SetObject(m_declaredMediatedObject, this);

		foreach (var archetype in new HashSet<EventArchetype>(m_subscribedEvents))
		{
			m_handler.Subscribe(archetype);
		}
	}

	void Start()
	{
		foreach (var mediatedObject in new HashSet<MediatedObject>(m_mediatedObjects))
		{
			m_mediations[mediatedObject] = m_mediator.GetObject(mediatedObject, this);
		}
	}

	void Update()
	{
		GameEvent? ev = null;

		if (m_handler.Dequeue(ref ev))
		{
			m_stateMachine.Process(ev);
		}

		m_stateMachine.Update();

		m_stateMachine.FixedUpdate();



		//Pass current event to animator

		//
	}
}
