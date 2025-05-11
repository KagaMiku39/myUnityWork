
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Aerofoil))]
public class AerofoilEditor : Editor
{
    SerializedProperty CL;
    SerializedProperty CD;
    SerializedProperty CM;

    private void OnEnable()
    {
        CL = serializedObject.FindProperty("CL");
        CD = serializedObject.FindProperty("CD");
        CM = serializedObject.FindProperty("CM");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(CL, new GUIContent("CL"));
        EditorGUILayout.PropertyField(CD, new GUIContent("CD"));
        EditorGUILayout.PropertyField(CM, new GUIContent("CM"));

        serializedObject.ApplyModifiedProperties();

    }
}
