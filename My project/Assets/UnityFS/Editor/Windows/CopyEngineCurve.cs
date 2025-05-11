/*************************************************************************************

 * 游研堂: www.gamedev3d.com

 *（1）本站致力于为广大的游戏从业者提供相关的资源素材与资讯。

 *（2）本站会持续更新更多相关的资源素材，为游戏领域开发者提供更好的资讯与灵感！

 *（3）本站所有资源素材仅供学习参考，切勿用作商业用途，并请在下载后的24小时内进行删除，

 *     否则由此引发的法律纠纷及连带责任本站和发布者概不承担。
 
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