using System.Collections.Generic;

using UnityEngine;

using GSP.Mediator;
using GSP.Controller;

namespace GSP.States
{
	public class AudioManagerBaseState : BaseState
	{
		protected AudioSource m_ambientSource;
		protected AudioSource m_musicSource;

		protected AudioClip m_schoolAmb;
		protected AudioClip m_cavesAmb;
		protected AudioClip m_labAmb;
		protected AudioClip m_schoolMusic;
		protected AudioClip m_cavesMusic;
		protected AudioClip m_labMusic;

		public AudioManagerBaseState(StateBasedEntityInterface _object) : base(_object)
		{
			m_thisObject = _object;
		}

		public AudioManagerBaseState(BaseState _state) : base(_state) { }

		protected override void GetMediations()
		{
			AudioManagerComponentInterface manager = (AudioManagerComponentInterface)m_thisObject;

			m_ambientSource = manager.AmbientSource;
			m_musicSource = manager.MusicSource;

			m_schoolAmb = manager.SchoolAmbience;
			m_cavesAmb = manager.CavesAmbience;
			m_labAmb = manager.LabsAmbience;
			m_schoolMusic = manager.SchoolMusic;
			m_cavesMusic = manager.CavesMusic;
			m_labMusic = manager.LabsMusic;
	}

		protected override void Awake()
		{

		}
	}
}
