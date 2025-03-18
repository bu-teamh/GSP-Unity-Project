using GSP.Events;

namespace GSP.InputHandling
{
    /// <summary>
    /// Interface contract for the InputManagerComponent class.
    /// </summary>
    public interface InputManagerComponentInterface
    {
        float GetAxisState(EventSubtype _subtype);

        System.Numerics.Vector2 GetDualAxisState(EventSubtype _subtype);
    }
}