using GSP.Mediator;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileScript : MonoBehaviour
{
	private Rigidbody m_rigidbody;

	private void Awake()
	{
		m_rigidbody = GetComponent<Rigidbody>();
	}

	public void Ranged(float m_speed, Vector3 m_direction)
	{
		m_rigidbody.velocity = m_direction * m_speed;
	}
}
