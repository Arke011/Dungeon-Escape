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
            // Calculate the desired position of the camera
            Vector3 desiredPosition = target.position + offset;

            // Smoothly move the camera towards the desired position
            transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothness);
        }
    }
}
