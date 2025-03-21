using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

public class RoomTransition : MonoBehaviour
{
	[SerializeField] public Transform Destination;

	[SerializeField] public GameObject Player;
	[SerializeField] public GameObject Companion;

	[SerializeField] public Volume m_volume;


	private void OnTriggerEnter(Collider other)
	{
		if(other.CompareTag("Player"))
		{
			Player.SetActive(false);
			Companion.SetActive(false);
			TeleportTransition();
			Player.transform.position = Destination.position;
			Player.transform.rotation = Quaternion.Inverse(Destination.rotation);
			Companion.transform.position = Destination.position;
			Companion.SetActive(true);
			Player.SetActive(true);

		}
	}


	void TeleportTransition()
	{
		if (m_volume != null)
		{
			StartCoroutine(ExposureFade());
		}
	}

	IEnumerator ExposureFade()
	{
		m_volume.weight = 0.0f;
		while (m_volume.weight != 1)
		{
			m_volume.weight += 0.1f;
			yield return new WaitForSeconds(15.0f);
		}
		yield return null;
	}



}
