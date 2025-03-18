using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using GSP.Mediator;
using GSP.Events;
using GSP.InputHandling;
using UnityEditor.Experimental.GraphView;
using System.Diagnostics.CodeAnalysis;
using Unity.VisualScripting;

public class PlayerComponent : MonoBehaviour
{
    private MediatorComponentInterface m_mediator;

    private InputManagerComponentInterface m_inputManager;

    private LocalEventHandlerComponentInterface m_handler;

    //begin trash tech demo sshtuff --- 
    private bool m_move;

    public string model;
    private Transform playerModelTransform;
    private CharacterController controller;


    public float acceleration;
    public float deceleration;
    public float maxSpeed;
    public float maxRotSpeed;
    public float rotDamping;
    public float dampingThreshold;

    private Vector3 velocity = Vector3.zero;
    private Quaternion targetRot;

    //-- even more bs values --



    // --- end shtuff

    // Start is called before the first frame update
    void Awake()
    {
        m_mediator = MediatorComponent.Instance;
        m_mediator.SetObject(MediatedObject.Player, this);

        m_handler = new LocalEventHandlerComponent();


        //demo stuff
        controller = GetComponent<CharacterController>();
        playerModelTransform = transform.Find(model);
    }
    void Start()
    {
        m_inputManager = m_mediator.GetObject(MediatedObject.InputManager, this) as InputManagerComponent;

        m_handler.Subscribe(EventArchetype.Input);

        //tech demo trash stuff ---
        //Physics.IgnoreCollision(controller, lanternController);
        //--end
    }

    // Update is called once per frame
    void Update()
    {
        if (m_handler.HasEvents())
        {
            GameEvent ev = m_handler.Dequeue();

            if (ev.m_type == EventArchetype.Input)
            {
                if (ev.m_subtype == EventSubtype.Move)
                {
                    if (ev.m_flag == EventFlag.KeyDown)
                    {
                        m_move = true;
                    }
                    else if (ev.m_flag == EventFlag.KeyUp)
                    {
                        m_move = false;
                    }
                }
            }
        }

        if (m_move)
        {
            System.Numerics.Vector2 axisState = m_inputManager.GetDualAxisState(EventSubtype.Move);

            Vector3 rawDirection = new Vector3(-axisState.X, 0.0f, axisState.Y);

            Quaternion rotation = Quaternion.Euler(0.0f, -45.0f, 0.0f);

            Vector3 offsetDirection = rotation * rawDirection;

            velocity += offsetDirection * acceleration * Time.deltaTime;
            velocity = Vector3.ClampMagnitude(velocity, maxSpeed);

            //clamp on y plane
            velocity.y = 0.0f;

            targetRot = Quaternion.LookRotation(velocity);
        }
        else if (!m_move)
        {
            velocity = Vector3.Lerp(velocity, Vector3.zero, deceleration * Time.deltaTime);
        }

        /*
        Vector3 direction = Vector3.zero;

        if (Input.GetKey(KeyCode.W))
        {
            direction += new Vector3(-1, 0, 1);
        }
        if (Input.GetKey(KeyCode.A))
        {
            direction += new Vector3(-1, 0, -1);
        }
        if (Input.GetKey(KeyCode.S))
        {
            direction += new Vector3(1, 0, -1);
        }
        if (Input.GetKey(KeyCode.D))
        {
            direction += new Vector3(1, 0, 1);
        }

        if (direction != Vector3.zero)
        {
            
            velocity += direction * acceleration * Time.deltaTime;
            velocity = Vector3.ClampMagnitude(velocity, maxSpeed);

            //clamp on y plane
            velocity.y = 0.0f;

            targetRot = Quaternion.LookRotation(velocity);
        }
        else
        {
            velocity = Vector3.Lerp(velocity, Vector3.zero, deceleration * Time.deltaTime);
        }
        */

        if (velocity != Vector3.zero)
        {
            if (playerModelTransform != null)
            {
                Quaternion currentRot = playerModelTransform.rotation;

                // Apply damping to smooth out the final rotation
                if (Quaternion.Angle(currentRot, targetRot) < dampingThreshold)
                {
                    playerModelTransform.rotation = Quaternion.Slerp(currentRot, targetRot, rotDamping);
                }
                else
                {
                    playerModelTransform.rotation = Quaternion.RotateTowards(currentRot, targetRot, maxRotSpeed * Time.deltaTime);
                    //playerModelTransform.rotation = Quaternion.Slerp(playerModelTransform.rotation, targetRotation, maxRotSpeed * Time.deltaTime);
                }
            }
        }

        controller.Move(velocity * Time.deltaTime);
        //rb.MovePosition(rb.position + velocity * Time.deltaTime);
        //transform.Translate(velocity * Time.deltaTime, Space.World);
    }

    private void OnDestroy()
    {
        //m_mediator.RemoveObject();
    }
}
