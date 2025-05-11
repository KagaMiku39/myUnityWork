/*************************************************************************************

 * 游研堂: www.gamedev3d.com

 *（1）本站致力于为广大的游戏从业者提供相关的资源素材与资讯。

 *（2）本站会持续更新更多相关的资源素材，为游戏领域开发者提供更好的资讯与灵感！

 *（3）本站所有资源素材仅供学习参考，切勿用作商业用途，并请在下载后的24小时内进行删除，

 *     否则由此引发的法律纠纷及连带责任本站和发布者概不承担。
 
*************************************************************************************/
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ControlSurface))]
public class ControlSurfaceEditor : InputEditor
{
    private bool m_InputFoldOut = true;
    private bool m_ParametersFoldOut = true;
    private bool m_VisualsFoldOut = true;
    private bool m_AffectedSectionsFoldOut = true;

    SerializedProperty Controller;
    SerializedProperty MaxDeflectionDegrees;
    SerializedProperty RootHingeDistanceFromTrailingEdge;
    SerializedProperty TipHingeDistanceFromTrailingEdge;
    SerializedProperty AffectedSections;
    SerializedProperty Model;
    SerializedProperty ModelRotationAxis;
    SerializedProperty InputCurve;


    private void OnEnable()
    {
        Controller = serializedObject.FindProperty("Controller");
        MaxDeflectionDegrees = serializedObject.FindProperty("MaxDeflectionDegrees");
        RootHingeDistanceFromTrailingEdge = serializedObject.FindProperty("RootHingeDistanceFromTrailingEdge");
        TipHingeDistanceFromTrailingEdge = serializedObject.FindProperty("TipHingeDistanceFromTrailingEdge");
        AffectedSections = serializedObject.FindProperty("AffectedSections");
        Model = serializedObject.FindProperty("Model");
        ModelRotationAxis = serializedObject.FindProperty("ModelRotationAxis");
        InputCurve = serializedObject.FindProperty("InputCurve");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        m_InputFoldOut = EditorGUILayout.Foldout(m_InputFoldOut, "Input");
        if (m_InputFoldOut == true)
        {
            //Call base class to draw some stuff..
            ShowInputAxisOptions("Surface", Controller);

            EditorGUILayout.PropertyField(InputCurve, new GUIContent("Input Curve"));

            //If input curve has no keyframes make some to ease user.
            if (InputCurve.animationCurveValue.keys.Length == 0)
            {
                InputCurve.animationCurveValue.AddKey(0.0f, 0.0f);
                InputCurve.animationCurveValue.AddKey(1.0f, 1.0f);
            }

        }

        EditorGUILayout.Space();

        m_ParametersFoldOut = EditorGUILayout.Foldout(m_ParametersFoldOut, "Parameters");
        if (m_ParametersFoldOut == true)
        {
            EditorGUILayout.PropertyField(MaxDeflectionDegrees, new GUIContent("Max Deflection Degrees"));

            int rootPercent = (int)(RootHingeDistanceFromTrailingEdge.floatValue * 100.0f);
            rootPercent = EditorGUILayout.IntSlider("Root hinge offset", rootPercent, 0, 100);
            RootHingeDistanceFromTrailingEdge.floatValue = (float)rootPercent / 100.0f;

            int tipPercent = (int)(TipHingeDistanceFromTrailingEdge.floatValue * 100.0f);
            tipPercent = EditorGUILayout.IntSlider("Tip hinge offset", tipPercent, 0, 100);
            TipHingeDistanceFromTrailingEdge.floatValue = (float)tipPercent / 100.0f;

            m_AffectedSectionsFoldOut = EditorGUILayout.Foldout(m_AffectedSectionsFoldOut, "Affected Sections (Root outwards)");
            if (m_AffectedSectionsFoldOut == true)
            {
                for (int i = 0; i < AffectedSections.arraySize; i++)
                {
                    SerializedProperty affectedSection = AffectedSections.GetArrayElementAtIndex(i);

                    affectedSection.boolValue = EditorGUILayout.Toggle(i.ToString(), affectedSection.boolValue);
                }
            }

        }

        EditorGUILayout.Space();

        m_VisualsFoldOut = EditorGUILayout.Foldout(m_VisualsFoldOut, "Visuals");
        if (m_VisualsFoldOut == true)
        {
            EditorGUILayout.PropertyField(Model, new GUIContent("Model"));

            if (Model.objectReferenceValue != null)
            {
                EditorGUILayout.PropertyField(ModelRotationAxis, new GUIContent("Model Rotation Axis"));
            }
        }

        //User help....
        SerializedProperty AxisName = Controller.FindPropertyRelative("AxisName");
        if (AxisName.arraySize == 0)
        {
            EditorGUILayout.HelpBox("No input axis defined.", MessageType.Error);
        }

        if (Model.objectReferenceValue == null)
        {
            EditorGUILayout.HelpBox("No model attached for visual rotation.", MessageType.Warning);
        }

        serializedObject.ApplyModifiedProperties();
    }
}
