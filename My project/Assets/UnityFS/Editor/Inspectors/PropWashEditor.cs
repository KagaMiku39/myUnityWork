/*************************************************************************************

 * 游研堂: www.gamedev3d.com

 *（1）本站致力于为广大的游戏从业者提供相关的资源素材与资讯。

 *（2）本站会持续更新更多相关的资源素材，为游戏领域开发者提供更好的资讯与灵感！

 *（3）本站所有资源素材仅供学习参考，切勿用作商业用途，并请在下载后的24小时内进行删除，

 *     否则由此引发的法律纠纷及连带责任本站和发布者概不承担。
 
*************************************************************************************/
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PropWash))]
public class PropWashEditor : Editor
{
    private bool m_AffectedSectionsFoldOut = true;

    SerializedProperty AffectedSections;

    SerializedProperty PropWashSource;
    SerializedProperty PropWashStrength;

    private void OnEnable()
    {
        AffectedSections = serializedObject.FindProperty("AffectedSections");

        PropWashSource = serializedObject.FindProperty("PropWashSource");
        PropWashStrength = serializedObject.FindProperty("PropWashStrength");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(PropWashSource, new GUIContent("Propwash Source"));
        EditorGUILayout.PropertyField(PropWashStrength, new GUIContent("Strength Multiplier"));

        m_AffectedSectionsFoldOut = EditorGUILayout.Foldout(m_AffectedSectionsFoldOut, "Affected Sections (Root outwards)");
        if (m_AffectedSectionsFoldOut == true)
        {
            for (int i = 0; i < AffectedSections.arraySize; i++)
            {
                SerializedProperty affectedSection = AffectedSections.GetArrayElementAtIndex(i);
                
                affectedSection.boolValue = EditorGUILayout.Toggle(i.ToString(), affectedSection.boolValue);
            }
        }

        EditorGUILayout.Space();

        if (PropWashSource.objectReferenceValue == null)
        {
            EditorGUILayout.HelpBox("No prop wash source. This will do nothing!", MessageType.Error);
        }

        serializedObject.ApplyModifiedProperties();
    }
}
