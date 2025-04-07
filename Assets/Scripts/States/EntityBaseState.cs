using GSP.Controller;

namespace GSP.States
{
	public class EntityBaseState : BaseState
	{
		protected new ControllerComponent m_thisObject;

		public EntityBaseState(ControllerComponent _object) : base(_object)
		{
			m_thisObject = _object;
		}

		public EntityBaseState(BaseState _state) : base(_state) { }
	}
}
