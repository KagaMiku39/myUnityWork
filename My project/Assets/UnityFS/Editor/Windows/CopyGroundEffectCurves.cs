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

public class CopyGroundEffectCurves : EditorWindow
{

    private AnimationCurve m_ClipboardCLCurve = null;
    private AnimationCurve m_ClipboardCDCurve = null;

    // Add menu named "My Window" to the Window menu
    [MenuItem("Window/UnityFS/Copy Ground Effect Curves")]

    static void Init()
    {
        // Get existing open window or if none, make a new one:
        EditorWindow.GetWindow(typeof(CopyGroundEffectCurves));
    }

    void OnGUI()
    {
        //Copy button.
        if (GUI.Button(new Rect(10, 10, position.width - 20, 40), "Copy"))
        {
            if (Selection.activeGameObject != null)
            {
                GroundEffect groundEffect = Selection.activeGameObject.GetComponent<GroundEffect>();
                m_ClipboardCLCurve = groundEffect.CLHeightVsChord;
                m_ClipboardCDCurve = groundEffect.CDHeightVsSpan;
            }
        }

        //Import button.
        if (GUI.Button(new Rect(10, 60, position.width - 20, 40), "Paste"))
        {
            if (Selection.activeGameObject != null)
            {
                GroundEffect groundEffect = Selection.activeGameObject.GetComponent<GroundEffect>();
                groundEffect.CLHeightVsChord = new AnimationCurve(m_ClipboardCLCurve.keys);
                groundEffect.CDHeightVsSpan = new AnimationCurve(m_ClipboardCDCurve.keys);
            }
        }
    }
}