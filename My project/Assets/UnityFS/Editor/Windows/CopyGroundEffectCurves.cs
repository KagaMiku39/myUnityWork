/*************************************************************************************

 * 游研堂: www.gamedev3d.com

 *（1）本站致力于为广大的游戏从业者提供相关的资源素材与资讯。

 *（2）本站会持续更新更多相关的资源素材，为游戏领域开发者提供更好的资讯与灵感！

 *（3）本站所有资源素材仅供学习参考，切勿用作商业用途，并请在下载后的24小时内进行删除，

 *     否则由此引发的法律纠纷及连带责任本站和发布者概不承担。
 
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