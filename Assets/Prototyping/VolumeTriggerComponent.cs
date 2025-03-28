using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using GSP.Controller;
using UnityEngine;

namespace GSP.Triggers
{
	public class VolumeTriggerComponent : MonoBehaviour, VolumeTriggerComponentInterface
	{
		//execution type :
		// - one-shot discrete - triggers once 
		// - multi-shot discrete - triggers once, untriggers when leaves, retriggers when enters
		// - one-shot continuous - trigger every frame until disabled by some other means
		// - multi-shot continuous - triggers every frame but stops when out of volume

		public List<ControllerExecutionPair> m_triggerGroup;
		public List<ControllerComponent> m_disableGroup;

		public List<TriggerExecutionPair> m_triggersTriggeredGroup;
		public List<VolumeTriggerComponent> m_triggersDisabledGroup;

		private HashSet<ControllerExecutionPair> m_oneShotDiscreteComponents;
		private HashSet<ControllerExecutionPair> m_multishotDiscreteComponents;
		private HashSet<ControllerExecutionPair> m_oneShotContinuousComponents;
		private HashSet<ControllerExecutionPair> m_multiShotContinuousComponents;
		private HashSet<TriggerExecutionPair> m_oneShotDiscreteTriggers;
		private HashSet<TriggerExecutionPair> m_multiShotDiscreteTriggers;
		private HashSet<TriggerExecutionPair> m_oneShotContinuousTriggers;
		private HashSet<TriggerExecutionPair> m_multiShotContinuousTriggers;

		void Awake()
		{
			foreach (var pair in m_triggerGroup)
			{
				if (pair.m_execution == ExecutionType.OneShotDiscrete)
				{
					m_oneShotDiscreteComponents.Add(pair);
				}
			}
		}

		// Start is called before the first frame update
		void Start()
		{

		}

		// Update is called once per frame
		void Update()
		{

		}


	}
}
