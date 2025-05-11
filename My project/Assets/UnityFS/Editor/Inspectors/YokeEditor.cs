/*************************************************************************************

 * ������: www.gamedev3d.com

 *��1����վ������Ϊ������Ϸ��ҵ���ṩ��ص���Դ�ز�����Ѷ��

 *��2����վ��������¸�����ص���Դ�زģ�Ϊ��Ϸ���򿪷����ṩ���õ���Ѷ����У�

 *��3����վ������Դ�زĽ���ѧϰ�ο�������������ҵ��;�����������غ��24Сʱ�ڽ���ɾ����

 *     �����ɴ������ķ��ɾ��׼��������α�վ�ͷ����߸Ų��е���
 
*************************************************************************************/
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Yoke))]
public class YokeEditor : InputEditor
{
    private bool m_InputFoldOut = true;
    private bool m_ParametersFoldOut = true;

    SerializedProperty PitchAxis;
    SerializedProperty MaxPitchTranslationMeters;

    SerializedProperty RollAxis;
    SerializedProperty MaxRollDeflectionDegrees;

    private void OnEnable()
    {
        PitchAxis = serializedObject.FindProperty("PitchAxis");
        MaxPitchTranslationMeters = serializedObject.FindProperty("MaxPitchTranslationMeters");

        RollAxis = serializedObject.FindProperty("RollAxis");
        MaxRollDeflectionDegrees = serializedObject.FindProperty("MaxRollDeflectionDegrees");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        //Show input dialogs for yoke.
        m_InputFoldOut = EditorGUILayout.Foldout(m_InputFoldOut, "Input");
        if (m_InputFoldOut == true)
        {
            SerializedProperty PitchController = serializedObject.FindProperty("PitchController");
            //Call base class to draw some stuff..
            ShowInputAxisOptions("Pitch", PitchController);

            EditorGUILayout.Space();

            ShowInputAxisOptions("Roll", PitchController);

            EditorGUILayout.Space();
        }

        m_ParametersFoldOut = EditorGUILayout.Foldout(m_ParametersFoldOut, "Parameters");
        if (m_ParametersFoldOut == true)
        {
            EditorGUILayout.PropertyField(PitchAxis, new GUIContent("Pitch Axis"));
            EditorGUILayout.PropertyField(MaxPitchTranslationMeters, new GUIContent("Max Pitch Translation Meters"));
            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(RollAxis, new GUIContent("Roll Axis"));
            EditorGUILayout.PropertyField(MaxRollDeflectionDegrees, new GUIContent("Max Roll Deflection Degrees"));
        }

        serializedObject.ApplyModifiedProperties();
    }
}
