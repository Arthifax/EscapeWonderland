using TinyGiantStudio.EditorHelpers;
using UnityEditor;
/// Created by Ferdowsur Asif @ Tiny Giant Studio
using UnityEngine;

namespace TinyGiantStudio.Text
{
    [CustomEditor(typeof(Toggle))]
    public class ToggleEditor : Editor
    {
        Toggle myTarget;
        SerializedObject soTarget;

        SerializedProperty isOn;
        SerializedProperty onGraphic;
        SerializedProperty onEvent;
        SerializedProperty offGraphic;
        SerializedProperty offEvent;


        void OnEnable()
        {
            myTarget = (Toggle)target;
            soTarget = new SerializedObject(target);

            isOn = soTarget.FindProperty("_isOn");
            onGraphic = soTarget.FindProperty("onGraphic");
            onEvent = soTarget.FindProperty("onEvent");
            offGraphic = soTarget.FindProperty("offGraphic");
            offEvent = soTarget.FindProperty("offEvent");
        }

        public override void OnInspectorGUI()
        {
            soTarget.Update();
            EditorGUI.BeginChangeCheck();

            MainSettings();

            if (EditorGUI.EndChangeCheck())
            {
                myTarget.VisualUpdate();
                soTarget.ApplyModifiedProperties();
                ApplyMyModifiedData();
                EditorUtility.SetDirty(myTarget);
            }
        }

        void ApplyMyModifiedData()
        {
            if (myTarget.IsOn)
                myTarget.ActiveVisualUpdate();
            else
                myTarget.InactiveVisualUpdate();
        }

        void MainSettings()
        {
            EditorGUI.indentLevel = 0;
            GUILayout.Space(2.5f);
            MText_Editor_Methods.ItalicHorizontalField(isOn, "Is On");
            GUILayout.Space(10f);

            GUILayout.BeginVertical(EditorStyles.helpBox);
            MText_Editor_Methods.ItalicHorizontalField(onGraphic, "On Graphic");
            EditorGUILayout.PropertyField(onEvent, GUIContent.none);
            GUILayout.EndVertical();
            GUILayout.Space(5);

            GUILayout.BeginVertical(EditorStyles.helpBox);
            MText_Editor_Methods.ItalicHorizontalField(offGraphic, "Off Graphic");
            EditorGUILayout.PropertyField(offEvent, GUIContent.none);
            GUILayout.EndVertical();

        }

        //void DrawUILine(Color color, int thickness = 1, int padding = 0)
        //{
        //    Rect r = EditorGUILayout.GetControlRect(GUILayout.Height(padding + thickness));
        //    r.height = thickness;
        //    r.y += padding / 2;
        //    r.x -= 2;
        //    r.width += 6;
        //    EditorGUI.DrawRect(r, color);
        //}
    }
}