using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ProgressBar : MonoBehaviour
{
    public float initialPosition; 
    private float finalPosition; 
    private TextMeshProUGUI percentageText; 
    public Slider slider;
    public Transform player; 
    
    void Start()
    {
        slider = GetComponent<Slider>();
        GameObject mapCompleteObject = GameObject.FindGameObjectWithTag("MapComplete");

        if (mapCompleteObject != null)
            finalPosition = mapCompleteObject.transform.position.x;
        else
            Debug.LogError("GameObject with the tag 'MapComplete' not found.");

        if (player != null)
            initialPosition = player.position.x;
        else
            Debug.LogError("Player object is not assigned.");

        percentageText = GameObject.FindWithTag("AttemptPercentage")?.GetComponent<TextMeshProUGUI>();

        if (percentageText == null)
            Debug.LogError("TextMeshProUGUI component not found in the scene.");
    }

    void Update()
    {
        if (player != null)
        {
            float progress = Mathf.Clamp01((player.position.x - initialPosition) / (finalPosition - initialPosition));
            slider.value = progress;
            UpdatePercentageText(progress);
        }
        else
            Debug.LogError("Player object is not assigned.");
    }

    void UpdatePercentageText(float progress)
    {
        if (percentageText != null)
        {
            int percentage = Mathf.RoundToInt(progress * 100);
            percentageText.text = percentage + "%";
        }
    }
}
