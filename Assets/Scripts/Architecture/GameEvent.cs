using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GSP.Events
{
    public class GameEvent
    {
        public Guid m_id;

        public float m_timeStamp;

        public EventPriority m_priority;

        public EventArchetype m_type;

        public EventSubtype m_subtype;

        public EventFlag m_flag;

        public object m_author;

        //constructor
        public GameEvent(EventArchetype _type, EventSubtype _subtype, EventPriority _priority, EventFlag _flag, object _author)
        {
            m_id = Guid.NewGuid();
            m_timeStamp = Time.time;
            m_priority = _priority;
            m_type = _type;
            m_subtype = _subtype;
            m_flag = _flag;
            m_author = _author;
        }

    }
}