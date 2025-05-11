/*************************************************************************************

 * 游研堂: www.gamedev3d.com

 *（1）本站致力于为广大的游戏从业者提供相关的资源素材与资讯。

 *（2）本站会持续更新更多相关的资源素材，为游戏领域开发者提供更好的资讯与灵感！

 *（3）本站所有资源素材仅供学习参考，切勿用作商业用途，并请在下载后的24小时内进行删除，

 *     否则由此引发的法律纠纷及连带责任本站和发布者概不承担。
 
*************************************************************************************/
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(WheelBrake))]
public class WheelBrakeEditor : InputEditor
{
    private bool m_InputFoldOut = true;

    SerializedProperty wheelModel;
    SerializedProperty wheelRotationAxis;
    SerializedProperty brakeTorque;

    SerializedProperty wheelRollClip;
    SerializedProperty rpmForMaxVolume;
    SerializedProperty maxVolume;

    private void OnEnable()
    {
        wheelModel = serializedObject.FindProperty("wheelModel");
        wheelRotationAxis = serializedObject.FindProperty("wheelRotationAxis");
        brakeTorque = serializedObject.FindProperty("brakeTorque");

        wheelRollClip = serializedObject.FindProperty("wheelRollClip");
        rpmForMaxVolume = serializedObject.FindProperty("rpmForMaxVolume");
        maxVolume = serializedObject.FindProperty("maxVolume");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        //Show input dialogs for yoke.
        m_InputFoldOut = EditorGUILayout.Foldout(m_InputFoldOut, "Input");
        if (m_InputFoldOut == true)
        {
            SerializedProperty Controller = serializedObject.FindProperty("Controller");
            ShowInputButtonOptions("Brake Input", Controller);

            EditorGUILayout.Space();
        }

        EditorGUILayout.PropertyField(wheelModel, new GUIContent("Wheel Model"));
        EditorGUILayout.PropertyField(wheelRotationAxis, new GUIContent("Wheel Rotation Axis"));
        EditorGUILayout.PropertyField(brakeTorque, new GUIContent("Brake Torque"));

        EditorGUILayout.Space();

        EditorGUILayout.PropertyField(wheelRollClip, new GUIContent("Wheel Roll Clip"));
        EditorGUILayout.PropertyField(rpmForMaxVolume, new GUIContent("Rpm For Max Volume"));
        EditorGUILayout.PropertyField(maxVolume, new GUIContent("Max Volume"));

        serializedObject.ApplyModifiedProperties();
    }
}
