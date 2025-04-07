using GSP.States;
using System;

namespace GSP.Controller
{
	public interface ControllerComponentInterface
	{
		Type GetState();

		void Enable();

		void Disable();

		void SetCreator(object _object);
	}
}
