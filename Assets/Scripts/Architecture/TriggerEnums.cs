using GSP.Controller;
using GSP.Events;

namespace GSP.Triggers
{
	public enum ExecutionType
	{
		OneShotDiscrete,
		MultiShotDiscrete,
		OneShotContinuous,
		MultiShotContinuous
	}

	public struct ControllerExecutionBundle
	{
		public ExecutionType m_execution;
		public ControllerComponent m_component;
		public bool m_deactivate;
	}

	public struct TriggerExecutionBundle
	{
		public ExecutionType m_execution;
		public VolumeTriggerComponent m_trigger;
		public bool m_deactivate;
	}

	public struct EventExecutionBundle
	{
		public ExecutionType m_execution;
		public EventArchetype m_type;
		public EventSubtype m_subtype;
		public EventFlag m_flag;
		public EventPriority m_priority;
		public object m_data;
		public bool m_deactivate;
	}
}
