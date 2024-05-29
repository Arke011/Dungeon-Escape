using UnityEngine;

public class FollowCam : MonoBehaviour
{
    public Transform target;
    public Vector3 offset;
    [Range(0.01f, 1f)] public float smoothness;

    private void LateUpdate()
    {
        if (target != null)
        {
            Vector3 desiredPosition = target.position + offset;

            float smoothedSpeed = smoothness * Time.deltaTime;
            Vector3 smoothedPosition = transform.position; // Current position

            // Lerp x position
            smoothedPosition.x = Mathf.Lerp(transform.position.x, desiredPosition.x, smoothedSpeed);

            // Instantly follow y position
            smoothedPosition.y = desiredPosition.y;

            // Apply Z position offset
            smoothedPosition.z = desiredPosition.z;

            // Apply the smoothed position to the camera
            transform.position = smoothedPosition;
        }
    }
}
