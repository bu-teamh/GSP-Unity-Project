using GSP.States;
using UnityEngine;

namespace GSP.Controller
{
	public interface ControllerComponentInterface
	{
		Types GetState();

		void Enable();

		void Disable();

		void SetCreator(object _object);
	}
}
