using UnityEngine;
using TMPro; 
public class TextColorChange : MonoBehaviour
{
    public TextMeshProUGUI targetText; 
    public GameObject triggerObject; 
    public Color targetColor = Color.white; 

    void OnTriggerEnter2D(Collider2D other)
{
    
    if (other.gameObject == triggerObject)
    {   
        if (targetText != null)
            targetText.color = targetColor;
    }
}

}
