using System.Collections;
using System.Collections.Generic;
using GSP.Controller;
using UnityEngine;

namespace GSP.Mediator
{
    public interface MediatorInterface
    {
		void Initialize();

        object GetObject(
            MediatedObject _object
            );

        void SetObject(
            MediatedObject _object,
            object _reference
            );

        void RemoveObject(
            object _object
            );

		HashSet<ControllerComponent> GetGroup(
			MediatedGroup _group
			);

		void AddToGroup(
			MediatedGroup _group,
			ControllerComponent _object
			);

		void RemoveFromGroups(
			ControllerComponent _object
			);
    }
}
