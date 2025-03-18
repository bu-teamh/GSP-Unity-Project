using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using GSP.Events;

namespace GSP.InputHandling
{
    //<System.Numerics.Vector2> utilised to maintain class-engine agnosticism; explicitly stated to reduce ambiguity with Unity <Vector2>. 
    public class InputManager : InputManagerInterface
    {
        private PlayerPreferences m_playerPreferences;

        private readonly Dictionary<KeyCode, EventSubtype> m_inputModeKeyMap;
        private readonly Dictionary<string, EventSubtype> m_inputModeAxisMap;
        private readonly Dictionary<DualAxis, EventSubtype> m_inputModeDualAxisMap;

        private Dictionary<EventSubtype, float> m_axisStates;
        private Dictionary<EventSubtype, System.Numerics.Vector2> m_dualAxisStates;

        public IReadOnlyDictionary<KeyCode, EventSubtype> InputModeKeyMap => m_inputModeKeyMap;
        public IReadOnlyDictionary<string, EventSubtype> InputModeAxisMap => m_inputModeAxisMap;
        public IReadOnlyDictionary<DualAxis, EventSubtype> InputModeDualAxisMap => m_inputModeDualAxisMap;

        public IReadOnlyDictionary<EventSubtype, float> AxisStates => m_axisStates;
        public IReadOnlyDictionary<EventSubtype, System.Numerics.Vector2> DualAxisStates => m_dualAxisStates;

        public InputManager()
        {
            m_playerPreferences = Resources.Load<PlayerPreferences>("PlayerPreferences");

            if (m_playerPreferences == null)
            {

                //error
                return;
            }

            m_inputModeKeyMap = new Dictionary<KeyCode, EventSubtype>();
            m_inputModeAxisMap = new Dictionary<string, EventSubtype>();
            m_inputModeDualAxisMap = new Dictionary<DualAxis, EventSubtype>();

            m_axisStates = new Dictionary<EventSubtype, float>();
            m_dualAxisStates = new Dictionary<EventSubtype, System.Numerics.Vector2>();

            SetInputMode(m_playerPreferences.m_mode);
        }

        public void SetInputMode(InputMode _mode)
        {
            m_inputModeKeyMap.Clear();
            m_inputModeAxisMap.Clear();
            m_inputModeDualAxisMap.Clear();

            var keyBindings = m_playerPreferences.m_inputModeKeyMaps.Find(b => b.m_mode == _mode);
            //TODO: Wrap in nullcheck
            foreach (var mapping in keyBindings.m_keyEventPairs)
            {
                m_inputModeKeyMap[mapping.m_key] = mapping.m_type;
            }

            var axisBindings = m_playerPreferences.m_inputModeAxisMaps.Find(b => b.m_mode == _mode);
            //TODO: Wrap in nullcheck
            foreach (var mapping in axisBindings.m_axisEventPairs)
            {
                m_inputModeAxisMap[mapping.m_axis] = mapping.m_type;
            }

            var dualAxisBindings = m_playerPreferences.m_inputModeDualAxisMaps.Find(b => b.m_mode == _mode);
            //TODO: Wrap in nullcheck
            foreach (var mapping in dualAxisBindings.m_dualAxisEventPairs)
            {
                m_inputModeDualAxisMap[mapping.m_dualAxis] = mapping.m_type;
            }
        }

        public void SetAxisState(EventSubtype _input, float _state)
        {
            if (!m_axisStates.ContainsKey(_input))
            {
                m_axisStates[_input] = new float();
            }

            m_axisStates[_input] = _state;
        }

        public void SetDualAxisState(EventSubtype _axis, System.Numerics.Vector2 _state)
        {
            if (!m_dualAxisStates.ContainsKey(_axis))
            {
                m_dualAxisStates[_axis] = new System.Numerics.Vector2();
            }

            m_dualAxisStates[_axis] = _state;
        }
    }
}