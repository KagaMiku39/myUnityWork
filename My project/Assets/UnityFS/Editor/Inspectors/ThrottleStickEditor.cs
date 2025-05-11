/*************************************************************************************

 * 游研堂: www.gamedev3d.com

 *（1）本站致力于为广大的游戏从业者提供相关的资源素材与资讯。

 *（2）本站会持续更新更多相关的资源素材，为游戏领域开发者提供更好的资讯与灵感！

 *（3）本站所有资源素材仅供学习参考，切勿用作商业用途，并请在下载后的24小时内进行删除，

 *     否则由此引发的法律纠纷及连带责任本站和发布者概不承担。
 
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
