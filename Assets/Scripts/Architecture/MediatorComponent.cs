using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//using GSP.Exceptions; - not necessary atm, not using custom inherited exceptions
using GSP.Mediator;
using System;
using GSP.Events;
using GSP.Controller;

public class MediatorComponent : MonoBehaviour, MediatorComponentInterface
{
    public static MediatorComponentInterface Instance => m_instance;
    private LocalEventHandlerInterface m_handler;

    private static MediatorComponent m_instance;

	//should be mediator interface, just using this to debug
    private Mediator m_mediator;

    void Awake()
    {
        // Ensure only one instance of this component exists.
        if (m_instance != null && (object)m_instance != this)
        {
            Destroy(gameObject); // Destroy duplicate instances.
        }

        m_instance = this;

        m_handler = new LocalEventHandler();

        m_mediator = new Mediator();

		m_mediator.Initialize();
    }

	void Start()
	{
		m_handler.Subscribe(EventArchetype.Lifetime);
	}

    // Update is called once per frame
    void Update()
    {
		/*
        Dictionary<MediatedObject, object> dict = m_mediator.GetDict();

        foreach (var kvp in dict)
        {
            Debug.Log($"Key: {kvp.Key}, Value: {kvp.Value}");
        }
        */

		if (m_mediator.m_mediatedGroupsOwnership[MediatedGroup.Enemies].Count > 0)
		{
			//Debug.Log($"There are {m_mediator.m_mediatedGroupsOwnership[MediatedGroup.Enemies].Count} enemies alive");
		}

		GameEvent ev = null;

		if (m_handler.Dequeue(ref ev))
        {
            //process it somehow - state machine, or what? or callback map
			//for now do this hack

			if (ev.m_type == EventArchetype.Lifetime)
			{
				if (ev.m_subtype == EventSubtype.Disable)
				{
					RemoveObject(ev.m_author);
					RemoveFromGroups((ControllerComponent)ev.m_author);
				}
			}
        }
    }

    public object GetObject(
        MediatedObject _object,
        object _caller
        )
    {
        object reference = default(object);

        try
        {
            reference = m_mediator.GetObject(_object);
        }
        catch (KeyNotFoundException exception)
        {
        
            //TODO: Pass exception to error handler
            Debug.LogWarning($"GSP: <{_caller}> asked Mediator for <{_object}> reference but it was not found.");
        }

        return reference;
    }

    public void SetObject(
        MediatedObject _object,
        object _caller
        )
    {
        Debug.Log($"MediatorComponent SetObj called {_object} {_caller}");
        try
        {
            m_mediator.SetObject(_object, _caller);
        }
        catch (InvalidOperationException exception)
        {
            //TODO: Pass exception to error handler
            //throw new MediatorUnauthorizedAccessException($"GSP: Mediator unauthorised access.", exception)
            Debug.LogWarning($"GSP: <{_caller}> made unauthorised attempt to change {_object}'s reference in Mediator.");
        }
        
        return;
    }

    private void RemoveObject(
        object _object
        )
    {
        try
        {
            m_mediator.RemoveObject(_object);
        }
        catch (KeyNotFoundException exception)
        {
            //TODO: Pass exception to error handler
            //throw new MediatorUnauthorizedAccessException($"GSP: Mediator unauthorised access.", exception)
            Debug.LogWarning($"GSP: <{_object}> was not found in Mediator.");
        }

        return;
    }

	public HashSet<ControllerComponent> GetGroup(
		MediatedGroup _group,
		object _caller
		)
	{
		HashSet<ControllerComponent> reference = default(HashSet<ControllerComponent>);

		try
		{
			reference = m_mediator.GetGroup(_group);
		}
		catch (KeyNotFoundException exception)
		{

			//TODO: Pass exception to error handler
			Debug.LogWarning($"GSP: <{_caller}> asked Mediator for <{_group}> group reference but it was not found.");
		}

		return reference;
	}

	public void AddToGroup(
	MediatedGroup _group,
	ControllerComponent _caller
	)
	{
		Debug.Log($"MediatorComponent AddToGroup {_group} by {_caller}");
		try
		{
			m_mediator.AddToGroup(_group, _caller);
		}
		catch (InvalidOperationException exception)
		{
			//TODO: Pass exception to error handler
			//throw new MediatorUnauthorizedAccessException($"GSP: Mediator unauthorised access.", exception)
			Debug.Log($"Invalid operation AddToGroup {_group}, {_caller} {exception.ToString()}");
		}
		catch (KeyNotFoundException exception)
		{
			Debug.Log($"Key Not Found AddToGroup {_group} (didn't exist), {_caller} (attemptor) {exception.ToString()}");
		}

		return;
	}

	private void RemoveFromGroups(
		ControllerComponent _object
		)
	{
		try
		{
			m_mediator.RemoveFromGroups(_object);
		}
		catch (KeyNotFoundException exception)
		{
			//TODO: Pass exception to error handler
			//throw new MediatorUnauthorizedAccessException($"GSP: Mediator unauthorised access.", exception)
			Debug.LogWarning($"GSP: <{_object}> was not found in Mediated Groups.");
		}

		return;
	}
}
