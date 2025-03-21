using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class ExposureFade : MonoBehaviour
{
	public Volume m_volume;

	// Update is called once per frame
	void Update()
    {
        if(m_volume != null)
		{
			m_volume.weight = Mathf.Sin(Time.realtimeSinceStartup);
		}
    }
}
