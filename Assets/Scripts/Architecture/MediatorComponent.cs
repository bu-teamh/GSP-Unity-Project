using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//using GSP.Exceptions; - not necessary atm, not using custom inherited exceptions
using GSP.Mediator;
using System;
using GSP.Events;

public class MediatorComponent : MonoBehaviour, MediatorComponentInterface
{
    public static MediatorComponentInterface Instance => m_instance;
    private LocalEventHandlerComponentInterface m_localEventHandler;

    private static MediatorComponent m_instance;

    private Mediator m_mediator;

    // Start is called before the first frame update
    void Awake()
    {
        // Ensure only one instance of this component exists.
        if (m_instance != null && (object)m_instance != this)
        {
            Destroy(gameObject); // Destroy duplicate instances.
        }

        m_instance = this;

        m_localEventHandler = new LocalEventHandlerComponent();

        m_mediator = new Mediator();
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

        if (m_localEventHandler.HasEvents())
        {
            //process it somehow - state machine, or what? or callback map
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
        MediatedObject _object,
        object _caller
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
}
