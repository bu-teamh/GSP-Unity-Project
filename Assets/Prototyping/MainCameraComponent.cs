using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using GSP.Mediator;

public class MainCameraComponent : MonoBehaviour
{
    private MediatorComponent m_mediator;
    private MainCameraTargetComponent m_cameraTarget;

    //public float distFromTarget;
    public float heightMultiplier;
    public float closestDist;
    public float farthestDist;
    public float lantMinDist;
    public float lantMaxDist;
    public float smoothPosSpeed;
    public float smoothDistSpeed;

    private float currentCamDist;

    // Start is called before the first frame update
    void Awake()
    {
        m_mediator = FindFirstObjectByType<MediatorComponent>();
    }

    void Start()
    {
        m_cameraTarget = m_mediator.GetObject(MediatedObject.CameraTarget, this) as MainCameraTargetComponent;
    }

    // Update is called once per frame
    void Update()
    {
        float playerLanternDist = GetPercentDist();

        float rangeDist = farthestDist - closestDist;

        float targetCamDist = (rangeDist * playerLanternDist) + closestDist;

        currentCamDist = Mathf.Lerp(currentCamDist, targetCamDist, smoothDistSpeed * Time.deltaTime);

        Vector3 targetCoords = m_cameraTarget.transform.position;

        Vector3 targetPos = new Vector3(
            targetCoords.x + currentCamDist,
            targetCoords.y + currentCamDist * heightMultiplier,
            targetCoords.z - currentCamDist
            );

        transform.position = Vector3.Lerp(transform.position, targetPos, smoothPosSpeed * Time.deltaTime);

        Vector3 direction = m_cameraTarget.transform.position - transform.position;

        direction.Normalize();

        Quaternion rotation = Quaternion.LookRotation( direction );

        transform.rotation = rotation;
    }

    public float GetPercentDist()
    {
        // Calculate the distance between the player and the lantern
        // private cam_target m_cameraTarget;
        //m_cameraTarget = m_mediator.GetObject(MediatedObject.CameraTarget, this) as cam_target;
        float distance = m_cameraTarget.GetSubjectDistance();

        // Clamp the distance within the minimum and maximum distance range
        if (distance < lantMinDist)
        {
            distance = lantMinDist;
        }
        else if (distance > lantMaxDist)
        {
            distance = lantMaxDist;
        }

        // Normalize the distance to a range between 0 and 1
        distance -= lantMinDist;
        distance /= (lantMaxDist - lantMinDist);

        return distance;
    }
}
