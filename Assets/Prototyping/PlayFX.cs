using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayFX : MonoBehaviour
{
	public AudioSource m_sound1FX;
	public AudioSource m_sound2FX;
	public AudioSource m_sound3FX;
	public AudioSource m_sound4FX;
	public AudioSource m_sound5FX;
	public void PlaySound1()
	{
		m_sound1FX.Play();

		float randomNum = Random.Range(0.2f, 0.6f);
		m_sound1FX.volume = randomNum;

		randomNum = Random.Range(0.8f, 1.3f);
		m_sound1FX.pitch = randomNum;
	}

	public void PlaySound2()
	{
		m_sound2FX.Play();

		float randomNum = Random.Range(0.2f, 0.6f);
		m_sound2FX.volume = randomNum;

		randomNum = Random.Range(0.8f, 1.3f);
		m_sound2FX.pitch = randomNum;
	}

	public void PlaySound3()
	{
		m_sound3FX.Play();

		float randomNum = Random.Range(0.2f, 0.6f);
		m_sound3FX.volume = randomNum;

		randomNum = Random.Range(0.8f, 1.3f);
		m_sound3FX.pitch = randomNum;
	}

	public void PlaySound4()
	{
		m_sound4FX.Play();

		float randomNum = Random.Range(0.2f, 0.6f);
		m_sound4FX.volume = randomNum;

		randomNum = Random.Range(0.8f, 1.3f);
		m_sound4FX.pitch = randomNum;
	}

	public void PlaySound5()
	{
		m_sound5FX.Play();

		float randomNum = Random.Range(0.2f, 0.6f);
		m_sound5FX.volume = randomNum;

		randomNum = Random.Range(0.8f, 1.3f);
		m_sound5FX.pitch = randomNum;
	}

}
