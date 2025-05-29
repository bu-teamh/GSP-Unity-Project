using GSP.States;
using System;

namespace GSP.Controller
{
	public interface ControllerComponentInterface
	{
		Type GetState();

		void SetCreator(object _object);
	}
}
