using UnityEngine;
using TMPro;

public class AttemptCounter : MonoBehaviour
{
    public TextMeshProUGUI targetText; 
    public int attempts; 

    void Start()
    {
        attempts = 1;
        UpdateAttemptText();
    }

    public void IncrementAttempts()
    {
        attempts++;
        UpdateAttemptText();
    }

    private void UpdateAttemptText()
    {
        if (targetText != null)
            targetText.text = "Attempt " + attempts;
        else
            Debug.LogError("AttemptText TextMeshProUGUI component is not assigned.");
    }
}
