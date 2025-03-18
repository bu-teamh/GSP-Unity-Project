using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using GSP.InputHandling;

[CreateAssetMenu(fileName = "PlayerPreferences", menuName = "PlayerPreferences", order = 1)]
public class PlayerPreferences : ScriptableObject
{
    [SerializeField]
    public InputMode m_mode;

    //Key bindings per input mode
    public List<InputModeKeyMap> m_inputModeKeyMaps = new List<InputModeKeyMap>();
    public List<InputModeAxisMap> m_inputModeAxisMaps = new List<InputModeAxisMap>();
    public List<InputModeDualAxisMap> m_inputModeDualAxisMaps = new List<InputModeDualAxisMap>();
}
