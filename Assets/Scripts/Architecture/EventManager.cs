using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.UIElements;

namespace GSP.Events
{
    public class EventManager : EventManagerInterface
    {
        private List<GameEvent> m_eventQueue;

        private Dictionary<EventArchetype, HashSet<LocalEventHandlerComponentInterface>> m_subscriberMap;
        
        public EventManager()
        {
            m_eventQueue = new List<GameEvent>();
            m_subscriberMap = new Dictionary<EventArchetype, HashSet<LocalEventHandlerComponentInterface>>();
        }

        public void SubscribeListener(LocalEventHandlerComponentInterface _listener, EventArchetype _type)
        {
            if (!m_subscriberMap.ContainsKey(_type))
            {
                m_subscriberMap[_type] = new HashSet<LocalEventHandlerComponentInterface>();
            }

            m_subscriberMap[_type].Add(_listener);

            Debug.Log($"$Added {_listener} to type {_type}");
        }

        public void UnsubscribeListener(LocalEventHandlerComponentInterface _listener) 
        {
            foreach (var kvp in m_subscriberMap) 
            {
                kvp.Value.Remove(_listener);
            }
        }

        public void ProcessQueue()
        {
            //sort events by priority, urgent -> routine -> deferred
            m_eventQueue.Sort((a, b) => b.m_priority.CompareTo(a.m_priority));

            //replace this while condition with time budget -> time budget should be as long as one frame pf minimum FPS cap, if not then leave rest of events until next frame update 
            while (m_eventQueue.Count > 0)
            {
                if (m_eventQueue.Count > 0)
                {
                    GameEvent ev = Dequeue();

                    // Second highest priority, after dequeueing event imediately brodcast it + force listener to handle it now instead of wait until update cycle
                    //if (ev.m_priority == EventPriority.Urgent)
                    //{
                    //    HandleEventQuickly(ev);
                    //}
                    //else
                    //{
                        HandleEvent(ev);
                    //}
                }
                else
                {
                    break;
                }
            }
        }

        public void Enqueue(GameEvent _ev) 
        {
            if (_ev.m_priority == EventPriority.Critical)
            {
                //Highest priority, do not even queue event, just command every listener to handle it now instead of wait until update cycle
                //HandleEventQuickly(_ev);
            }
            else
            {
                m_eventQueue.Add(_ev);
            }

            return;
        }

        private GameEvent Dequeue()
        {
            GameEvent ev = m_eventQueue[0];

            m_eventQueue.RemoveAt(0);

            return ev;
        }

        private void HandleEventQuickly(GameEvent _ev)
        {
            //Broadcast(_ev);
            //ForceHandle(_ev.m_type);
        }

        private void HandleEvent(GameEvent _ev)
        {
            //mutate event based on gamestate then broadcast
            //i.e. if get input event > mutate to menu event or player event based on current gamestate
            Broadcast(_ev);
        }

        private void Broadcast(GameEvent _ev)
        {
            foreach (var listener in m_subscriberMap[_ev.m_type])
            {
                listener.Enqueue(_ev);
            }
        }

        private void ForceHandle(EventArchetype _type)
        {
            //force local event handler to handle event NOW isntead of waiting for next update cycle
        }
    }

    /* EVENT MANAGER
     * Functions:
     * - add to queue

     * Attributes:
     * queue
     * subscribers map
     * buffers
     */

}