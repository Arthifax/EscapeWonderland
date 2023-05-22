using TinyGiantStudio.EditorHelpers;
using UnityEditor;
using UnityEditor.AnimatedValues;
using UnityEngine;

namespace TinyGiantStudio.Text
{
    [CustomEditor(typeof(InputField))]
    public class InputFieldEditor : Editor
    {
        public AssetSettings settings;

        readonly float defaultSmallHorizontalFieldSize = 72.5f;
        readonly float defaultNormalltHorizontalFieldSize = 100;
        readonly float defaultLargeHorizontalFieldSize = 120f;
        readonly float defaultExtraLargeHorizontalFieldSize = 150f;


        InputField myTarget;
        SerializedObject soTarget;

        SerializedProperty autoFocusOnGameStart;
        SerializedProperty interactable;

        SerializedProperty maxCharacter;
        SerializedProperty caret;
        SerializedProperty hideCaretIfMaxCharacter;

        SerializedProperty enterKeyEndsInput;

        SerializedProperty contentType;

        SerializedProperty textComponent;
        SerializedProperty background;

        SerializedProperty text;
        SerializedProperty placeHolderText;

        SerializedProperty placeHolderTextMat;

        SerializedProperty inFocusTextMat;
        SerializedProperty inFocusBackgroundMat;

        SerializedProperty outOfFocusTextMat;
        SerializedProperty outOfFocusBackgroundMat;

        SerializedProperty disabledTextMat;
        SerializedProperty disabledBackgroundMat;

        SerializedProperty typeSound;
        SerializedProperty audioSource;

        SerializedProperty onInput;
        SerializedProperty onBackspace;
        SerializedProperty onInputEnd;


        AnimBool showMainSettings;
        AnimBool showStyleSettings;
        AnimBool showAudioSettings;
        AnimBool showUnityEventSettings;

        Color openedFoldoutTitleColor = new Color(124 / 255f, 170 / 255f, 239 / 255f, 1);

        GUIStyle foldOutStyle;
        GUIStyle defaultLabel = null;
        GUIStyle defaultMultilineLabel = null;
        GUIStyle headerLabel = null;



        void OnEnable()
        {
            myTarget = (InputField)target;
            soTarget = new SerializedObject(target);

            FindProperties();

            AnimBools();
        }



        public override void OnInspectorGUI()
        {
            GenerateStyle();
            soTarget.Update();
            EditorGUI.BeginChangeCheck();

            GUILayout.Space(10);
            MainSettings();
            StyleSettings();
            AudioSettings();
            UnityEventsSettings();

            if (EditorGUI.EndChangeCheck())
            {
                soTarget.ApplyModifiedProperties();
                ApplyModifiedValuesToGraphics();
                EditorUtility.SetDirty(myTarget);
            }
        }

        void MainSettings()
        {
            GUILayout.BeginVertical(EditorStyles.helpBox);
            EditorGUI.indentLevel = 1;
            GUILayout.BeginVertical(EditorStyles.toolbar);
            GUIContent content = new GUIContent("Main");
            showMainSettings.target = EditorGUILayout.Foldout(showMainSettings.target, content, true, foldOutStyle);
            GUILayout.EndVertical();
            if (EditorGUILayout.BeginFadeGroup(showMainSettings.faded))
            {
                EditorGUI.indentLevel = 0;

                MText_Editor_Methods.ItalicHorizontalField(text, "Text");
                MText_Editor_Methods.ItalicHorizontalField(placeHolderText, "Placeholder");
                EditorGUILayout.Space(1);
                MText_Editor_Methods.ItalicHorizontalField(enterKeyEndsInput, "'Enter' ends Input", "", FieldSize.large);
                MText_Editor_Methods.ItalicHorizontalField(caret, "Caret");
                MText_Editor_Methods.ItalicHorizontalField(maxCharacter, "Max Char", "The maximum amount of character allowed in the input field.");
                MText_Editor_Methods.ItalicHorizontalField(hideCaretIfMaxCharacter, "Hide caret if max char", "This hides the typing symbol when max character has been typed.", FieldSize.gigantic);

                EditorGUILayout.Space(5);
                MText_Editor_Methods.ItalicHorizontalField(contentType, "Content Type");
                EditorGUILayout.Space(5);

                if (!StaticMethods.GetParentList(myTarget.transform))
                {
                    MText_Editor_Methods.ItalicHorizontalField(autoFocusOnGameStart, "Auto Focus", "If not in a list, this focuses the element on game start. \nFocused = you can type to give input.", FieldSize.small);
                }
                MText_Editor_Methods.ItalicHorizontalField(interactable, "Interactable", "", FieldSize.small);
                EditorGUILayout.Space(10);

                if (!myTarget.textComponent)
                    EditorGUILayout.HelpBox("Text Component is required", MessageType.Error);
                HorizontalField(textComponent, "Text Component", "Reference to the 3D Text where input will be shown");
                HorizontalField(background, "Background");
                EditorGUILayout.Space(10);
            }
            EditorGUILayout.EndFadeGroup();

            GUILayout.EndVertical();
        }
        void StyleSettings()
        {
            GUILayout.BeginVertical(EditorStyles.helpBox);
            EditorGUI.indentLevel = 1;
            GUILayout.BeginVertical(EditorStyles.toolbar);
            GUIContent content = new GUIContent("Style");
            showStyleSettings.target = EditorGUILayout.Foldout(showStyleSettings.target, content, true, foldOutStyle);
            GUILayout.EndVertical();
            if (EditorGUILayout.BeginFadeGroup(showStyleSettings.faded))
            {
                GUILayout.Space(5);
                TextMats();
                GUILayout.Space(5);
                BackgroundMats();
            }
            EditorGUILayout.EndFadeGroup();

            GUILayout.EndVertical();
        }

        void TextMats()
        {
            EditorGUI.indentLevel = 0;

            GUILayout.BeginVertical(EditorStyles.helpBox);
            EditorGUILayout.LabelField("Text Metarial", headerLabel);
            EditorGUI.indentLevel = 0;


            GUILayout.BeginHorizontal();
            MText_Editor_Methods.PriviewField(placeHolderTextMat, myTarget.placeHolderTextMat, "Placeholder Text", "");
            MText_Editor_Methods.PriviewField(disabledTextMat, myTarget.disabledTextMat, "Disabled Text", "");
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            MText_Editor_Methods.PriviewField(inFocusTextMat, myTarget.inFocusTextMat, "In Focus Text", "");
            MText_Editor_Methods.PriviewField(outOfFocusTextMat, myTarget.outOfFocusTextMat, "Out of Focus Text", "");
            GUILayout.EndHorizontal();


            GUILayout.EndVertical();
        }

        void BackgroundMats()
        {
            EditorGUI.indentLevel = 0;

            GUILayout.BeginVertical(EditorStyles.helpBox);
            EditorGUILayout.LabelField("Background Metarial", headerLabel);
            EditorGUI.indentLevel = 0;


            GUILayout.BeginHorizontal();
            MText_Editor_Methods.PriviewField(inFocusBackgroundMat, myTarget.inFocusBackgroundMat, "In Focus", "");
            MText_Editor_Methods.PriviewField(outOfFocusBackgroundMat, myTarget.outOfFocusBackgroundMat, "Out of Focus", "");
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            MText_Editor_Methods.PriviewField(disabledBackgroundMat, myTarget.disabledBackgroundMat, "Disabled", "");
            GUILayout.EndHorizontal();

            GUILayout.EndVertical();
        }

        void AudioSettings()
        {
            GUILayout.BeginVertical(EditorStyles.helpBox);
            EditorGUI.indentLevel = 1;
            GUILayout.BeginVertical(EditorStyles.toolbar);
            GUIContent content = new GUIContent("Audio");
            showAudioSettings.target = EditorGUILayout.Foldout(showAudioSettings.target, content, true, foldOutStyle);
            GUILayout.EndVertical();
            if (EditorGUILayout.BeginFadeGroup(showAudioSettings.faded))
            {
                EditorGUI.indentLevel = 0;

                HorizontalField(typeSound, "Type Sound");
                HorizontalField(audioSource, "Audio Source");
            }
            EditorGUILayout.EndFadeGroup();
            GUILayout.EndVertical();
        }

        void UnityEventsSettings()
        {
            GUILayout.BeginVertical(EditorStyles.helpBox);
            EditorGUI.indentLevel = 1;

            GUILayout.BeginVertical(EditorStyles.toolbar);
            GUIContent content = new GUIContent("Events");
            showUnityEventSettings.target = EditorGUILayout.Foldout(showUnityEventSettings.target, content, true, foldOutStyle);
            GUILayout.EndVertical();

            if (EditorGUILayout.BeginFadeGroup(showUnityEventSettings.faded))
            {
                EditorGUILayout.PropertyField(onInput);
                EditorGUILayout.PropertyField(onBackspace);
                EditorGUILayout.PropertyField(onInputEnd);
            }
            EditorGUILayout.EndFadeGroup();

            GUILayout.EndVertical();
        }

        void ApplyModifiedValuesToGraphics()
        {
            if (!myTarget.interactable)
                myTarget.UninteractableUsedByEditorOnly();
            else
                myTarget.InteractableUsedByEditorOnly();
        }





        void FindProperties()
        {
            autoFocusOnGameStart = soTarget.FindProperty("autoFocusOnGameStart");
            interactable = soTarget.FindProperty("interactable");

            maxCharacter = soTarget.FindProperty("maxCharacter");

            caret = soTarget.FindProperty("caret");
            hideCaretIfMaxCharacter = soTarget.FindProperty("hideCaretIfMaxCharacter");

            enterKeyEndsInput = soTarget.FindProperty("enterKeyEndsInput");
            contentType = soTarget.FindProperty("contentType");

            textComponent = soTarget.FindProperty("textComponent");
            background = soTarget.FindProperty("background");

            text = soTarget.FindProperty("_text");
            placeHolderText = soTarget.FindProperty("placeHolderText");


            placeHolderTextMat = soTarget.FindProperty("placeHolderTextMat");

            inFocusTextMat = soTarget.FindProperty("inFocusTextMat");
            inFocusBackgroundMat = soTarget.FindProperty("inFocusBackgroundMat");

            outOfFocusTextMat = soTarget.FindProperty("outOfFocusTextMat");
            outOfFocusBackgroundMat = soTarget.FindProperty("outOfFocusBackgroundMat");

            disabledTextMat = soTarget.FindProperty("disabledTextMat");
            disabledBackgroundMat = soTarget.FindProperty("disabledBackgroundMat");


            disabledBackgroundMat = soTarget.FindProperty("disabledBackgroundMat");

            typeSound = soTarget.FindProperty("typeSound");
            audioSource = soTarget.FindProperty("audioSource");

            onInput = soTarget.FindProperty("onInput");
            onBackspace = soTarget.FindProperty("onBackspace");
            onInputEnd = soTarget.FindProperty("onInputEnd");
        }
        void AnimBools()
        {
            showMainSettings = new AnimBool(true);
            showMainSettings.valueChanged.AddListener(Repaint);

            showStyleSettings = new AnimBool(false);
            showStyleSettings.valueChanged.AddListener(Repaint);

            showAudioSettings = new AnimBool(false);
            showAudioSettings.valueChanged.AddListener(Repaint);

            showUnityEventSettings = new AnimBool(false);
            showUnityEventSettings.valueChanged.AddListener(Repaint);
        }


        void GenerateStyle()
        {
            if (EditorGUIUtility.isProSkin)
            {
                if (settings)
                    openedFoldoutTitleColor = settings.openedFoldoutTitleColor_darkSkin;
            }
            else
            {
                if (settings)
                    openedFoldoutTitleColor = settings.openedFoldoutTitleColor_lightSkin;
            }


            if (foldOutStyle == null)
            {
                foldOutStyle = new GUIStyle(EditorStyles.foldout)
                {
                    overflow = new RectOffset(-10, 0, 3, 0),
                    padding = new RectOffset(15, 0, -3, 0),
                    fontStyle = FontStyle.Bold
                };
                foldOutStyle.onNormal.textColor = openedFoldoutTitleColor;
            }


            if (defaultLabel == null)
            {
                defaultLabel = new GUIStyle(EditorStyles.whiteMiniLabel)
                {
                    fontStyle = FontStyle.Italic,
                    fontSize = 12
                };
                defaultLabel.normal.textColor = new Color(0.9f, 0.9f, 0.9f, 0.75f);
            }
            if (defaultMultilineLabel == null)
            {
                defaultMultilineLabel = new GUIStyle(EditorStyles.wordWrappedLabel)
                {
                    fontSize = 10,
                    fontStyle = FontStyle.Italic,
                    alignment = TextAnchor.MiddleCenter,
                };
                defaultMultilineLabel.normal.textColor = new Color(0.9f, 0.9f, 0.9f, 0.75f);
            }
            if (headerLabel == null)
            {
                headerLabel = new GUIStyle(EditorStyles.wordWrappedLabel)
                {
                    fontSize = 12,
                    fontStyle = FontStyle.Bold,
                    alignment = TextAnchor.MiddleCenter,
                };
            }
        }

        void HorizontalField(SerializedProperty property, string label, string tooltip = "", FieldSize fieldSize = FieldSize.normal)
        {
            float myMaxWidth;
            //not to self: it's ternary operator not tarnary operator. Stop mistyping
            if (settings)
                myMaxWidth = fieldSize == FieldSize.small ? settings.smallHorizontalFieldSize : fieldSize == FieldSize.normal ? settings.normalltHorizontalFieldSize : fieldSize == FieldSize.large ? settings.largeHorizontalFieldSize : fieldSize == FieldSize.extraLarge ? settings.extraLargeHorizontalFieldSize : settings.normalltHorizontalFieldSize;
            else
                myMaxWidth = fieldSize == FieldSize.small ? defaultSmallHorizontalFieldSize : fieldSize == FieldSize.normal ? defaultNormalltHorizontalFieldSize : fieldSize == FieldSize.large ? defaultLargeHorizontalFieldSize : fieldSize == FieldSize.extraLarge ? defaultExtraLargeHorizontalFieldSize : settings.normalltHorizontalFieldSize;

            GUILayout.BeginHorizontal();
            GUIContent gUIContent = new GUIContent(label, tooltip);
            EditorGUILayout.LabelField(gUIContent, GUILayout.MaxWidth(myMaxWidth));
            EditorGUILayout.PropertyField(property, GUIContent.none);
            GUILayout.EndHorizontal();
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