using GSP.Events;

namespace GSP.InputHandling
{
    /// <summary>
    /// Interface contract for the InputManagerComponent class.
    /// </summary>
    public interface InputManagerComponentInterface
    {
        float GetAxisState(EventSubtype _subtype);

		bool KeyHeld(EventSubtype _input);

		bool AxisHeld(EventSubtype _input);

		bool DualAxisHeld(EventSubtype _input);

		System.Numerics.Vector2 GetDualAxisState(EventSubtype _subtype);
    }
}
