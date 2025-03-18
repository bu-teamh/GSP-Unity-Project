using System.Collections.Generic;
using GSP.Events;
using UnityEngine;

namespace GSP.InputHandling
{
    public interface InputManagerInterface
    {
        IReadOnlyDictionary<KeyCode, EventSubtype> InputModeKeyMap { get; }
        IReadOnlyDictionary<string, EventSubtype> InputModeAxisMap { get; }
        IReadOnlyDictionary<DualAxis, EventSubtype> InputModeDualAxisMap { get; }

        public IReadOnlyDictionary<EventSubtype, float> AxisStates { get; }
        public IReadOnlyDictionary<EventSubtype, System.Numerics.Vector2> DualAxisStates { get; }

        public void SetInputMode(InputMode _mode);

        public void SetAxisState(EventSubtype _input, float _state);

        public void SetDualAxisState(EventSubtype _axis, System.Numerics.Vector2 _state);
    }
}
