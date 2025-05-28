using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

using GSP.Events;
using GSP.Mediator;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine.Rendering;
using UnityEditor;

namespace GSP.InputHandling
{
    /// <summary>
    /// InputManagerComponent encapsulates an internal InputManager and interfaces with it using Unity's life-cycle methods
    /// (and other high-level points of interfacing with Unity).
    /// </summary>
    public class InputManagerComponent : MonoBehaviour, InputManagerComponentInterface
    {
        private InputManagerInterface m_inputManager;
        private LocalEventHandlerInterface m_localEventHandler;

        private MediatorComponentInterface m_mediator;

		private Queue<GameEvent> m_pauseBuffer;

        private HashSet<KeyCode> m_heldKeys;
        private HashSet<string> m_heldAxes;
        private HashSet<string> m_heldDualAxes;

        /// <summary>
        /// Called when script instance is loaded. Initialises the InputManager and <HashSet> attributes.
        /// </summary>
        private void Awake()
        {
            m_inputManager = new InputManager();
            m_localEventHandler = new LocalEventHandler();

            m_mediator = MediatorComponent.Instance;
            m_mediator.SetObject(MediatedObject.InputManager, this);

			m_pauseBuffer = new Queue<GameEvent>();

			m_heldKeys = new HashSet<KeyCode>();
            m_heldAxes = new HashSet<string>();
            m_heldDualAxes = new HashSet<string>();
        }

        /// <summary>
        /// Called before first frame update after instantiation. Retrieve EventManager reference from Mediator. 
        /// </summary>
        void Start()
        {
			m_localEventHandler.Subscribe(EventArchetype.Gameplay);

            /* Below is debug stuff to log the conntected gamepads
            string[] names = Input.GetJoystickNames();

            foreach (var name in names)
            {
                Debug.Log($"{name}");
            }
            End debug */
        }

        // Update is called once per frame
        void Update()
        {
            if (m_heldKeys.Count > 0)
            {
                CatchKeyRelease();
            }

            if (m_heldAxes.Count > 0)
            {
                CatchAxisRelease();
            }

            if (m_heldDualAxes.Count > 0)
            {
                CatchDualAxisRelease();
            }

            if (Input.anyKeyDown)
            {
                CatchKeyPress();
            }

            CatchAxisPress();

            CatchDualAxisPress();

            if (m_heldAxes.Count > 0)
            {
                CatchAxisState();
            }

            if (m_heldDualAxes.Count > 0)
            {
                CatchDualAxisState();
            }

			Listen();

			return;
        }

		private void Listen()
		{
			GameEvent ev = null;

			if (m_localEventHandler.Dequeue(ref ev))
			{
				if (ev.m_type == EventArchetype.Gameplay && ev.m_subtype == EventSubtype.Pause)
				{
					if (ev.m_flag == EventFlag.Active)
					{
						ReleaseHeld();
					}
					else if (ev.m_flag == EventFlag.Inactive)
					{
						while (m_pauseBuffer.Count() > 0)
						{
							m_localEventHandler.Dispatch(m_pauseBuffer.Dequeue());
						}
					}
				}
			}
		}

        private void CatchKeyPress()
        {
            foreach (var kvp in m_inputManager.InputModeKeyMap)
            {
                //<HashSet>.Add() returns true only if key is added (no duplicates allowed in sets).
                if (Input.GetKeyDown(kvp.Key) && m_heldKeys.Add(kvp.Key))
                {
                    GenerateEvent(kvp.Value, EventFlag.KeyDown);
                }
            }

            return;
        }

        private void CatchAxisPress()
        {
            foreach (var kvp in m_inputManager.InputModeAxisMap)
            {
                //<HashSet>.Add() returns true only if key is added (no duplicates allowed in sets).
                if (Input.GetAxisRaw(kvp.Key) != 0 && m_heldAxes.Add(kvp.Key))
                {
                    GenerateEvent(kvp.Value, EventFlag.KeyDown);
                }
            }

            return;
        }

        private void CatchDualAxisPress()
        {
            foreach (var kvp in m_inputManager.InputModeDualAxisMap)
            {
                if (
                    (
                        Input.GetAxisRaw(kvp.Key.m_xAxis) != 0 || 
                        Input.GetAxisRaw(kvp.Key.m_yAxis) != 0
                    ) &&
                    !m_heldDualAxes.Contains(kvp.Key.m_xAxis) && 
                    !m_heldDualAxes.Contains(kvp.Key.m_yAxis)
                )
                {
                    GenerateEvent(kvp.Value, EventFlag.KeyDown);

                    if (Input.GetAxisRaw(kvp.Key.m_xAxis) != 0)
                    {
                        m_heldDualAxes.Add(kvp.Key.m_xAxis);
                    }
                        
                    if (Input.GetAxisRaw(kvp.Key.m_yAxis) != 0)
                    {
                        m_heldDualAxes.Add(kvp.Key.m_yAxis);
                    }
                }
            }

            return;
        }

        private void CatchKeyRelease()
        {
            //.ToArray() allows for con-currency safe iteration, quicker than lists.
            foreach (KeyCode key in m_heldKeys.ToArray())
            {
                //<HashSet>.Add() returns true only if key is removed.
                if (!Input.GetKey(key) && m_heldKeys.Remove(key))
                {
                    GenerateEvent(m_inputManager.InputModeKeyMap[key], EventFlag.KeyUp);
                }
            }

            return;
        }

        private void CatchAxisRelease()
        {
            //.ToArray() allows for con-currency safe iteration, quicker than lists.
            foreach (var axis in m_heldAxes.ToArray())
            {
                //<HashSet>.Add() returns true only if key is removed.
                if (Input.GetAxisRaw(axis) == 0 && m_heldAxes.Remove(axis))
                {
                    GenerateEvent(m_inputManager.InputModeAxisMap[axis], EventFlag.KeyUp);
                }
            }

            return;
        }

        private void CatchDualAxisRelease()
        {
            foreach (var kvp in m_inputManager.InputModeDualAxisMap)
            {
                bool xAxisHeld = Input.GetAxisRaw(kvp.Key.m_xAxis) != 0;
                bool yAxisHeld = Input.GetAxisRaw(kvp.Key.m_yAxis) != 0;

                if (
                    (
                        !xAxisHeld && 
                        m_heldDualAxes.Remove(kvp.Key.m_xAxis)
                    ) ||
                    (
                        !yAxisHeld && 
                        m_heldDualAxes.Remove(kvp.Key.m_yAxis)
                    )
                )
                {
                    if (!xAxisHeld && !yAxisHeld)
                    {
                        GenerateEvent(kvp.Value, EventFlag.KeyUp);
                    }
                }
            }

            return;
        }

        private void CatchAxisState()
        {
            foreach (var kvp in m_inputManager.InputModeAxisMap)
            {
                if (m_heldAxes.Contains(kvp.Key))
                {
                    float state = Input.GetAxisRaw(kvp.Key);

                    m_inputManager.SetAxisState(kvp.Value, state);
                }
            }

            return;
        }

        //<System.Numerics.Vector2> utilised to maintain class-engine agnosticism for the internal implementation; explicitly stated to reduce ambiguity with Unity <Vector2>. 
        private void CatchDualAxisState()
        {
            foreach (var kvp in m_inputManager.InputModeDualAxisMap)
            {
                if (
                    m_heldDualAxes.Contains(kvp.Key.m_xAxis) ||
                    m_heldDualAxes.Contains(kvp.Key.m_yAxis)
                )
                {
                    System.Numerics.Vector2 state = new System.Numerics.Vector2(
                        Input.GetAxisRaw(kvp.Key.m_xAxis),
                        Input.GetAxisRaw(kvp.Key.m_yAxis)
                    );

                    m_inputManager.SetDualAxisState(kvp.Value, state);
                }
            }
        }

        private void GenerateEvent(EventSubtype _type, EventFlag _flag)
        {
            GameEvent ev = new GameEvent(
                EventArchetype.Input,
                _type,
                EventPriority.Urgent,
                _flag,
                this
            );
			
            m_localEventHandler.Dispatch(ev);
        }

		private void StashEvent(EventSubtype _type, EventFlag _flag)
		{
			GameEvent ev = new GameEvent(
				EventArchetype.Input,
				_type,
				EventPriority.Urgent,
				_flag,
				this
			);
			
			m_pauseBuffer.Enqueue(ev);
		}

		public float GetAxisState(EventSubtype _subtype)
        {
            return m_inputManager.AxisStates[_subtype];
        }

        public System.Numerics.Vector2 GetDualAxisState(EventSubtype _subtype)
        {
			/*
			switch (m_inputManager.Mode)
			{
				case InputMode.Keyboard:

					break;

				case InputMode.KeyboardMouse:



					break;

				case InputMode.Controller:

					break;
			}
			*/

            return m_inputManager.DualAxisStates[_subtype];
        }

		public bool KeyHeld(EventSubtype _input)
		{
			bool held = false;

			foreach (var pair in m_inputManager.InputModeKeyMap)
			{
				if (
					pair.Value == _input &&
					m_heldKeys.Contains(pair.Key)
				)
				{
					held = true;
					break;
				}
			}

			return held;
		}

		public bool AxisHeld(EventSubtype _input)
		{
			bool held = false;

			foreach (var pair in m_inputManager.InputModeAxisMap)
			{
				if (
					pair.Value == _input &&
					m_heldAxes.Contains(pair.Key)
				)
				{
					held = true;
					break;
				}
			}

			return held;
		}

		public bool DualAxisHeld(EventSubtype _input)
		{
			bool held = false;

			foreach (var pair in m_inputManager.InputModeDualAxisMap)
			{
				if (
					pair.Value == _input && (
						m_heldDualAxes.Contains(pair.Key.m_xAxis) ||
						m_heldDualAxes.Contains(pair.Key.m_yAxis)
					)
				)
				{
					held = true;
					break;
				}
			}

			return held;
		}

		private void ReleaseHeld()
		{
			foreach (var pair in m_inputManager.InputModeKeyMap)
			{
				if (m_heldKeys.Contains(pair.Key)				)
				{
					StashEvent(pair.Value, EventFlag.KeyUp);
				}
			}

			foreach (var pair in m_inputManager.InputModeAxisMap)
			{
				if (m_heldAxes.Contains(pair.Key))
				{
					StashEvent(pair.Value, EventFlag.KeyUp);
				}
			}

			foreach (var pair in m_inputManager.InputModeDualAxisMap)
			{
				if (
						m_heldDualAxes.Contains(pair.Key.m_xAxis) ||
						m_heldDualAxes.Contains(pair.Key.m_yAxis)
					)
				{
					StashEvent(pair.Value, EventFlag.KeyUp);
				}
			}

			m_heldKeys.Clear();
			m_heldAxes.Clear();
			m_heldDualAxes.Clear();
		}
	}
}

