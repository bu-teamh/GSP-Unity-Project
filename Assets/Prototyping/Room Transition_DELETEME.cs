using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

public class RoomTransition : MonoBehaviour
{
	[SerializeField] public Transform Destination;

	public Transform GetDestination()
	{
		return Destination;
	}
	



}
