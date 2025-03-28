using GSP.Controller;

namespace GSP.Triggers
{
	public enum ExecutionType
	{
		OneShotDiscrete,
		MultiShotDiscrete,
		OneShotContinuous,
		MultiShotContinuous
	}

	public struct ControllerExecutionPair
	{
		public ExecutionType m_execution;
		public ControllerComponent m_component;
	}

	public struct TriggerExecutionPair
	{
		public ExecutionType m_execution;
		public VolumeTriggerComponent m_component;
	}
}
