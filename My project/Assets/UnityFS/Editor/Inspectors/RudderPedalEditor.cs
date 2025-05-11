/*************************************************************************************

 * ������: www.gamedev3d.com

 *��1����վ������Ϊ������Ϸ��ҵ���ṩ��ص���Դ�ز�����Ѷ��

 *��2����վ��������¸�����ص���Դ�زģ�Ϊ��Ϸ���򿪷����ṩ���õ���Ѷ����У�

 *��3����վ������Դ�زĽ���ѧϰ�ο�������������ҵ��;�����������غ��24Сʱ�ڽ���ɾ����

 *     �����ɴ������ķ��ɾ��׼��������α�վ�ͷ����߸Ų��е���
 
*************************************************************************************/
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(RudderPedal))]
public class RudderPedalEditor : InputEditor
{
    private bool m_InputFoldOut = true;
    private bool m_ParametersFoldOut = true;

    SerializedProperty TranslateAxis;
    SerializedProperty DeflectionMeters;

    private void OnEnable()
    {
        TranslateAxis = serializedObject.FindProperty("TranslateAxis");
        DeflectionMeters = serializedObject.FindProperty("DeflectionMeters");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        //Show input dialogs for yoke.
        m_InputFoldOut = EditorGUILayout.Foldout(m_InputFoldOut, "Input");
        if (m_InputFoldOut == true)
        {
            SerializedProperty Controller = serializedObject.FindProperty("Controller");
            //Call base class to draw some stuff..
            ShowInputAxisOptions("Yaw", Controller);

            EditorGUILayout.Space();
        }

        m_ParametersFoldOut = EditorGUILayout.Foldout(m_ParametersFoldOut, "Parameters");
        if (m_ParametersFoldOut == true)
        {
            EditorGUILayout.PropertyField(TranslateAxis, new GUIContent("Translate Axis"));
            EditorGUILayout.PropertyField(DeflectionMeters, new GUIContent("Deflection Meters"));
        }

        serializedObject.ApplyModifiedProperties();
    }
}
