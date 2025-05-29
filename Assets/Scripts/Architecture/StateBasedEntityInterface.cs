using GSP.Events;

namespace GSP.States
{
	public interface StateBasedEntityInterface
	{
		public LocalEventHandlerInterface Handler { get;  }

		InitialState InitialState { get; }
	}

}
