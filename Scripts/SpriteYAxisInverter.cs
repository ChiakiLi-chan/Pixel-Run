using UnityEngine;

public class SpriteYAxisInverter : MonoBehaviour
{
    public Movement movement;
    public SpriteRenderer targetRenderer; 
    void Start()
    {
        movement = FindObjectOfType<Movement>();

        if (movement == null)
            Debug.LogError("Movement component not found in the scene.");
    }

    void Update()
    {
        if (movement != null && targetRenderer != null)
        {
            if (movement.Gravity == -1)
            {
                targetRenderer.transform.localScale = new Vector3(
                    targetRenderer.transform.localScale.x,
                    -Mathf.Abs(targetRenderer.transform.localScale.y),
                    targetRenderer.transform.localScale.z
                );
            }
            else
            {
                targetRenderer.transform.localScale = new Vector3(
                    targetRenderer.transform.localScale.x,
                    Mathf.Abs(targetRenderer.transform.localScale.y),
                    targetRenderer.transform.localScale.z
                );
            }
        }
        else
            Debug.LogWarning("Movement or target SpriteRenderer is not assigned.");
    }
}