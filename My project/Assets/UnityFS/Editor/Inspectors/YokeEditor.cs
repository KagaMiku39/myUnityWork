/*************************************************************************************

 * 游研堂: www.gamedev3d.com

 *（1）本站致力于为广大的游戏从业者提供相关的资源素材与资讯。

 *（2）本站会持续更新更多相关的资源素材，为游戏领域开发者提供更好的资讯与灵感！

 *（3）本站所有资源素材仅供学习参考，切勿用作商业用途，并请在下载后的24小时内进行删除，

 *     否则由此引发的法律纠纷及连带责任本站和发布者概不承担。
 
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
