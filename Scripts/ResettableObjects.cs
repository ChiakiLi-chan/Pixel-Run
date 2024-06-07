using TMPro;
using UnityEngine;

public class ResettableObject : MonoBehaviour
{
    private Vector3 initialPosition;
    private Quaternion initialRotation;
    private Vector3 initialScale;
    private Color initialColor;

    private SpriteRenderer spriteRenderer;
    private TextMeshProUGUI textMeshPro;

    void Start()
    {
        SaveInitialState();
    }

    public void SaveInitialState()
    {
        initialPosition = transform.position;
        initialRotation = transform.rotation;
        initialScale = transform.localScale;

        spriteRenderer = GetComponent<SpriteRenderer>();
        textMeshPro = GetComponent<TextMeshProUGUI>();

        if (spriteRenderer != null)
            initialColor = spriteRenderer.color;
        else if (textMeshPro != null)
            initialColor = textMeshPro.color;
    }

    public void ResetToInitialState()
    {
        transform.position = initialPosition;
        transform.rotation = initialRotation;
        transform.localScale = initialScale;

        if (spriteRenderer != null)
            spriteRenderer.color = initialColor;
        else if (textMeshPro != null)
            textMeshPro.color = initialColor;
    }
}
