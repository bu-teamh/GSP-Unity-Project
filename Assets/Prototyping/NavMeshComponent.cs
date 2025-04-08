using GSP.Mediator;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavMeshComponent : MonoBehaviour
{
	private MediatorComponentInterface m_mediator;
	private void Awake()
	{
		m_mediator = MediatorComponent.Instance;

		m_mediator.SetObject(MediatedObject.NavMesh, this);
	}
}
