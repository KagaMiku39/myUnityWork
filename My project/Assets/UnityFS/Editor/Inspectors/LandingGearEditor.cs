/*************************************************************************************

 * ������: www.gamedev3d.com

 *��1����վ������Ϊ������Ϸ��ҵ���ṩ��ص���Դ�ز�����Ѷ��

 *��2����վ��������¸�����ص���Դ�زģ�Ϊ��Ϸ���򿪷����ṩ���õ���Ѷ����У�

 *��3����վ������Դ�زĽ���ѧϰ�ο�������������ҵ��;�����������غ��24Сʱ�ڽ���ɾ����

 *     �����ɴ������ķ��ɾ��׼��������α�վ�ͷ����߸Ų��е���
 
*************************************************************************************/
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(LandingGear))]
public class LandingGearEditor : InputEditor
{
    private bool m_InputFoldOut = true;

    SerializedProperty ToggleLandingGearClip;
    SerializedProperty ToggleLandingGearAnimationGameObject;
    SerializedProperty ToggleLandingGearAnimationName;
    SerializedProperty GearDownDrag;
    SerializedProperty GearUpDrag;

    private void OnEnable()
    {
        ToggleLandingGearClip = serializedObject.FindProperty("ToggleLandingGearClip");
        ToggleLandingGearAnimationGameObject = serializedObject.FindProperty("ToggleLandingGearAnimationGameObject");
        ToggleLandingGearAnimationName = serializedObject.FindProperty("ToggleLandingGearAnimationName");
        GearDownDrag = serializedObject.FindProperty("GearDownDrag");
        GearUpDrag = serializedObject.FindProperty("GearUpDrag");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        //Show input dialogs for yoke.
        m_InputFoldOut = EditorGUILayout.Foldout(m_InputFoldOut, "Input");
        if (m_InputFoldOut == true)
        {
            SerializedProperty LandingGearController = serializedObject.FindProperty("LandingGearController");
            //Call base class to draw some stuff..
            ShowInputButtonOptions("Toggle Landing Gear", LandingGearController);

            EditorGUILayout.Space();
        }

        EditorGUILayout.PropertyField(ToggleLandingGearClip, new GUIContent("Toggle Landing Gear Clip"));
        EditorGUILayout.PropertyField(ToggleLandingGearAnimationGameObject, new GUIContent("Toggle Landing Gear Animation GameObject"));
        EditorGUILayout.PropertyField(ToggleLandingGearAnimationName, new GUIContent("Toggle Landing Gear Animation Name"));
        EditorGUILayout.Space();
        EditorGUILayout.PropertyField(GearDownDrag, new GUIContent("Gear Down Drag"));
        EditorGUILayout.PropertyField(GearUpDrag, new GUIContent("Gear Up Drag"));

        serializedObject.ApplyModifiedProperties();
    }
}
