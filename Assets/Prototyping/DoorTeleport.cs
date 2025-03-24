using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTeleport : MonoBehaviour
{
	public int doorCode;

	public CharacterController p_characterController;
	public CharacterController c_characterController;

	private void OnTriggerEnter(Collider other)
	{
		if(other.CompareTag("Player"))
		{
			p_characterController = other.GetComponent<CharacterController>();
			foreach(DoorTeleport tp in FindObjectsOfType<DoorTeleport>())
			{
				if(tp.doorCode == doorCode && tp != this)
				{
					Vector3 position = tp.gameObject.transform.position;
					Vector3 c_position = position;
					c_position.x += 2;
					p_characterController.Move(position);
					c_characterController.Move(c_position);

				}
			}
		}
	}

}
