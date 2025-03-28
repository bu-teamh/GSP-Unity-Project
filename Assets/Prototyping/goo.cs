using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StretchController : MonoBehaviour
{
    // Exposed variables in the inspector
    public Transform stretchTransform;
    public float maxStretchDistance = 1.0f;
    public Color baseColor = Color.white;

    // Reference to the material
    private Material material;

    void Start()
    {
        // Get the material attached to the renderer
        material = GetComponent<Renderer>().material;
    }

    void Update()
    {
        // Update the shader properties
        if (stretchTransform != null)
        {
            material.SetVector("_WorldPoint", stretchTransform.position);
        }
        material.SetFloat("_MaxStretchDistance", maxStretchDistance);
        material.SetColor("_BaseColor", baseColor);
    }
}
