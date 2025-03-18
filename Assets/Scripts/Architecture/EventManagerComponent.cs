using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using GSP.Mediator;

namespace GSP.Events
{
    public class EventManagerComponent : MonoBehaviour, EventManagerComponentInterface
    {
        public static EventManagerComponentInterface Instance => m_instance;

        private static EventManagerComponent m_instance;

        private EventManagerInterface m_eventManager;

        private void Awake()
        {
            // Ensure only one instance of this component exists.
            if (m_instance != null && (object)m_instance != this)
            {
                Destroy(gameObject); // Destroy duplicate instances.
            }

            m_instance = this;

            m_eventManager = new EventManager();
        }

        // Update is called once per frame
        private void Update()
        {
            m_eventManager.ProcessQueue();
        }

        public void Enqueue(GameEvent _ev)
        {
            m_eventManager.Enqueue(_ev);
        }

        public void SubscribeListener(LocalEventHandlerComponentInterface _listener, EventArchetype _type)
        {
            m_eventManager.SubscribeListener(_listener, _type);
        }

        public void UnsubscribeListener(LocalEventHandlerComponentInterface _listener)
        {
            m_eventManager.UnsubscribeListener(_listener);
        }
    }
}