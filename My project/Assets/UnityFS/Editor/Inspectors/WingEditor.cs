/*************************************************************************************

 * ������: www.gamedev3d.com

 *��1����վ������Ϊ������Ϸ��ҵ���ṩ��ص���Դ�ز�����Ѷ��

 *��2����վ��������¸�����ص���Դ�زģ�Ϊ��Ϸ���򿪷����ṩ���õ���Ѷ����У�

 *��3����վ������Դ�زĽ���ѧϰ�ο�������������ҵ��;�����������غ��24Сʱ�ڽ���ɾ����

 *     �����ɴ������ķ��ɾ��׼��������α�վ�ͷ����߸Ų��е���
 
*************************************************************************************/
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Wing))]
public class WingEditor : Editor
{
    SerializedProperty Aerofoil;
    SerializedProperty SectionCount;
    SerializedProperty WingTipWidthZeroToOne;
    SerializedProperty WingTipSweep;
    SerializedProperty WingTipAngle;
    SerializedProperty CDOverride;

    private void OnEnable()
    {
        Aerofoil = serializedObject.FindProperty("Aerofoil");
        SectionCount = serializedObject.FindProperty("SectionCount");
        WingTipWidthZeroToOne = serializedObject.FindProperty("WingTipWidthZeroToOne");
        WingTipSweep = serializedObject.FindProperty("WingTipSweep");
        WingTipAngle = serializedObject.FindProperty("WingTipAngle");
        CDOverride = serializedObject.FindProperty("CDOverride");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        SectionCount.intValue = EditorGUILayout.IntSlider("Section Count", SectionCount.intValue, 1, 10);

        EditorGUILayout.Space();

        float widthF = WingTipWidthZeroToOne.floatValue * 100.0f;
        int widthPercent = (int)widthF;
        widthPercent = EditorGUILayout.IntSlider("Wing Tip width", widthPercent, 0, 100);
        WingTipWidthZeroToOne.floatValue = ((float)widthPercent) / 100.0f;

        float sweepF = WingTipSweep.floatValue * 100.0f;
        int wingTipSweep = (int)sweepF;
        wingTipSweep = EditorGUILayout.IntSlider("Wing Tip Sweep", wingTipSweep, -1000, 1000);
        WingTipSweep.floatValue = (float)wingTipSweep / 100.0f;

        float angleF = WingTipAngle.floatValue;
        int wingTipAngle = (int)angleF;
        wingTipAngle = EditorGUILayout.IntSlider("Wing Tip Angle", wingTipAngle, -90, 90);
        WingTipAngle.floatValue = (float)wingTipAngle;

        EditorGUILayout.Space();

        EditorGUILayout.PropertyField(Aerofoil, new GUIContent("Aerofoil"));

        if (Aerofoil.objectReferenceValue == null)
        {
            EditorGUILayout.Space();

            EditorGUILayout.HelpBox("No aerofoil selected using basic lift drag equations.", MessageType.Warning);

            EditorGUILayout.PropertyField(CDOverride, new GUIContent("CD Override"));
        }

        serializedObject.ApplyModifiedProperties();
    }
}