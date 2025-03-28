using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerTeleport : MonoBehaviour
{

	[SerializeField] private bool isTriggered = false;

	[SerializeField] private Volume m_volume;
	[SerializeField] public float weightIncrement;

	[SerializeField] private int Counter;
	[SerializeField] public bool isExposed = false;

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

	private void FixedUpdate()
	{
		if (isTriggered)
		{
			if (m_volume.weight < 1.0f && !isExposed)
			{
				m_volume.weight += weightIncrement;
			}
			else if (Counter == 0)
			{
				isExposed = true;
				p_characterController.enabled = false;
				c_characterController.enabled = false;

				MainCamera.transform.position = Offset + Destination;
				this.transform.position = Destination;
				companion.transform.position = Destination;
			}
			if (m_volume.weight > 0.0f && isExposed)
			{
				if (Counter < 20)
				{
					Counter += 1;
				}
				else
				{
					print("isdecrease");
					m_volume.weight -= weightIncrement;
				}
			}
			else if (m_volume.weight <= weightIncrement)
			{
				Counter = 0;
				isExposed = false;
				isTriggered = false;
			}
		}
		p_characterController.enabled = true;
		c_characterController.enabled = true;
	}
	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Teleport"))
		{
			isTriggered = true;
			RoomTransition transition = other.GetComponent<RoomTransition>();
			Destination = transition.GetDestination().position;

		}
	}


}
