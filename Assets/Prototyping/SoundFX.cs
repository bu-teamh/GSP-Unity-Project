using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SoundFX : MonoBehaviour
{
	public enum SoundType
	{
		Footsteps,
		Damaged,
		Death,
		Hiss,
		Growl
	}

	[System.Serializable]

	public class SoundEffect
	{
		public SoundType Type;
		public AudioClip Clip;

		[Range(0f, 1f)]
		public float Volume = 1f;

		[HideInInspector]
		public AudioSource AudioSource;
	}

	public static SoundFX Instance;

	public SoundEffect[] AllSoundFX;

	private Dictionary<SoundType, SoundEffect> m_soundFXDictionary = new Dictionary<SoundType, SoundEffect>();

	private void Awake()
	{
		Instance = this;

		foreach (var f in AllSoundFX)
		{
			m_soundFXDictionary[f.Type] = f;
		}
	}

	public SoundType SelectedSoundFX;

	private void Update()
	{
	}

	public void PlayFX(SoundType type)
	{
		if(!m_soundFXDictionary.TryGetValue(type, out SoundEffect effect))
		{
			Debug.Log("Sound Type Not Found");
			return;
		}

		var SoundObj = new GameObject($"SoundEffect_{type}");
		var audioSrc = SoundObj.AddComponent<AudioSource>();

		audioSrc.clip = effect.Clip;
		audioSrc.volume = effect.Volume;

		audioSrc.Play();

		Destroy(SoundObj, effect.Clip.length);
	}


}
