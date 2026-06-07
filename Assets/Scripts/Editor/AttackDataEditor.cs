using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(AttackData))]
[CanEditMultipleObjects]
public class AttackDataEditor : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(serializedObject.FindProperty("attackDuration"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("damage"));

        EditorGUILayout.Space();

        var shapeProp = serializedObject.FindProperty("shape");
        EditorGUILayout.PropertyField(shapeProp);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("hitboxOffset"));

        bool isBox      = shapeProp.enumNames[shapeProp.enumValueIndex] == "Box";
        bool isTriangle = shapeProp.enumNames[shapeProp.enumValueIndex] == "Triangle";

        if (isBox)
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("hitboxSize"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("hitboxRotation"));
        }
        else if (isTriangle)
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("trianglePoints"));
        }
        else
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("hitboxRadius"));
        }

        EditorGUILayout.Space();
        EditorGUILayout.PropertyField(serializedObject.FindProperty("gizmoColor"));

        serializedObject.ApplyModifiedProperties();
    }
}
