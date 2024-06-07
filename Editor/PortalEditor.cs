using UnityEditor;
using UnityEngine;
 
[CustomEditor(typeof(PortalScript)), CanEditMultipleObjects]
public class PortalEditor : Editor
{
    public enum DisplayCategory
    {
        Gravity, PortalValue
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
                    categoryToDisplay = DisplayCategory.Gravity;
                    break;
                case 1:
                    categoryToDisplay = DisplayCategory.PortalValue;
                    break;
            }
        }
        else
            categoryToDisplay = (DisplayCategory)EditorGUILayout.EnumPopup("Display", categoryToDisplay);
 
        EditorGUILayout.Space();
 
        switch (categoryToDisplay)
        {
            case DisplayCategory.Gravity:
                DisplayProperty("Gravity", 0);
                break;

            case DisplayCategory.PortalValue:
                DisplayProperty("PortalValue", 1);
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