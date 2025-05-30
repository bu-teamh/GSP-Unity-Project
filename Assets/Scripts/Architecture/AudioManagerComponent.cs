using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using GSP.Controller;
using System;
using GSP.Mediator;
using UnityEngine;
using GSP.Events;

namespace GSP.States
{
	public class AudioManagerComponent : MonoBehaviour, AudioManagerComponentInterface, StateBasedEntityInterface
	{
		public AudioSource m_ambientSource;
		public AudioSource m_musicSource;

		public AudioClip m_schoolAmb;
		public AudioClip m_cavesAmb;
		public AudioClip m_labAmb;
		public AudioClip m_schoolMusic;
		public AudioClip m_cavesMusic;
		public AudioClip m_labMusic;

		private StateMachineInterface m_stateMachine;
		private LocalEventHandlerInterface m_handler;

		private InitialState m_initialState = InitialState.AudioManagerInitialState;

		public AudioSource AmbientSource => m_ambientSource;
		public AudioSource MusicSource => m_musicSource;
		public AudioClip SchoolAmbience => m_schoolAmb;
		public AudioClip CavesAmbience => m_cavesAmb;
		public AudioClip LabsAmbience =>m_labAmb;
		public AudioClip SchoolMusic => m_schoolMusic;
		public AudioClip CavesMusic => m_cavesMusic;
		public AudioClip LabsMusic => m_labMusic;

		public LocalEventHandlerInterface Handler => m_handler;
		public InitialState InitialState => m_initialState;

		void Awake()
		{
			m_stateMachine = new StateMachine(this);
			m_handler = new LocalEventHandler();

			m_handler.Subscribe(EventArchetype.Audio);
		}

		// Start is called before the first frame update
		void Start()
		{
			m_stateMachine.Start(m_initialState);
		}

		public void Update()
		{
			GameEvent ev = null;

			if (m_handler.Dequeue(ref ev))
			{
				m_stateMachine.Process(ev);
			}

			//update attributes
			m_stateMachine.Update();

		}
	}
}
