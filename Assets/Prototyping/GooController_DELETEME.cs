using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StretchController : MonoBehaviour
{
	// Exposed variables in the inspector
	public Transform stretchTransform;
	public float maxStretchDistance = 1.0f;
	//public Color baseColor = Color.black;
	//public Color specularColor = Color.white;
	public float smoothness = 1.0f;

	// Expose a Light component in the inspector
	public Light mainLight;

	// Reference to the material
	private Material material;

	void Start()
	{
		// Get the material attached to the renderer
		material = GetComponentInChildren<Renderer>().material;
	}

	void Update()
	{
		// Update the shader properties
		if (stretchTransform != null)
		{
			material.SetVector("_WorldPoint", stretchTransform.position);
		}

		material.SetFloat("_MaxStretchDistance", maxStretchDistance);
		material.SetFloat("_Smoothness", smoothness);

		// Pass light direction and color to the shader if assigned
		if (mainLight != null)
		{
			material.SetVector("_LightDir", -mainLight.transform.forward);
			material.SetColor("_LightColor", mainLight.color);
		}
	}
}
