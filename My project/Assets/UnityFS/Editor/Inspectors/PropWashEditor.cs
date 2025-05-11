/*************************************************************************************

 * ������: www.gamedev3d.com

 *��1����վ������Ϊ������Ϸ��ҵ���ṩ��ص���Դ�ز�����Ѷ��

 *��2����վ��������¸�����ص���Դ�زģ�Ϊ��Ϸ���򿪷����ṩ���õ���Ѷ����У�

 *��3����վ������Դ�زĽ���ѧϰ�ο�������������ҵ��;�����������غ��24Сʱ�ڽ���ɾ����

 *     �����ɴ������ķ��ɾ��׼��������α�վ�ͷ����߸Ų��е���
 
*************************************************************************************/
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PropWash))]
public class PropWashEditor : Editor
{
    private bool m_AffectedSectionsFoldOut = true;

    SerializedProperty AffectedSections;

    SerializedProperty PropWashSource;
    SerializedProperty PropWashStrength;

    private void OnEnable()
    {
        AffectedSections = serializedObject.FindProperty("AffectedSections");

        PropWashSource = serializedObject.FindProperty("PropWashSource");
        PropWashStrength = serializedObject.FindProperty("PropWashStrength");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(PropWashSource, new GUIContent("Propwash Source"));
        EditorGUILayout.PropertyField(PropWashStrength, new GUIContent("Strength Multiplier"));

        m_AffectedSectionsFoldOut = EditorGUILayout.Foldout(m_AffectedSectionsFoldOut, "Affected Sections (Root outwards)");
        if (m_AffectedSectionsFoldOut == true)
        {
            for (int i = 0; i < AffectedSections.arraySize; i++)
            {
                SerializedProperty affectedSection = AffectedSections.GetArrayElementAtIndex(i);
                
                affectedSection.boolValue = EditorGUILayout.Toggle(i.ToString(), affectedSection.boolValue);
            }
        }

        EditorGUILayout.Space();

        if (PropWashSource.objectReferenceValue == null)
        {
            EditorGUILayout.HelpBox("No prop wash source. This will do nothing!", MessageType.Error);
        }

        serializedObject.ApplyModifiedProperties();
    }
}
