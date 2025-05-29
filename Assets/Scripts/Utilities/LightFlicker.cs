using UnityEngine;

public class FlickeringLight : MonoBehaviour
{
	public float minFlickerInterval = 0.05f; // Minimum time between flickers
	public float maxFlickerInterval = 0.3f; // Maximum time between flickers
	public float minIntensity = 0f; // Off state
	public float maxIntensity = 1f; // Fully on state
	public float minRange = 3f; // Minimum light range
	public float maxRange = 6f; // Maximum light range

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
			float randomIntensity = Random.Range(minIntensity, maxIntensity);
			float randomRange = Random.Range(minRange, maxRange);
			pointLight.intensity = randomIntensity;
			pointLight.range = randomRange; // Randomizing the range
		}
	}

	void SetNextFlickerTime()
	{
		nextFlickerTime = Random.Range(minFlickerInterval, maxFlickerInterval);
	}
}
