using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace GSP.States
{
	public interface AudioManagerComponentInterface
	{
		AudioSource AmbientSource { get; }
		AudioSource MusicSource { get; }
		public AudioClip SchoolAmbience { get; }
		public AudioClip CavesAmbience { get; }
		public AudioClip LabsAmbience { get; }
		public AudioClip SchoolMusic { get; }
		public AudioClip CavesMusic { get; }
		public AudioClip LabsMusic { get; }
	}
}
