using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpriteColorChange : MonoBehaviour
{
    public List<SpriteRenderer> targetRenderers; 
    public GameObject triggerObject; 
    public Color targetColor;
    public float fadeDuration = 1.0f;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject == triggerObject)
        {
            foreach (var renderer in targetRenderers)
            {
                if (renderer != null)
                    StartCoroutine(FadeToColor(renderer, targetColor, fadeDuration));
            }
        }
    }

    IEnumerator FadeToColor(SpriteRenderer renderer, Color targetColor, float duration)
    {
        Color startColor = renderer.color;
        float elapsedTime = 0;

        while (elapsedTime < duration)
        {
            renderer.color = Color.Lerp(startColor, targetColor, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        renderer.color = targetColor;
    }
}
