using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using GSP.Mediator;

public class CompanionComponent : MonoBehaviour
{
    private MediatorComponentInterface m_mediator;
    private PlayerComponent m_player;


    public string model;
    private Transform lanternModelTransform;
    private CharacterController controller;
    //public CharacterController playerController;
    public LayerMask groundLayer;

    public float maxDist;
    public float minDist;
    public float maxSpeed;
    public float maxRotSpeed;
    public float accel;
    public float decel;
    public float repelAccelMultplr;
    public float dampingThreshold;
    public float rotDamping;
    public float mouseAccel;
    public float mouseDecel;
    public float mouseFollowSpeed;
    public float mouseDecelThreshold;
    public float hovHeight;

    private Vector3 velocity = Vector3.zero;
    private Quaternion targetRot;
    private Vector3 lastMousePos;
    //private Vector3 aimVector;

    private bool isGrabbed = false;

    // Start is called before the first frame update
    void Awake()
    {
		m_mediator = MediatorComponent.Instance;
		m_mediator.SetObject(MediatedObject.Companion, this);
    }

    void Start()
    {
        //TO DO: explicit cast: (GameObject)m_mediator.GetObject(MediatedObject.Player, this); then wrap that in try/catch for error handling (will return InvalidCastException if not gameobject)
        m_player = m_mediator.GetObject(MediatedObject.Player, this) as PlayerComponent;

        controller = GetComponent<CharacterController>();
        lanternModelTransform = transform.Find(model);

        //Physics.IgnoreCollision(controller, playerController);

        Vector3 startPos = new Vector3(0, hovHeight, 0);
        transform.position += startPos;
    }

    // Update is called once per frame
    void Update()
    {
        // Check if the left mouse button is pressed
        if (Input.GetMouseButton(0))
        {
            // Create a ray from the camera to the mouse position
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // Perform the raycast
            if (Physics.Raycast(ray, out hit))
            {
                // Check if the hit object is the same as this GameObject
                if (hit.collider.gameObject == gameObject)
                {
                    isGrabbed = true;

                    lastMousePos = Input.mousePosition;
                }
            }
        }
        else
        {
            isGrabbed = false;
        }

        if (isGrabbed)
        {
            // Cast a ray from the camera to the mouse position
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit groundHit;

            // Perform the raycast to find the intersection with the ground
            if (Physics.Raycast(ray, out groundHit, Mathf.Infinity, groundLayer))
            {
                if (lastMousePos != Input.mousePosition)
                {
                    // Intersection point with the ground
                    Vector3 intersectionPoint = groundHit.point;

                    // Calculate the direction vector from the intersection point to the camera
                    Vector3 direction = (Camera.main.transform.position - intersectionPoint).normalized;

                    // Calculate the desired distance along the direction vector
                    float desiredDistance = (hovHeight - intersectionPoint.y) / direction.y;

                    // Calculate the new position along the direction vector at the desired height
                    Vector3 newPosition = intersectionPoint + direction * desiredDistance;

                    float distance = Vector3.Distance(transform.position, newPosition);

                    Vector3 moveDirection = (newPosition - transform.position).normalized;

                    if (distance < mouseDecelThreshold)
                    {
                        velocity = Vector3.zero;
                        //velocity = Vector3.Lerp(velocity, Vector3.zero, mouseDecel * Time.deltaTime);
                    }
                    else
                    {
                        velocity = moveDirection * mouseFollowSpeed;
                        //velocity = Vector3.ClampMagnitude(velocity, maxMouseFollowSpeed);
                    }
                }

                else
                {
                    velocity = Vector3.zero;
                }

                // Update the object's position, keeping the initial height
                //transform.position = new Vector3(newPosition.x, hovHeight, newPosition.z);
                //Debug.Log("Object moved to position: " + transform.position);
            }
        }
        /*
        if (isGrabbed)
        {
            // Create a new ray for detecting the intersection with the ground
            Ray groundRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit groundHit;

            // Perform the raycast to find the intersection with the ground, ignoring all other GameObjects
            if (Physics.Raycast(groundRay, out groundHit, Mathf.Infinity, groundLayer))
            {
                float distance = Vector3.Distance(transform.position, groundHit.point);

                Vector3 direction = (groundHit.point - transform.position).normalized;

                velocity += direction * mouseAccel * Time.deltaTime;
                velocity = Vector3.ClampMagnitude(velocity, maxMouseFollowSpeed);
            }
        }
        */
        else
        {
            float distance = Vector3.Distance(transform.position, m_player.transform.position);

            Vector3 direction = (m_player.transform.position - transform.position).normalized;

            if (distance > maxDist)
            {
                velocity += direction * accel * Time.deltaTime;
                velocity = Vector3.ClampMagnitude(velocity, maxSpeed);
            }
            else if (distance < minDist)
            {
                velocity -= direction * (accel * repelAccelMultplr) * Time.deltaTime;
                velocity = Vector3.ClampMagnitude(velocity, maxSpeed);
            }
            else
            {
                velocity = Vector3.Lerp(velocity, Vector3.zero, decel * Time.deltaTime);
            }
        }

        //clamp on y plane
        //currently clamped to zero but at some point must consider factoring height from player height not ground height (otherwise on tall shit the lantern might clamp to ground if floats over edge)
        velocity.y = 0.0f;

        float vely = (hovHeight - transform.position.y) * accel * Time.deltaTime;

        vely = Mathf.Clamp(vely, -maxSpeed, maxSpeed);

        velocity.y = vely;

        controller.Move(velocity * Time.deltaTime);

        //rb.MovePosition(rb.position + velocity * Time.deltaTime);
        //transform.Translate(velocity * Time.deltaTime, Space.World);

        // Rotate the object to face the direction it's traveling
        /*
        if (velocity != Vector3.zero)
        {
            Quaternion targetRot = Quaternion.LookRotation(velocity);
            lanternModelTransform.rotation = Quaternion.Slerp(lanternModelTransform.rotation, targetRot, Time.deltaTime * maxRotSpeed);

        }
        */

        if (velocity != Vector3.zero)
        {
            if (lanternModelTransform != null)
            {
                // Calculate the target rotation based on the direction
                targetRot = Quaternion.LookRotation(velocity);

                // Extract the y-component of the target rotation
                targetRot = Quaternion.Euler(0, targetRot.eulerAngles.y, 0);

                Quaternion currentRot = lanternModelTransform.rotation;

                // Apply damping to smooth out the final rotation
                if (Quaternion.Angle(currentRot, targetRot) < dampingThreshold)
                {
                    lanternModelTransform.rotation = Quaternion.Slerp(currentRot, targetRot, rotDamping);
                }
                else
                {
                    lanternModelTransform.rotation = Quaternion.RotateTowards(currentRot, targetRot, maxRotSpeed * Time.deltaTime);
                    //playerModelTransform.rotation = Quaternion.Slerp(playerModelTransform.rotation, targetRotation, maxRotSpeed * Time.deltaTime);
                }
            }
        }

        /*
        if (velocity != Vector3.zero)
        {
            if (lanternModelTransform != null)
            {
                targetRot = Quaternion.LookRotation(velocity);

                Quaternion currentRot = lanternModelTransform.rotation;

                // Apply damping to smooth out the final rotation
                if (Quaternion.Angle(currentRot, targetRot) < dampingThreshold)
                {
                    lanternModelTransform.rotation = Quaternion.Slerp(currentRot, targetRot, rotDamping);
                }
                else
                {
                    lanternModelTransform.rotation = Quaternion.RotateTowards(currentRot, targetRot, maxRotSpeed * Time.deltaTime);
                    //playerModelTransform.rotation = Quaternion.Slerp(playerModelTransform.rotation, targetRotation, maxRotSpeed * Time.deltaTime);
                }
            }
        }
        */

    }

}
