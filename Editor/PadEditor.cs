using UnityEditor;
using UnityEngine;
 
[CustomEditor(typeof(PadScript)), CanEditMultipleObjects]
public class PadEditor : Editor
{
    public enum DisplayCategory
    {
        PadType
    }
    public DisplayCategory categoryToDisplay;
 
    bool FirstTime = true;
 
    public override void OnInspectorGUI()
    {
        if (FirstTime)
        {
            switch (serializedObject.FindProperty("State").intValue)
            {
                case 0:
                    categoryToDisplay = DisplayCategory.PadType;
                    break;
            }
        }
        else
            categoryToDisplay = (DisplayCategory)EditorGUILayout.EnumPopup("Display", categoryToDisplay);
 
        EditorGUILayout.Space();
 
        switch (categoryToDisplay)
        {
            case DisplayCategory.PadType:
                DisplayProperty("PadType", 0);
                break;

        }
 
        FirstTime = false;
 
        serializedObject.ApplyModifiedProperties();
    }
 
    void DisplayProperty(string property, int PropNumb)
    {
        try
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty(property));
        }
        catch
        { }
        serializedObject.FindProperty("State").intValue = PropNumb;
    }

    
}