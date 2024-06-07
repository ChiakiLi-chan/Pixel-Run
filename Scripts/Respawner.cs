using UnityEngine;

public class Respawner : MonoBehaviour
{
    public Transform player; // Reference to the player object

    void Update()
    {
        
    }

    public void RespawnPlayer()
    {
        if (player != null)
        {
            // Set the player's position to the respawn point's position
            player.position = transform.position;
        }
        else
        {
            Debug.LogWarning("Player reference is missing!");
        }
    }
}
