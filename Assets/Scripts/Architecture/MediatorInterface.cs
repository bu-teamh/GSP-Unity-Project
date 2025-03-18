using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GSP.Mediator
{
    public interface MediatorInterface
    {
        public Dictionary<MediatedObject, object> GetDict();

        object GetObject(
            MediatedObject _object
            );

        void SetObject(
            MediatedObject _object,
            object _reference
            );

        void RemoveObject(
            MediatedObject _object
            );
    }
}