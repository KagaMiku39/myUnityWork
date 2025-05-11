/*************************************************************************************

 * ������: www.gamedev3d.com

 *��1����վ������Ϊ������Ϸ��ҵ���ṩ��ص���Դ�ز�����Ѷ��

 *��2����վ��������¸�����ص���Դ�زģ�Ϊ��Ϸ���򿪷����ṩ���õ���Ѷ����У�

 *��3����վ������Դ�زĽ���ѧϰ�ο�������������ҵ��;�����������غ��24Сʱ�ڽ���ɾ����

 *     �����ɴ������ķ��ɾ��׼��������α�վ�ͷ����߸Ų��е���
 
*************************************************************************************/
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ThrottleStick))]
public class ThrottleStickEditor : InputEditor
{
    private bool m_InputFoldOut = true;
    private bool m_ParametersFoldOut = true;

    SerializedProperty ThrottleAxis;
    SerializedProperty MaxDeflectionDegrees;

    private void OnEnable()
    {
        ThrottleAxis = serializedObject.FindProperty("ThrottleAxis");
        MaxDeflectionDegrees = serializedObject.FindProperty("MaxDeflectionDegrees");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        //Show input dialogs for yoke.
        m_InputFoldOut = EditorGUILayout.Foldout(m_InputFoldOut, "Input");
        if (m_InputFoldOut == true)
        {
            SerializedProperty Controller = serializedObject.FindProperty("Controller");

            ShowInputAxisOptions("Throttle", Controller);

            EditorGUILayout.Space();
        }

        m_ParametersFoldOut = EditorGUILayout.Foldout(m_ParametersFoldOut, "Parameters");
        if (m_ParametersFoldOut == true)
        {
            EditorGUILayout.PropertyField(ThrottleAxis, new GUIContent("Throttle Axis"));
            EditorGUILayout.PropertyField(MaxDeflectionDegrees, new GUIContent("Max Deflection Degrees"));
        }

        serializedObject.ApplyModifiedProperties();
    }
}
