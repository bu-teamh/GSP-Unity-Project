using GSP.Events;
using GSP.Controller;

namespace GSP.States
{
	public class ProjectileIdleState : ProjectileBaseState
	{
		public ProjectileIdleState(ControllerComponent _object) : base(_object) { }

		public ProjectileIdleState(BaseState _state) : base(_state) { }

		protected override void Awake()
		{
			base.Awake();
		}

		protected override void InitializeMap()
		{
			SetTransition(typeof(ProjectileMoveState), EventArchetype.Internal, EventSubtype.Move);
		}

		public override void Update()
		{
			base.Update();

			InternalEvent(EventSubtype.Move);

			return;
		}

		public override void FixedUpdate()
		{
			base.FixedUpdate();

			return;
		}
	}
}
