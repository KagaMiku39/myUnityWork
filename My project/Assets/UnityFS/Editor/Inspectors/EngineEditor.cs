/*************************************************************************************

 * ������: www.gamedev3d.com

 *��1����վ������Ϊ������Ϸ��ҵ���ṩ��ص���Դ�ز�����Ѷ��

 *��2����վ��������¸�����ص���Դ�زģ�Ϊ��Ϸ���򿪷����ṩ���õ���Ѷ����У�

 *��3����վ������Դ�زĽ���ѧϰ�ο�������������ҵ��;�����������غ��24Сʱ�ڽ���ɾ����

 *     �����ɴ������ķ��ɾ��׼��������α�վ�ͷ����߸Ų��е���
 
*************************************************************************************/
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Engine))]
public class EngineEditor : InputEditor
{
    private bool m_InputFoldOut = true;
    private bool m_PropFoldOut = true;
    private bool m_EngineFoldOut = true;
    private bool m_AudioFoldOut = true;

    SerializedProperty AnimatedPropellerPivot;
    SerializedProperty AnimatedPropellerPivotRotateAxis;
    SerializedProperty SlowPropeller;
    SerializedProperty FastPropeller;
    SerializedProperty RPMToUseFastProp;

    SerializedProperty CurrentEngineState;
    SerializedProperty IdleRPM;
    SerializedProperty MaxRPM;
    SerializedProperty ForceAtMaxRPM;
    SerializedProperty PercentageForceAppliedVSAirspeedKTS;
    SerializedProperty RPMToAddPerKTOfSpeed;
    SerializedProperty RPMLerpSpeed;

    SerializedProperty EngineStartClip;
    SerializedProperty EngineRunClip;
    SerializedProperty PitchAtIdleRPM;
    SerializedProperty PitchAtMaxRPM;

    private void OnEnable()
    {
        AnimatedPropellerPivot = serializedObject.FindProperty("AnimatedPropellerPivot");
        AnimatedPropellerPivotRotateAxis = serializedObject.FindProperty("AnimatedPropellerPivotRotateAxis");
        SlowPropeller = serializedObject.FindProperty("SlowPropeller");
        FastPropeller = serializedObject.FindProperty("FastPropeller");
        RPMToUseFastProp = serializedObject.FindProperty("RPMToUseFastProp");

        CurrentEngineState = serializedObject.FindProperty("CurrentEngineState");
        IdleRPM = serializedObject.FindProperty("IdleRPM");
        MaxRPM = serializedObject.FindProperty("MaxRPM");
        ForceAtMaxRPM = serializedObject.FindProperty("ForceAtMaxRPM");
        PercentageForceAppliedVSAirspeedKTS = serializedObject.FindProperty("PercentageForceAppliedVSAirspeedKTS");
        RPMToAddPerKTOfSpeed = serializedObject.FindProperty("RPMToAddPerKTOfSpeed");
        RPMLerpSpeed = serializedObject.FindProperty("RPMLerpSpeed");

        EngineStartClip = serializedObject.FindProperty("EngineStartClip");
        EngineRunClip = serializedObject.FindProperty("EngineRunClip");
        PitchAtIdleRPM = serializedObject.FindProperty("PitchAtIdleRPM");
        PitchAtMaxRPM = serializedObject.FindProperty("PitchAtMaxRPM");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        //Show input dialogs for yoke.
        m_InputFoldOut = EditorGUILayout.Foldout(m_InputFoldOut, "Input");
        if (m_InputFoldOut == true)
        {
            SerializedProperty ThrottleController = serializedObject.FindProperty("ThrottleController");
            //Call base class to draw some stuff..
            ShowInputAxisOptions("Throttle", ThrottleController);

            EditorGUILayout.Space();

            SerializedProperty EngineStartController = serializedObject.FindProperty("EngineStartController");
            ShowInputButtonOptions("Engine", EngineStartController);

            EditorGUILayout.Space();
        }

        m_PropFoldOut = EditorGUILayout.Foldout(m_PropFoldOut, "Propeller");
        if (m_PropFoldOut == true)
        {
            EditorGUILayout.PropertyField(AnimatedPropellerPivot, new GUIContent("Animated Pivot"));
            EditorGUILayout.PropertyField(AnimatedPropellerPivotRotateAxis, new GUIContent("Rotate Axis"));
            EditorGUILayout.PropertyField(SlowPropeller, new GUIContent("Slow Propeller"));
            EditorGUILayout.PropertyField(FastPropeller, new GUIContent("Fast Propeller"));
            EditorGUILayout.PropertyField(RPMToUseFastProp, new GUIContent("RPM To Switch To Fast Prop"));

            EditorGUILayout.Space();
        }

        m_EngineFoldOut = EditorGUILayout.Foldout(m_EngineFoldOut, "Engine");
        if (m_EngineFoldOut == true)
        {
            EditorGUILayout.PropertyField(CurrentEngineState, new GUIContent("Current Engine State"));
            EditorGUILayout.PropertyField(IdleRPM, new GUIContent("Idle RPM"));
            EditorGUILayout.PropertyField(MaxRPM, new GUIContent("Max RPM"));
            EditorGUILayout.PropertyField(ForceAtMaxRPM, new GUIContent("Force At Max RPM"));
            EditorGUILayout.PropertyField(PercentageForceAppliedVSAirspeedKTS, new GUIContent("Percentage Force Applied VS Airspeed KTS"));
            EditorGUILayout.PropertyField(RPMToAddPerKTOfSpeed, new GUIContent("RPM To Add Per Knot Of Speed"));
            EditorGUILayout.PropertyField(RPMLerpSpeed, new GUIContent("RPM Lerp Speed"));

            EditorGUILayout.Space();
        }

        m_AudioFoldOut = EditorGUILayout.Foldout(m_AudioFoldOut, "Audio");
        if (m_AudioFoldOut == true)
        {
            EditorGUILayout.PropertyField(EngineStartClip, new GUIContent("Engine Start Clip"));
            EditorGUILayout.PropertyField(EngineRunClip, new GUIContent("Engine Run Clip"));
            EditorGUILayout.PropertyField(PitchAtIdleRPM, new GUIContent("Pitch At Idle RPM"));
            EditorGUILayout.PropertyField(PitchAtMaxRPM, new GUIContent("Pitch At Max RPM"));
          
            EditorGUILayout.Space();
        }

        serializedObject.ApplyModifiedProperties();
    }
}
