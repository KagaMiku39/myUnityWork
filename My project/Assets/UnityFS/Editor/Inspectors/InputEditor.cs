/*************************************************************************************

 * 游研堂: www.gamedev3d.com

 *（1）本站致力于为广大的游戏从业者提供相关的资源素材与资讯。

 *（2）本站会持续更新更多相关的资源素材，为游戏领域开发者提供更好的资讯与灵感！

 *（3）本站所有资源素材仅供学习参考，切勿用作商业用途，并请在下载后的24小时内进行删除，

 *     否则由此引发的法律纠纷及连带责任本站和发布者概不承担。
 
*************************************************************************************/
using UnityEngine;
using UnityEditor;
using System.Reflection;

//Base class for any editor that needs to edit input - provides
//functionality for selecting input source.
public class InputEditor : Editor
{
    protected void ShowInputAxisOptions(string name, SerializedProperty inputController)
    {
        SerializedProperty inputSource = inputController.FindPropertyRelative("inputSource");
        EditorGUILayout.PropertyField(inputSource, new GUIContent(string.Format("{0} Input Source", name)));

        if (inputSource.enumValueIndex == (int)InputController.InputSource.InputManager)
        {
            SerializedProperty AxisName = inputController.FindPropertyRelative("AxisName");
            AxisName.stringValue = EditorGUILayout.TextField(string.Format("{0} Axis Name", name), AxisName.stringValue);
        }
        else if(inputSource.enumValueIndex == (int)InputController.InputSource.Manual)
        {
            EditorGUILayout.HelpBox("Use SetManualInputMinusOneToOne() to set", MessageType.Info);
        }

        SerializedProperty Invert = inputController.FindPropertyRelative("Invert");
        Invert.boolValue = EditorGUILayout.Toggle(string.Format("Invert {0}", name), Invert.boolValue);
    }

    protected void ShowInputButtonOptions(string name, SerializedProperty inputController)
    {
        SerializedProperty inputSource = inputController.FindPropertyRelative("inputSource");
        EditorGUILayout.PropertyField(inputSource, new GUIContent(string.Format("{0} Input Source", name)));

        if (inputSource.enumValueIndex == (int)InputController.InputSource.InputManager)
        {
            SerializedProperty ButtonName = inputController.FindPropertyRelative("ButtonName");
            ButtonName.stringValue = EditorGUILayout.TextField(string.Format("{0} Button Name", name), ButtonName.stringValue);
        }
        else if (inputSource.enumValueIndex == (int)InputController.InputSource.Manual)
        {
            EditorGUILayout.HelpBox("Use SetManualInputButtonPressed() to set", MessageType.Info);
        }

        SerializedProperty Invert = inputController.FindPropertyRelative("Invert");
        Invert.boolValue = EditorGUILayout.Toggle(string.Format("Invert {0}", name), Invert.boolValue);

    }

}
