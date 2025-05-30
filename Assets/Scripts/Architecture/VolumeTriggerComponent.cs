using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using GSP.Controller;
using GSP.Events;
using GSP.Mediator;
using Unity.VisualScripting;
using UnityEngine;

namespace GSP.Triggers
{
	public class VolumeTriggerComponent : MonoBehaviour, VolumeTriggerComponentInterface, TriggerableInterface
	{
		//execution type :
		// - one-shot discrete - triggers once 
		// - multi-shot discrete - triggers once, untriggers when leaves, retriggers when enters
		// - one-shot continuous - trigger every frame until disabled by some other means
		// - multi-shot continuous - triggers every frame but stops when out of volume

		private MediatorComponentInterface m_mediator;
		private LocalEventHandlerInterface m_handler;

		private ControllerComponent m_player;
		private HashSet<ControllerComponent> m_enemies;

		public List<ControllerExecutionBundle> m_components;
		public List<TriggerExecutionBundle> m_triggers;
		public List<EventExecutionBundle> m_events;

		private HashSet<ControllerExecutionBundle> m_componentGroup;
		private HashSet<TriggerExecutionBundle> m_triggerGroup;
		private HashSet<EventExecutionBundle> m_eventGroup;

		public bool m_liveOnAwake;

		public bool m_heavyTrigger;

		private bool m_live;
		private bool m_tripped;
		private bool m_initialCycle;
		private bool m_initialEpoch;
		private bool m_newEpoch;
		private bool m_paused;
		private Collider m_trippedEntity;

		void Awake()
		{
			m_mediator = MediatorComponent.Instance;
			m_handler = new LocalEventHandler(this);

			m_componentGroup = new HashSet<ControllerExecutionBundle>(m_components);
			m_triggerGroup = new HashSet<TriggerExecutionBundle>(m_triggers);
			m_eventGroup = new HashSet<EventExecutionBundle>(m_events);

			m_live = m_liveOnAwake;
			m_tripped = false;

			m_paused = false;

			m_initialCycle = true;
			m_initialEpoch = true;
			m_newEpoch = false;

			m_trippedEntity = null;
		}

		// Start is called before the first frame update
		void Start()
		{
			m_player = (ControllerComponent)m_mediator.GetObject(MediatedObject.Player, this);
			m_enemies = m_mediator.GetGroup(MediatedGroup.Enemies, this);
		}

		// Update is called once per frame
		void Update()
		{
			m_handler.Listen();

			if (m_live && m_tripped && !m_paused)
			{
				if (m_initialCycle)
				{
					OneShotCycle();
				}

				if (m_initialEpoch)
				{
					OneShotEpoch();
				}

				if (m_newEpoch)
				{
					MultiShotCycle();
				}

				MultiShotEpoch();
			}
			else if (!m_live)
			{
				m_handler.PumpEvents();

				//must find a way not to iterate the hashsets everytime, way too unperformant
				//MultiShotEpoch();
			}
		}

		private void OneShotCycle()
		{
			Cycle(ExecutionType.OneShotDiscrete);

			m_initialCycle = false;
		}

		private void OneShotEpoch()
		{
			Cycle(ExecutionType.OneShotContinuous);
		}

		private void MultiShotCycle()
		{
			Cycle(ExecutionType.MultiShotDiscrete);

			m_newEpoch = false;
		}

		private void MultiShotEpoch()
		{
			Cycle(ExecutionType.MultiShotContinuous);
		}

		private void Cycle(ExecutionType _execution)
		{
			foreach (var bundle in m_componentGroup)
			{
				if (bundle.m_execution == _execution)
				{
					EventFlag flag = EventFlag.Activate;

					if (bundle.m_deactivate)
					{
						flag = EventFlag.Deactivate;
					}

					TriggerEntity(bundle.m_component, EventSubtype.Component, flag);
				}
			}

			foreach (var bundle in m_triggerGroup)
			{
				if (bundle.m_execution == _execution)
				{
					EventFlag flag = EventFlag.Activate;

					if (bundle.m_deactivate)
					{
						flag = EventFlag.Deactivate;
					}

					TriggerEntity(bundle.m_trigger, EventSubtype.Component, flag);
				}
			}

			foreach (var bundle in m_eventGroup)
			{
				if (bundle.m_execution == _execution)
				{
					TriggerEvent(bundle);
				}
			}
		}

		private void OnTriggerEnter(Collider _collider)
		{
			if (!m_heavyTrigger)
			{
				Debug.Log("Player:" + m_player.name);

				if (_collider == m_player.GetComponentInParent<Collider>())
				{
					m_trippedEntity = _collider;

					m_newEpoch = true;

					Activate(_collider);
				}
			}
			else
			{
				HashSet<Collider> enemyColliders = new HashSet<Collider>();

				foreach (var enemy in m_enemies)
				{
					enemyColliders.Add(enemy.GetComponentInParent<Collider>());
				}	

				if (enemyColliders.Contains(_collider))
				{
					Activate(_collider);
				}
			}	
		}

		private void OnTriggerExit(Collider _collider)
		{
			if (_collider == m_trippedEntity)
			{
				Deactivate();
			}

			return;
		}

		private void TriggerEntity(object _entity, EventSubtype _subtype, EventFlag _flag)
		{
			GameEvent ev = new GameEvent(
				EventArchetype.Trigger,
				_subtype,
				EventPriority.Routine,
				_flag,
				this,
				_entity
			);

			m_handler.Dispatch(ev);
		}

		private void TriggerEvent(EventExecutionBundle _bundle)
		{
			GameEvent ev = new GameEvent(
				_bundle.m_type,
				_bundle.m_subtype,
				EventPriority.Routine,
				_bundle.m_flag,
				this,
				null,
				_bundle.m_data
			);

			m_handler.Dispatch(ev);
		}

		private void Activate(Collider _collider)
		{
			m_trippedEntity = _collider;

			m_newEpoch = true;

			m_tripped = true;
		}

		private void Deactivate()
		{
			if (m_initialEpoch)
			{
				m_initialEpoch = false;
			}

			m_tripped = false;
		}

		public void Enable ()
		{
			m_live = true;
		}

		public void Disable()
		{
			m_live = false;
		}

		public void Pause()
		{
			m_paused = true;
		}

		public void Unpause()
		{
			m_paused = false;
		}
	}
}
