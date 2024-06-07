using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PulsatingObjects : MonoBehaviour
{
    public List<Transform> targetObjects;  // List of objects you want to pulsate
    public float scaleMultiplier = 5f;     // Multiplier for the pulsating effect
    public float smoothSpeed = 10f;        // Smooth speed for the scaling
    public int spectrumDataSize = 64;      // Size of the spectrum data array

    private AudioSource audioSource;
    private float[] spectrumData;
    private List<Vector3> originalScales;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        spectrumData = new float[spectrumDataSize];
        originalScales = new List<Vector3>();

        // Store the original scale of each target object
        foreach (var target in targetObjects)
        {
            if (target != null)
            {
                originalScales.Add(target.localScale);
            }
            else
            {
                originalScales.Add(Vector3.one);
                Debug.LogWarning("One of the target objects is null. Defaulting to Vector3.one for its scale.");
            }
        }
    }

    void Update()
    {
        // Get spectrum data from the audio source
        audioSource.GetSpectrumData(spectrumData, 0, FFTWindow.Blackman);

        // Find the peak value in the spectrum data
        float peakValue = 0f;
        for (int i = 0; i < spectrumDataSize; i++)
        {
            if (spectrumData[i] > peakValue)
            {
                peakValue = spectrumData[i];
            }
        }

        // Amplify the peak value to make the pulsation more obvious
        peakValue *= scaleMultiplier;

        // Smoothly interpolate the scale for each target object
        for (int i = 0; i < targetObjects.Count; i++)
        {
            Transform target = targetObjects[i];
            if (target != null)
            {
                Vector3 originalScale = originalScales[i];
                Vector3 targetScaleVector = new Vector3(originalScale.x + peakValue, originalScale.y + peakValue, originalScale.z);
                target.localScale = Vector3.Lerp(target.localScale, targetScaleVector, Time.deltaTime * smoothSpeed);
            }
        }
    }
}
