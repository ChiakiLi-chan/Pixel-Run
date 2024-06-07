using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class PortalSprites : MonoBehaviour
{
    public Sprite[] portalSprites;
    private SpriteRenderer spriteRenderer;
    private PortalScript portalScript;

    void Awake()
    {
        // Get the SpriteRenderer component
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            Debug.LogError("No SpriteRenderer found on the GameObject.");
            return;
        }

        // Get the PortalScript component
        portalScript = GetComponent<PortalScript>();
        if (portalScript == null)
        {
            Debug.LogError("No PortalScript found on the GameObject.");
            return;
        }

        // Update the sprite based on the current PortalValue
        UpdateSprite();
    }

    public void UpdateSprite()
    {
        // Check if the PortalValue is within the range of available sprites
        if ((int)portalScript.PortalValue >= 0 && (int)portalScript.PortalValue < portalSprites.Length)
        {
            spriteRenderer.sprite = portalSprites[(int)portalScript.PortalValue];
        }
        else
        {
            Debug.LogWarning("PortalValue is out of range of the sprites array.");
        }
    }
}
