/*************************************************************************************

 * ������: www.gamedev3d.com

 *��1����վ������Ϊ������Ϸ��ҵ���ṩ��ص���Դ�ز�����Ѷ��

 *��2����վ��������¸�����ص���Դ�زģ�Ϊ��Ϸ���򿪷����ṩ���õ���Ѷ����У�

 *��3����վ������Դ�زĽ���ѧϰ�ο�������������ҵ��;�����������غ��24Сʱ�ڽ���ɾ����

 *     �����ɴ������ķ��ɾ��׼��������α�վ�ͷ����߸Ų��е���
 
*************************************************************************************/
using UnityEngine;
using UnityEditor;
using System.IO;

public class JavaFoil : EditorWindow
{
    private Vector2 m_ScrollPos = Vector2.zero;

    // Add menu
    [MenuItem("Window/UnityFS/JavaFoil")]
    static void Init()
    {
        // Get existing open window or if none, make a new one:
        EditorWindow.GetWindow(typeof(JavaFoil));
    }

    void OnGUI()
    {
        EditorGUILayout.BeginHorizontal();
        m_ScrollPos = EditorGUILayout.BeginScrollView(m_ScrollPos, GUILayout.Width(position.width), GUILayout.Height(100));
        string text = "Click the launch button to run JavaFoil and create your own aerofoils. " +
                        "This link is provided meerly as a matter of convenience. " +
                        "UnityFS is in no way affiliated with JavaFoil and accepts no liability should you choose to use it. " +
                        "For more information about JavaFoil visit http://www.mh-aerotools.de/airfoils/javafoil.htm";

        text = GUILayout.TextArea(text, GUILayout.Height(position.height - 30));
        EditorGUILayout.EndScrollView();
        EditorGUILayout.EndHorizontal();

        if (GUILayout.Button("Launch"))
        {
            Application.OpenURL("http://www.mh-aerotools.de/airfoils/jf_applet.htm");
        }
    }
}