using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using GSP.Mediator;

public class MainCameraTargetComponent : MonoBehaviour
{

    private MediatorComponent m_mediator;

    private PlayerComponent m_player;
    private CompanionComponent m_companion;
    //public float maxSpeed;
    //public float smoothSpeed;

    public int playerWeight;
    public int lanternWeight;

    private float m_subjectDistance;

    // Start is called before the first frame update
    void Awake()
    {
        m_mediator = FindFirstObjectByType<MediatorComponent>();
        m_mediator.SetObject(MediatedObject.CameraTarget, this);
    }

    void Start()
    {
        //TO DO: explicit cast: (GameObject)m_mediator.GetObject(MediatedObject.Player, this); then wrap that in try/catch for error handling (will return InvalidCastException if not correct gameobject)
        m_player = m_mediator.GetObject(MediatedObject.Player, this) as PlayerComponent;
        m_companion = m_mediator.GetObject(MediatedObject.Companion, this) as CompanionComponent;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 playerPos = m_player.transform.position;
        Vector3 lanternPos = m_companion.transform.position;
        m_subjectDistance = Vector3.Distance(playerPos, lanternPos);

        //Vector3 newTargetPosition
        transform.position = new Vector3(
            (((playerWeight * playerPos.x) + (lanternWeight * lanternPos.x)) / (playerWeight + lanternWeight)),
            (((playerWeight * playerPos.y) + (lanternWeight * lanternPos.y)) / (playerWeight + lanternWeight)),
            (((playerWeight * playerPos.z) + (lanternWeight * lanternPos.z)) / (playerWeight + lanternWeight))
            );

        // Smoothly interpolate towards the target position using Lerp
        //Vector3 targetPosition = Vector3.Lerp(transform.position, newTargetPosition, smoothSpeed * Time.deltaTime);

        // Calculate the maximum distance the cameratarget can move this frame
        //float maxDistance = maxSpeed * Time.deltaTime;


        // Move the cameratarget towards the target position at a controlled speed
        //transform.position = Vector3.MoveTowards(transform.position, targetPosition, maxDistance);
    }

    public float GetSubjectDistance() { return m_subjectDistance; }
}

