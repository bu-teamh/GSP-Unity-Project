using System;
using System.Collections;
using System.Collections.Generic;
using GSP.Controller;
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

		//should be private, public only for debug
		public Dictionary<
			MediatedGroup,
			HashSet<ControllerComponent>
			> m_mediatedGroupsOwnership =
			new Dictionary<
				MediatedGroup,
				HashSet<ControllerComponent>
				>();

		public void Initialize()
		{
			foreach (MediatedGroup group in Enum.GetValues(typeof(MediatedGroup)))
			{
				m_mediatedGroupsOwnership[group] = new HashSet<ControllerComponent>();

				Debug.Log("Mediator created group:" + group);
			}

			return;
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
            object _object
            )
        {
			bool success = false;

			try
            {
				foreach (var key in m_mediatedObjectOwnership.Keys)
				{
					if (m_mediatedObjectOwnership[key] == _object)
					{
						m_mediatedObjectOwnership.Remove(key);
						success = true;
						//was removed
					}
					else
					{
						//wasn't removed
					}
				}

				if (!success)
				{
					throw new KeyNotFoundException();
				}
            }
            catch (KeyNotFoundException)
            {
                throw new KeyNotFoundException($"Mediator could not remove object.");
            }
    
            return;
        }

		public HashSet<ControllerComponent> GetGroup(
			MediatedGroup _group
			)
		{
			HashSet<ControllerComponent> group = default(HashSet<ControllerComponent>);	

			try
			{
				if (!m_mediatedGroupsOwnership.ContainsKey(_group))
				{
					throw new KeyNotFoundException($"GSP: {_group} not found.");
				}
				else
				{
					group = m_mediatedGroupsOwnership[_group];
				}
			}
			catch (KeyNotFoundException exception)
			{
				throw new KeyNotFoundException($"GSP: Mediator could not return object reference.", exception);
			}

			return group;
		}

		public void AddToGroup(
			MediatedGroup _group,
			ControllerComponent _reference
			)
		{
			Debug.Log($"Mediator AddToGroup called {_group} {_reference}.");

			try
			{
				if (m_mediatedGroupsOwnership[_group].Add(_reference))
				{
					Debug.Log($"{_reference} registered in Mediated {_group}.");
				}
				else
				{
					throw new InvalidOperationException($"GSP: Mediated object {_reference} already referenced in {_group}.");
					
				}
			}
			catch (InvalidOperationException exception)
			{
				//Already referenced in group
				throw new InvalidOperationException($"GSP: Mediator could not set {_group} with {_reference}.", exception);
			}
			catch (KeyNotFoundException exception)
			{
				//Group was not found
				throw new KeyNotFoundException ($"GSP: Mediator could not set {_group} with {_reference}.", exception);
			}

			return;
		}
		public void RemoveFromGroups(
			ControllerComponent _object
			)
		{
			try
			{
				foreach (var key in m_mediatedGroupsOwnership.Keys)
				{
					if (m_mediatedGroupsOwnership[key].Remove(_object))
					{
						//was removed
					}
					else
					{
						//wasn't removed
					}
				}
			}
			catch (Exception exception)
			{
				throw new KeyNotFoundException($"Unhandled exception when trying to remove {_object} from all Mediated Groups.", exception);
			}

			return;
		}
	}
}
