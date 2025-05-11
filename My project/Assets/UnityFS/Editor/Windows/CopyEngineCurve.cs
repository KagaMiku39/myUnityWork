/*************************************************************************************

 * ������: www.gamedev3d.com

 *��1����վ������Ϊ������Ϸ��ҵ���ṩ��ص���Դ�ز�����Ѷ��

 *��2����վ��������¸�����ص���Դ�زģ�Ϊ��Ϸ���򿪷����ṩ���õ���Ѷ����У�

 *��3����վ������Դ�زĽ���ѧϰ�ο�������������ҵ��;�����������غ��24Сʱ�ڽ���ɾ����

 *     �����ɴ������ķ��ɾ��׼��������α�վ�ͷ����߸Ų��е���
 
*************************************************************************************/
using UnityEngine;
using UnityEditor;

public class CopyEngineCurve : EditorWindow
{
    private AnimationCurve m_ClipboardCurve = null;

    // Add menu named "My Window" to the Window menu
    [MenuItem("Window/UnityFS/Copy Engine Curve")]

    static void Init()
    {
        // Get existing open window or if none, make a new one:
        EditorWindow.GetWindow(typeof(CopyEngineCurve));
    }

    void OnGUI()
    {
        //Copy button.
        if (GUI.Button(new Rect(10, 10, position.width - 20, 40), "Copy"))
        {
            if (Selection.activeGameObject != null)
            {
                Engine engine = Selection.activeGameObject.GetComponent<Engine>();
                m_ClipboardCurve = engine.PercentageForceAppliedVSAirspeedKTS;
            }
        }

        //Import button.
        if (GUI.Button(new Rect(10, 60, position.width - 20, 40), "Paste"))
        {
            if (Selection.activeGameObject != null)
            {
                Engine engine = Selection.activeGameObject.GetComponent<Engine>();
                engine.PercentageForceAppliedVSAirspeedKTS = new AnimationCurve(m_ClipboardCurve.keys);
            }
        }
    }
}