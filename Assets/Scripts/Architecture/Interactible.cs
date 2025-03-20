using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using GSP.Mediator;
using GSP.Events;
using GSP.States;

public class Interactible : MonoBehaviour
{
	private MediatorComponentInterface m_mediator;
	private LocalEventHandlerComponentInterface m_handler;
	private StateMachine m_stateMachine;
	//an animator
	//a sound-player that is injected into the animator
	//an attributes collection 

	private Dictionary<MediatedObject, object> m_mediations;

	public MediatedObject m_declaredMediatedObject;
	public List<MediatedObject> m_mediatedObjects;
	public List<EventArchetype> m_subscribedEvents;
	public GameState m_initialState;

	void Awake()
	{
		m_mediator = MediatorComponent.Instance;
		m_handler = new LocalEventHandlerComponent();
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
		if (m_handler.HasEvents())
		{
			m_stateMachine.Update(m_handler.Dequeue());
		}

		while (m_stateMachine.HasEvents())
		{
			m_handler.Dispatch(m_stateMachine.Dequeue());
		}

		//Pass current event to animator

		//
	}
}
