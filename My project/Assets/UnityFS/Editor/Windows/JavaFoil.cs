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