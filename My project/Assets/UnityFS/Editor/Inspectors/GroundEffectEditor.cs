/*************************************************************************************

 * 游研堂: www.gamedev3d.com

 *（1）本站致力于为广大的游戏从业者提供相关的资源素材与资讯。

 *（2）本站会持续更新更多相关的资源素材，为游戏领域开发者提供更好的资讯与灵感！

 *（3）本站所有资源素材仅供学习参考，切勿用作商业用途，并请在下载后的24小时内进行删除，

 *     否则由此引发的法律纠纷及连带责任本站和发布者概不承担。
 
*************************************************************************************/
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GroundEffect))]
public class GroundEffectEditor : Editor
{
    SerializedProperty CLHeightVsChord;
    SerializedProperty CDHeightVsSpan;

    SerializedProperty RayCastAxis;
    SerializedProperty RayCastLayers;

    SerializedProperty Wingspan;

    private void OnEnable()
    {
        CLHeightVsChord = serializedObject.FindProperty("CLHeightVsChord");
        CDHeightVsSpan = serializedObject.FindProperty("CDHeightVsSpan");

        RayCastAxis = serializedObject.FindProperty("RayCastAxis");
        RayCastLayers = serializedObject.FindProperty("RayCastLayers");

        Wingspan = serializedObject.FindProperty("Wingspan");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        //Add default keyframes.
        if (CLHeightVsChord.animationCurveValue.keys.Length == 0)
        {
            CLHeightVsChord.animationCurveValue.AddKey(0.0f, 1.6f);
            CLHeightVsChord.animationCurveValue.AddKey(0.4f, 1.2f);
            CLHeightVsChord.animationCurveValue.AddKey(1.0f, 1.0f);
        }

        if (CDHeightVsSpan.animationCurveValue.keys.Length == 0)
        {
            CDHeightVsSpan.animationCurveValue.AddKey(0.0f, 0.2f);
            CDHeightVsSpan.animationCurveValue.AddKey(0.4f, 0.8f);
            CDHeightVsSpan.animationCurveValue.AddKey(1.0f, 1.0f);
        }

        EditorGUILayout.PropertyField(CLHeightVsChord, new GUIContent("CL Height Vs Chord"));
        EditorGUILayout.PropertyField(CDHeightVsSpan, new GUIContent("CD Height Vs Span"));
        EditorGUILayout.Space();
        EditorGUILayout.PropertyField(RayCastAxis, new GUIContent("RayCast Axis"));
        EditorGUILayout.PropertyField(RayCastLayers, new GUIContent("RayCast Layers"));
        EditorGUILayout.Space();
        EditorGUILayout.PropertyField(Wingspan, new GUIContent("Wing Span"));

        serializedObject.ApplyModifiedProperties();
    }

}
