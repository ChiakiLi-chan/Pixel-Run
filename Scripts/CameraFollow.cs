using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // The target the camera will follow
    public Vector3 offset; // Offset from the target
    public float smoothSpeed = 0.125f; // Smoothness of the camera movement

    void LateUpdate()
    {
        if (target == null)
        {
            Debug.LogWarning("Target not assigned to CameraFollow script.");
            return;
        }

        if (!target.GetComponent<Movement>().MapComplete && !target.GetComponent<Movement>().isDead) // Check if MapComplete is false
        {
            // Desired position of the camera
            Vector3 desiredPosition = target.position + offset;
            // Smoothly interpolate between the camera's current position and the desired position
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
            // Update the camera's position
            transform.position = smoothedPosition;
        }
    
    }
}

