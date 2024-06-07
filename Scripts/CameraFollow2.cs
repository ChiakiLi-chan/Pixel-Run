using UnityEngine;

public class CameraFollow2 : MonoBehaviour
{
    public Transform player;
    public float offsetX = 10f; // Horizontal offset from the player's position
    public float smoothSpeed = 0.125f;
    private Movement movement;
    private bool isFrozen = false; 
    
    void Start()
    {
        if (player == null)
        {
            Debug.LogError("Player not assigned to CameraFollow2 script.");
            return;
        }

        movement = player.GetComponent<Movement>();

        if (movement == null)
        {
            Debug.LogError("Movement script not found on player.");
            return;
        }
    }

    void LateUpdate()
    {
        if (isFrozen || player == null || movement == null)
        {
            return;
        }

        float speed = movement.SpeedValues[(int)movement.CurrentSpeed];
        float pan = speed * Time.fixedDeltaTime;

        // x position of the camera relative to the player
        float desiredXPosition = player.position.x + offsetX + pan;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, new Vector3(desiredXPosition, transform.position.y, transform.position.z), smoothSpeed);
        transform.position = smoothedPosition;
    }

    public void FreezeCamera()
    {
        isFrozen = true;
    }
    public void UnfreezeCamera()
    {
        isFrozen=false;
    }
}
