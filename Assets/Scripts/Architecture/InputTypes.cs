using GSP.Events;
using System.Collections.Generic;
using UnityEngine;

namespace GSP.InputHandling
{
    public enum InputMode
    {
        Keyboard,
        KeyboardMouse,
        Controller
    }

    [System.Serializable]
    public struct KeyEventPair
    {
        public KeyCode m_key;
        public EventSubtype m_type;
    }

    [System.Serializable]
    public struct AxisEventPair
    {
        public string m_axis;
        public EventSubtype m_type;
    }

    [System.Serializable]
    public struct DualAxis
    {
        public string m_xAxis;
        public string m_yAxis;
    }

    [System.Serializable]
    public struct DualAxisEventPair
    {
        public DualAxis m_dualAxis;
        public EventSubtype m_type;
    }

    [System.Serializable]
    public struct InputModeKeyMap
    {
        public InputMode m_mode;
        public List<KeyEventPair> m_keyEventPairs;
    }

    [System.Serializable]
    public struct InputModeAxisMap
    {
        public InputMode m_mode;
        public List<AxisEventPair> m_axisEventPairs;
    }

    [System.Serializable]
    public struct InputModeDualAxisMap
    {
        public InputMode m_mode;
        public List<DualAxisEventPair> m_dualAxisEventPairs;
    }
}