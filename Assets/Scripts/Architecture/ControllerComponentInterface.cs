using UnityEngine;

namespace GSP.Controller
{
	public interface ControllerComponentInterface
	{
		public void Enable();

		public void Disable();

		void SetCreator(object _object);
	}
}
