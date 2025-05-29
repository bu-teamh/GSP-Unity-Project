using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace GSP.Triggers
{
	public interface TriggerableInterface
	{
		void Enable();

		void Disable();

		void Pause();

		void Unpause();
	}
}

