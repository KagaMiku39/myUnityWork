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

public class ImportAerofoil : EditorWindow
{
    private AnimationCurve m_CL = AnimationCurve.Linear(-180.0f, 0.0f, 180, 0);
    private AnimationCurve m_CD = AnimationCurve.Linear(-180.0f, 0.0f, 180, 0);
    private AnimationCurve m_CM = AnimationCurve.Linear(-180.0f, 0.0f, 180, 0);
    private Aerofoil m_Aerofoil;
    private string m_FileName = "";
    private bool m_Rename = true;

    // Add menu named "My Window" to the Window menu
    [MenuItem("Window/UnityFS/Import Aerofoil")]
    static void Init()
    {
        // Get existing open window or if none, make a new one:
        EditorWindow.GetWindow(typeof(ImportAerofoil));
    }

    void OnGUI()
    {
        //Import button.
        if (GUI.Button(new Rect(10, 10, position.width - 20, 20), "Import .afl"))
        {
            string path = EditorUtility.OpenFilePanel("Aerofoil", "", "afl");
            if (path.Length != 0)
            {
                //Store file name.
                m_FileName = Path.GetFileNameWithoutExtension(path);

                //Reset curves.
                m_CL = new AnimationCurve();
                m_CD = new AnimationCurve();
                m_CM = new AnimationCurve();

                //Create a stream reader.
                StreamReader file = new StreamReader(path);
                string text = "";

                //Skip past the header.
                while (text.Contains("alpha") == false)
                {
                    text = file.ReadLine();
                }

                //Read in the data.
                while (text != null)
                {
                    text = file.ReadLine();
                    if (text != null)
                    {
                        string[] parts = text.Split(null);

                        string[] reducedParts = new string[4];
                        int j = 0;
                        for (int i = 0; i < parts.Length; i++)
                        {
                            if (parts[i].Length > 0)
                            {
                                reducedParts[j] = parts[i];
                                j++;
                            }
                        }

                        float aoa = 0.0f;
                        float.TryParse(reducedParts[0], out aoa);

                        if ((int)aoa == 180)
                        {
                            return;
                        }

                        float cl = 0.0f;
                        float.TryParse(reducedParts[1], out cl);

                        float cd = 0.0f;
                        float.TryParse(reducedParts[2], out cd);

                        float cm = 0.0f;
                        float.TryParse(reducedParts[3], out cm);

                        //Assign value to curves.
                        m_CL.AddKey(aoa, cl);
                        m_CD.AddKey(aoa, cd);
                        m_CM.AddKey(aoa, cm);

                    }
                }
            }
        }

        //Curves.
        m_CL = EditorGUI.CurveField(new Rect(10, 40, position.width - 20, 40), "(Cl)Lift", m_CL);
        m_CD = EditorGUI.CurveField(new Rect(10, 90, position.width - 20, 40), "(Cd)Drag", m_CD);
        m_CM = EditorGUI.CurveField(new Rect(10, 140, position.width - 20, 40), "(Cm)Moment", m_CM);

        //Target aerofoil selector.
        m_Aerofoil = (Aerofoil)EditorGUI.ObjectField(new Rect(10, 200, position.width - 20, 20),
                "Target Aerofoil",
                m_Aerofoil,
                typeof(Aerofoil), true);

        //Filename preview / editor/
        m_FileName = EditorGUI.TextField(new Rect(10, 230, position.width - 20, 20), "Name:", m_FileName);

        //Rename toggle.
        m_Rename = EditorGUI.Toggle(new Rect(10, 250, position.width, 20), "Rename", m_Rename);

        //Apply
        if (GUI.Button(new Rect(10, 300, position.width - 20, 40), "APPLY"))
        {
            if (m_Aerofoil != null)
            {
                //Assign curves.
                m_Aerofoil.CL = m_CL;
                m_Aerofoil.CD = m_CD;
                m_Aerofoil.CM = m_CM;

                //Rename
                if (m_Rename == true)
                {
                    m_Aerofoil.name = m_FileName;
                }
            }
        }

    }
}