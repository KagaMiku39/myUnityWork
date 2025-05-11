/*************************************************************************************

 * ������: www.gamedev3d.com

 *��1����վ������Ϊ������Ϸ��ҵ���ṩ��ص���Դ�ز�����Ѷ��

 *��2����վ��������¸�����ص���Դ�زģ�Ϊ��Ϸ���򿪷����ṩ���õ���Ѷ����У�

 *��3����վ������Դ�زĽ���ѧϰ�ο�������������ҵ��;�����������غ��24Сʱ�ڽ���ɾ����

 *     �����ɴ������ķ��ɾ��׼��������α�վ�ͷ����߸Ų��е���
 
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
