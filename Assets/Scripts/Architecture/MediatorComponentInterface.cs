using System.Collections.Generic;
using System;

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
    }
}