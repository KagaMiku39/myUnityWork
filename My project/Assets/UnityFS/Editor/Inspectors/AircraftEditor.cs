
using UnityEditor;
using UnityEngine;
using System.IO;

[CustomEditor(typeof(Aircraft))]
public class AircraftEditor : InputEditor
{
    private bool m_InputFoldOut = true;

    SerializedProperty AircraftEnabledAtStart;
    SerializedProperty OverrideInertiaTensor;
    SerializedProperty InertiaTensor;
    SerializedProperty RollwiseDamping;

    private void OnEnable()
    {
        AircraftEnabledAtStart = serializedObject.FindProperty("AircraftEnabledAtStart");
        OverrideInertiaTensor = serializedObject.FindProperty("OverrideInertiaTensor");
        InertiaTensor = serializedObject.FindProperty("InertiaTensor");
        RollwiseDamping = serializedObject.FindProperty("RollwiseDamping");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        //Show input dialogs for camera change.
        m_InputFoldOut = EditorGUILayout.Foldout(m_InputFoldOut, "Input");
        if (m_InputFoldOut == true)
        {
            SerializedProperty ChangeCameraController = serializedObject.FindProperty("ChangeCameraController");
            ShowInputButtonOptions("Change Camera", ChangeCameraController);
            EditorGUILayout.Space();
        }

        EditorGUILayout.PropertyField(AircraftEnabledAtStart, new GUIContent("Aircraft Enabled At Start"));
        EditorGUILayout.PropertyField(OverrideInertiaTensor, new GUIContent("Override Inertia Tensor"));
        EditorGUILayout.PropertyField(InertiaTensor, new GUIContent("Inertia Tensor"));
        EditorGUILayout.PropertyField(RollwiseDamping, new GUIContent("Roll Wise Damping"));

        serializedObject.ApplyModifiedProperties();
    }

    public void OnSceneGUI()
    {
        CopyGizmos();
    }

    private void CopyGizmos()
    {
        string source = Application.dataPath + "/UnityFS/Data/";
        string dest = Application.dataPath + "/Gizmos/";
        string[] files = System.IO.Directory.GetFiles(source, "*.png");

        if (Directory.Exists(dest) == false)
        {
            Directory.CreateDirectory(dest);
        }

        for (int i = 0;i < files.Length;i++)
        {
            string sourceFilename = files[i];
            string destFilename = sourceFilename.Replace(source, dest);
            if (File.Exists(destFilename) == false)
            {
                File.Copy(sourceFilename, destFilename);
            }
        }
    }
}
