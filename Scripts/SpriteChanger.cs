using UnityEngine;

public class SpriteChanger : MonoBehaviour
{
    public SpriteRenderer spriteRenderer; // Reference to the Sprite Renderer component
    public Sprite[] sprites; // Array of sprites for different game modes

    void Start()
    {
        // If spriteRenderer is not set in the Inspector, try to get it from the GameObject
        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        // Initial sprite assignment (optional)
        ChangeSprite(0); // Change to the sprite at index 0 initially
    }

    public void ChangeSprite(int modeIndex)
    {
        // Check if spriteRenderer is not null and sprites array is valid
        if (spriteRenderer != null && sprites != null && modeIndex >= 0 && modeIndex < sprites.Length)
        {
            spriteRenderer.sprite = sprites[modeIndex]; // Change the sprite based on modeIndex
        }
        else
        {
            Debug.LogError("Sprite Renderer or Sprites array is not assigned or index out of range.");
        }
    }
}
