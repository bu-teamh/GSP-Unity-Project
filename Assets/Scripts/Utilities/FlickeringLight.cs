using UnityEngine;

public class BrokenLightFlicker : MonoBehaviour
{
	public float minFlickerInterval = 0.05f; // Minimum time between flickers
	public float maxFlickerInterval = 0.3f; // Maximum time between flickers
	public float minIntensity = 0f; // Off state
	public float midIntensity = 0.5f; // Half-bright state
	public float maxIntensity = 1f; // Fully on state

	private Light pointLight;
	private float timer;
	private float nextFlickerTime;

	void Start()
	{
		pointLight = GetComponent<Light>();
		SetNextFlickerTime();
	}

	void Update()
	{
		timer += Time.deltaTime;
		if (timer >= nextFlickerTime)
		{
			timer = 0;
			Flicker();
			SetNextFlickerTime();
		}
	}

	void Flicker()
	{
		if (pointLight != null)
		{
			int flickerState = Random.Range(0, 3); // Randomly pick 0, 1, or 2

			switch (flickerState)
			{
				case 0:
					pointLight.intensity = minIntensity; // Off
					break;
				case 1:
					pointLight.intensity = midIntensity; // Half brightness
					break;
				case 2:
					pointLight.intensity = maxIntensity; // Full brightness
					break;
			}
		}
	}

	void SetNextFlickerTime()
	{
		nextFlickerTime = Random.Range(minFlickerInterval, maxFlickerInterval);
	}
}
