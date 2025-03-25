using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerTeleport : MonoBehaviour
{

	[SerializeField] private bool isTriggered = false;

	[SerializeField] private Volume m_volume;

	[SerializeField] public GameObject companion;

	public CharacterController p_characterController = null;
	public CharacterController c_characterController = null;

	//public RoomTransition transition = null;

	public Vector3 Destination;

	public Camera MainCamera;
	Vector3 Offset;


	//[SerializeField] public Transform randomObject;

	public RoomTransition transition = null;
	private void Awake()
	{
		c_characterController = companion.GetComponent<CharacterController>();
		p_characterController = this.GetComponent<CharacterController>();
		Offset = MainCamera.transform.position - this.transform.position;
	}
	private void Update()
	{
		if (isTriggered)
		{
			ExposureUp();
			print(Destination);
			p_characterController.enabled = false;
			c_characterController.enabled = false;

			this.transform.position = Destination;
			companion.transform.position = Destination;
			MainCamera.transform.position = Offset + Destination;

			ExposureDown();

			isTriggered = false;
		}
		p_characterController.enabled = true;
		c_characterController.enabled = true;
	}
	private void OnTriggerEnter(Collider other)
	{
		print("Enter" + other.name);
		if (other.CompareTag("Teleport"))
		{
			print(other.name);
			isTriggered = true;
			RoomTransition transition = other.GetComponent<RoomTransition>();
			Destination = transition.GetDestination().position;

		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.CompareTag("Teleport"))
		{
			isTriggered = false;
			Destination = Vector3.zero;
			transition = null;
		}
	}

	public void ExposureUp()
	{
		StartCoroutine(IncreaseExposure());
	}

	public void ExposureDown()
	{
		StartCoroutine(DecreaseExposure());
	}

	IEnumerator IncreaseExposure()
	{
		while (m_volume.weight <= 1)
		{
			m_volume.weight += 0.01f;
			yield return new WaitForSeconds(Time.deltaTime);
		}

		for (int i = 0; i < 50; i++)
		{
			yield return new WaitForSeconds(Time.deltaTime);
		}

		yield return null;
	}

	IEnumerator DecreaseExposure()
	{
		while (m_volume.weight >= 0)
		{
			m_volume.weight -= 0.01f;
			yield return new WaitForSeconds(Time.deltaTime * 2);
		}
		yield return null;
	}
}
