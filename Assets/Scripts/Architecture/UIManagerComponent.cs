using System.Collections;
using System.Collections.Generic;
using GSP.Controller;
using GSP.Mediator;
using GSP.States;
using UnityEngine;

namespace GSP.Interface
{
	public class UIManagerComponent : MonoBehaviour, UIManagerComponentInterface
	{
		private MediatorComponentInterface m_mediator;

		private UIManagerInterface m_interface;

		public Dictionary<MediatedObject, object> m_mediatedObjects = new();
		public Dictionary<MediatedGroup, HashSet<ControllerComponent>> m_mediatedGroups = new();

		//all the public values for shit that will need to be used by the states

		void Awake()
		{
			m_mediator = MediatorComponent.Instance;

			m_interface = new UIManager(this);
		}

		// Start is called before the first frame update
		void Start()
		{
			m_mediatedObjects[MediatedObject.GameStateManager] = m_mediator.GetObject(MediatedObject.GameStateManager, this);
		}

		// Update is called once per frame
		void Update()
		{
			m_interface.Update();
		}

		void FixedUpdate()
		{
			m_interface.Update();
		}
	}

}
