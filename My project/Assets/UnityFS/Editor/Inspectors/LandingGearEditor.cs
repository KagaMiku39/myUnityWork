/*************************************************************************************

 * 游研堂: www.gamedev3d.com

 *（1）本站致力于为广大的游戏从业者提供相关的资源素材与资讯。

 *（2）本站会持续更新更多相关的资源素材，为游戏领域开发者提供更好的资讯与灵感！

 *（3）本站所有资源素材仅供学习参考，切勿用作商业用途，并请在下载后的24小时内进行删除，

 *     否则由此引发的法律纠纷及连带责任本站和发布者概不承担。
 
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
