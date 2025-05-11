/*************************************************************************************

 * ������: www.gamedev3d.com

 *��1����վ������Ϊ������Ϸ��ҵ���ṩ��ص���Դ�ز�����Ѷ��

 *��2����վ��������¸�����ص���Դ�زģ�Ϊ��Ϸ���򿪷����ṩ���õ���Ѷ����У�

 *��3����վ������Դ�زĽ���ѧϰ�ο�������������ҵ��;�����������غ��24Сʱ�ڽ���ɾ����

 *     �����ɴ������ķ��ɾ��׼��������α�վ�ͷ����߸Ų��е���
 
*************************************************************************************/
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(WheelBrake))]
public class WheelBrakeEditor : InputEditor
{
    private bool m_InputFoldOut = true;

    SerializedProperty wheelModel;
    SerializedProperty wheelRotationAxis;
    SerializedProperty brakeTorque;

    SerializedProperty wheelRollClip;
    SerializedProperty rpmForMaxVolume;
    SerializedProperty maxVolume;

    private void OnEnable()
    {
        wheelModel = serializedObject.FindProperty("wheelModel");
        wheelRotationAxis = serializedObject.FindProperty("wheelRotationAxis");
        brakeTorque = serializedObject.FindProperty("brakeTorque");

        wheelRollClip = serializedObject.FindProperty("wheelRollClip");
        rpmForMaxVolume = serializedObject.FindProperty("rpmForMaxVolume");
        maxVolume = serializedObject.FindProperty("maxVolume");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        //Show input dialogs for yoke.
        m_InputFoldOut = EditorGUILayout.Foldout(m_InputFoldOut, "Input");
        if (m_InputFoldOut == true)
        {
            SerializedProperty Controller = serializedObject.FindProperty("Controller");
            ShowInputButtonOptions("Brake Input", Controller);

            EditorGUILayout.Space();
        }

        EditorGUILayout.PropertyField(wheelModel, new GUIContent("Wheel Model"));
        EditorGUILayout.PropertyField(wheelRotationAxis, new GUIContent("Wheel Rotation Axis"));
        EditorGUILayout.PropertyField(brakeTorque, new GUIContent("Brake Torque"));

        EditorGUILayout.Space();

        EditorGUILayout.PropertyField(wheelRollClip, new GUIContent("Wheel Roll Clip"));
        EditorGUILayout.PropertyField(rpmForMaxVolume, new GUIContent("Rpm For Max Volume"));
        EditorGUILayout.PropertyField(maxVolume, new GUIContent("Max Volume"));

        serializedObject.ApplyModifiedProperties();
    }
}
