using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField]
    private Transform targetTransform;

    private float zOffset;
    private Vector3 cameraVelocity;
    [SerializeField]
    private float smoothTime = 0.2f;

    [SerializeField]
    private float leftLimit;
    [SerializeField]
    private float rightLimit;
    [SerializeField]
    private float topLimit;
    [SerializeField]
    private float bottomLimit;


    private void Awake()
    {
        zOffset = transform.position.z;
    }

    private void LateUpdate()
    {
        if (targetTransform != null)
        {
            Vector3 targetPosition = targetTransform.position;
            targetPosition.z = zOffset;


            Vector3 newPosition = new Vector3
            (
                Mathf.Clamp(targetPosition.x, leftLimit, rightLimit),
                Mathf.Clamp(targetPosition.y, bottomLimit, topLimit),
                targetPosition.z
            );

            transform.position = Vector3.SmoothDamp(
                transform.position,
                newPosition,
                ref cameraVelocity,
                smoothTime
                );

        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        // top line
        Gizmos.DrawLine(new Vector2(leftLimit, topLimit), new Vector2(rightLimit, topLimit));
        // right line
        Gizmos.DrawLine(new Vector2(rightLimit, topLimit), new Vector2(rightLimit , bottomLimit));
        // bottom line
        Gizmos.DrawLine(new Vector2(leftLimit, bottomLimit), new Vector2(rightLimit, bottomLimit));
        // left line
        Gizmos.DrawLine(new Vector2(leftLimit,bottomLimit), new Vector2(leftLimit, topLimit));
    }

}