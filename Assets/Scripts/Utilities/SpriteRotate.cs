using UnityEngine;

public class PlayerMarkerRotation : MonoBehaviour
{
	public float rotationSpeed = 30f; // Adjust rotation speed as needed

	void Update()
	{
		//Rotate around world Y-axis without affecting position
		transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime, Space.World);
	}
}
