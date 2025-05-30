using System.Collections.Generic;

using UnityEngine;

using GSP.Mediator;
using GSP.Controller;
using GSP.Events;
using UnityEngine.UIElements;

namespace GSP.States
{
	public class AudioManagerPlayState : AudioManagerBaseState
	{
		public AudioManagerPlayState(StateBasedEntityInterface _object) : base(_object) { }

		public AudioManagerPlayState(BaseState _state) : base(_state) { }

		public override void React(GameEvent _event)
		{
			if (CompareEvent(_event, EventArchetype.Audio, EventSubtype.Ambient, EventFlag.School))
			{
				m_ambientSource.clip = m_schoolAmb;
				m_ambientSource.Play();
			}
			else if (CompareEvent(_event, EventArchetype.Audio, EventSubtype.Ambient, EventFlag.Caves))
			{
				m_ambientSource.clip = m_cavesAmb;
				m_ambientSource.Play();
			}
			else if (CompareEvent(_event, EventArchetype.Audio, EventSubtype.Ambient, EventFlag.Lab))
			{
				m_ambientSource.clip = m_labAmb;
				m_ambientSource.Play();
			}
			else if (CompareEvent(_event, EventArchetype.Audio, EventSubtype.Music, EventFlag.School))
			{
				m_musicSource.clip = m_schoolMusic;
				m_musicSource.Play();
			}
			else if (CompareEvent(_event, EventArchetype.Audio, EventSubtype.Music, EventFlag.Caves))
			{
				m_musicSource.clip = m_cavesMusic;
				m_musicSource.Play();
			}
			else if (CompareEvent(_event, EventArchetype.Audio, EventSubtype.Music, EventFlag.Lab))
			{
				m_musicSource.clip = m_labMusic;
				m_musicSource.Play();
			}
		}
	}
}
