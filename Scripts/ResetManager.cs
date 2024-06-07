using System.Collections.Generic;
using UnityEngine;

public class ResetManager : MonoBehaviour
{
    private List<ResettableObject> resettableObjects = new List<ResettableObject>();

    void Start()
    {
        FindAllResettableObjects();
    }

    void FindAllResettableObjects()
    {
        ResettableObject[] allResettableObjects = FindObjectsOfType<ResettableObject>();
        resettableObjects.AddRange(allResettableObjects);
    }

    public void ResetAllObjects()
    {
        foreach (var resettableObject in resettableObjects)
            resettableObject.ResetToInitialState();
    }

    public void ResetSelectedObject(GameObject selectedObject)
    {
        ResettableObject resettable = selectedObject.GetComponent<ResettableObject>();
        if (resettable != null)
            resettable.ResetToInitialState();
        else
            Debug.LogWarning("Selected object does not have a ResettableObject component.");
    }
}
