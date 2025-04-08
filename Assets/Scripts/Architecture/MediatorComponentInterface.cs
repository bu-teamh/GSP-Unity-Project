using System.Collections.Generic;
using System;
using GSP.Controller;

namespace GSP.Mediator
{
    public interface MediatorComponentInterface
    {
        object GetObject(
            MediatedObject _object,
            object _caller
        );

        void SetObject(
            MediatedObject _object,
            object _caller
        );

		public HashSet<ControllerComponent> GetGroup(
			MediatedGroup _group,
			object _caller
		);

		public void AddToGroup(
		MediatedGroup _group,
		ControllerComponent _caller
		);
	}
}
