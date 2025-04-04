using GSP.Controller;
using GSP.Events;
using System.Collections.Generic;

namespace GSP.Triggers
{
	public enum ExecutionType
	{
		OneShotDiscrete,
		MultiShotDiscrete,
		OneShotContinuous,
		MultiShotContinuous
	}

	[System.Serializable]
	public struct ControllerExecutionBundle
	{
		public ExecutionType m_execution;
		public ControllerComponent m_component;
		public bool m_deactivate;
	}

	[System.Serializable]
	public struct TriggerExecutionBundle
	{
		public ExecutionType m_execution;
		public VolumeTriggerComponent m_trigger;
		public bool m_deactivate;
	}

	[System.Serializable]
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
