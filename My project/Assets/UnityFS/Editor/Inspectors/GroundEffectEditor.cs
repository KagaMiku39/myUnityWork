/*************************************************************************************

 * ������: www.gamedev3d.com

 *��1����վ������Ϊ������Ϸ��ҵ���ṩ��ص���Դ�ز�����Ѷ��

 *��2����վ��������¸�����ص���Դ�زģ�Ϊ��Ϸ���򿪷����ṩ���õ���Ѷ����У�

 *��3����վ������Դ�زĽ���ѧϰ�ο�������������ҵ��;�����������غ��24Сʱ�ڽ���ɾ����

 *     �����ɴ������ķ��ɾ��׼��������α�վ�ͷ����߸Ų��е���
 
*************************************************************************************/
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GroundEffect))]
public class GroundEffectEditor : Editor
{
    SerializedProperty CLHeightVsChord;
    SerializedProperty CDHeightVsSpan;

    SerializedProperty RayCastAxis;
    SerializedProperty RayCastLayers;

    SerializedProperty Wingspan;

    private void OnEnable()
    {
        CLHeightVsChord = serializedObject.FindProperty("CLHeightVsChord");
        CDHeightVsSpan = serializedObject.FindProperty("CDHeightVsSpan");

        RayCastAxis = serializedObject.FindProperty("RayCastAxis");
        RayCastLayers = serializedObject.FindProperty("RayCastLayers");

        Wingspan = serializedObject.FindProperty("Wingspan");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        //Add default keyframes.
        if (CLHeightVsChord.animationCurveValue.keys.Length == 0)
        {
            CLHeightVsChord.animationCurveValue.AddKey(0.0f, 1.6f);
            CLHeightVsChord.animationCurveValue.AddKey(0.4f, 1.2f);
            CLHeightVsChord.animationCurveValue.AddKey(1.0f, 1.0f);
        }

        if (CDHeightVsSpan.animationCurveValue.keys.Length == 0)
        {
            CDHeightVsSpan.animationCurveValue.AddKey(0.0f, 0.2f);
            CDHeightVsSpan.animationCurveValue.AddKey(0.4f, 0.8f);
            CDHeightVsSpan.animationCurveValue.AddKey(1.0f, 1.0f);
        }

        EditorGUILayout.PropertyField(CLHeightVsChord, new GUIContent("CL Height Vs Chord"));
        EditorGUILayout.PropertyField(CDHeightVsSpan, new GUIContent("CD Height Vs Span"));
        EditorGUILayout.Space();
        EditorGUILayout.PropertyField(RayCastAxis, new GUIContent("RayCast Axis"));
        EditorGUILayout.PropertyField(RayCastLayers, new GUIContent("RayCast Layers"));
        EditorGUILayout.Space();
        EditorGUILayout.PropertyField(Wingspan, new GUIContent("Wing Span"));

        serializedObject.ApplyModifiedProperties();
    }

}
