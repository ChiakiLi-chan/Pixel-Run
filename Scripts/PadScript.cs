using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class PadScript : MonoBehaviour
{
    public PadType PadType;
    public bool gravity;
    public int State;
    public Vector3 newDimensions = new Vector3(0.9604774f, 0.1360047f, 1f);  // New dimensions for the object

    void OnValidate()
    {
        ApplyNewDimensions();
    }

    void Start()
    {
        ApplyNewDimensions();
    }

    public void initiatePad(Movement movement)
    {
        movement.InteractionBetweenPad(PadType, gravity ? 1 : -1, State, transform.position.y);
    }

    void ApplyNewDimensions()
    {
        Transform objectTransform = transform;
        Renderer objectRenderer = GetComponent<Renderer>();

        if (objectRenderer != null)
        {
            Vector3 currentSize = objectRenderer.bounds.size;
            Vector3 newScale = new Vector3(0.9604774f, 0.1360047f, 1f);
            objectTransform.localScale = newScale;
        }
        else
            Debug.LogWarning("Renderer component is missing. Cannot resize the object.");
    }
}
