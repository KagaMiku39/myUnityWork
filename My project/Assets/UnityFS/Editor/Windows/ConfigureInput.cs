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

public class ConfigureInput : EditorWindow
{
    private Vector2 m_ScrollPos = Vector2.zero;
    private bool m_InputOverwritten = false;
    private bool m_InputOverwriteFailed = false;

    // Add menu
    [MenuItem("Window/UnityFS/** Configure Input **")]
    static void Init()
    {
        // Get existing open window or if none, make a new one:
        EditorWindow.GetWindow(typeof(ConfigureInput));
    }

    void OnGUI()
    {
        EditorGUILayout.BeginHorizontal();
        m_ScrollPos = EditorGUILayout.BeginScrollView(m_ScrollPos, GUILayout.Width(position.width), GUILayout.Height(200));
        string text = "Warning: The following tool will overwrite your default input project settings with UnityFS axis. " +
                        "If you wish to keep the default settings you can add the UnityFS axis manually or " +
                        "alternatively configure aircraft to use your own custom defined Input." +
                        "\n \nThe following axis will be created: \n\nPitch, Roll, Yaw, Throttle, Landing Gear, Change View, Camera Zoom, Brake, Engine";

        text = GUILayout.TextArea(text, GUILayout.Height(position.height - 30));
        EditorGUILayout.EndScrollView();
        EditorGUILayout.EndHorizontal();

        if (m_InputOverwritten == false)
        {
            if (GUILayout.Button("OVERWRITE"))
            {
                if (OverwriteInput() == true)
                {
                    m_InputOverwritten = true;
                }
                else
                {
                    m_InputOverwriteFailed = true;
                }
            }
        }
        else if (m_InputOverwriteFailed == true)
        {
            if (GUILayout.Button("Overwrite failed."))
            {
                AssetDatabase.Refresh();
            }
        }
        else
        {
            if (GUILayout.Button("Done.") == true)
            {
                AssetDatabase.Refresh();
            }
        }
    }

    bool OverwriteInput()
    {
        string sourcePath = Application.dataPath + "/UnityFS/Data/input.dat";
        string destPath = Application.dataPath + "/../ProjectSettings/InputManager.asset";

        if (File.Exists(sourcePath) == false)
        {
            Debug.LogError("Configure Input - Overwrite error. Could not find source file.");
            return false;
        }

        if (File.Exists(destPath) == false)
        {
            Debug.LogError("Configure Input - Overwrite error. Could not find dest file.");
            return false;
        }

        File.Copy(sourcePath, destPath, true);
        AssetDatabase.Refresh();

        return true;
    }
}