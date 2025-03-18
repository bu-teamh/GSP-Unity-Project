using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace GSP.Mediator
{
    public class GlobalEventManager
    {
        
    }

    public class Mediator : MediatorInterface
    {
        private Dictionary<
            MediatedObject,
            object
            > m_mediatedObjectOwnership =
            new Dictionary<
                MediatedObject,
                object
                >();

        public Dictionary<MediatedObject, object> GetDict()
        {
            return m_mediatedObjectOwnership;
        }

        public object GetObject(
            MediatedObject _object
            )
        {
            object reference = default(object);
    
            try
            {
                if (!m_mediatedObjectOwnership.ContainsKey(_object))
                {
                    throw new KeyNotFoundException($"GSP: {_object} not found.");
                }
                else
                {
                    reference = m_mediatedObjectOwnership[_object];
                }

            }
            catch (KeyNotFoundException exception)
            {
                throw new KeyNotFoundException($"GSP: Mediator could not return object reference.", exception);
            }
            
            return reference;
        }

        public void SetObject(
            MediatedObject _object,
            object _reference
            )
        {
            Debug.Log($"Mediator SetObj called {_object} {_reference}.");

            try
            {
                if (m_mediatedObjectOwnership.ContainsKey(_object))
                {
                    throw new InvalidOperationException($"GSP: Mediated object {_object} already referenced.");
                }
                else
                {
                    m_mediatedObjectOwnership[_object] = _reference;

                    Debug.Log($"{_object} {_reference} registered in Mediator.");
                }
            }
            catch (InvalidOperationException exception)
            {
                throw new InvalidOperationException($"GSP: Mediator could not set {_object} with {_reference}.", exception);
            }
            
            return;
        }

        //TO DO: Implement on destroy event
        /// <summary>
        /// Removes a GameObject from the list of mediated objects. 
        /// ???Can only be called by instance of class GlovalEventManager. 
        /// ???Should only be called by GlobalEventManager when handling a "game object destroyed" event. 
        /// Prevents GetValue returning outdated attribute data of already-destroyed GameObjects.
        /// </summary>
        /// <param name="_caller">Function caller. Removal is gracefully denied if not type GameObjectManager.</param>
        /// <param name="_object">GameObject to be removed from mediated objects.</param>
        public void RemoveObject(
            MediatedObject _object
            )
        {
            try
            {
                if (m_mediatedObjectOwnership.ContainsKey(_object))
                {
                    m_mediatedObjectOwnership.Remove(_object);
                }
                else
                {
                    throw new KeyNotFoundException($"GSP: {_object} not found.");
                }
            }
            catch (KeyNotFoundException exception)
            {
                throw new KeyNotFoundException($"Mediator could not remove object.", exception);
            }
    
            return;
        }
    }
}