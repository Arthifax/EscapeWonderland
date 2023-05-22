using System.Collections.Generic;
using TinyGiantStudio.EditorHelpers;
using TinyGiantStudio.Modules;
using UnityEditor;
using UnityEditor.AnimatedValues;
using UnityEngine;

namespace TinyGiantStudio.Text
{
    [CustomEditor(typeof(Button))]
    public class ButtonEditor : Editor
    {
        public AssetSettings settings;

        Button myTarget;
        SerializedObject soTarget;

        SerializedProperty onClickEvents;
        SerializedProperty whileBeingClickedEvents;
        SerializedProperty onSelectEvents;
        SerializedProperty onUnselectEvents;

        SerializedProperty interactable;
        SerializedProperty interactableByMouse;

        SerializedProperty text;
        SerializedProperty background;

        SerializedProperty useStyles;

        SerializedProperty normalTextSize;
        SerializedProperty normalTextMaterial;
        SerializedProperty normalBackgroundMaterial;

        SerializedProperty useSelectedVisual;
        SerializedProperty selectedTextSize;
        SerializedProperty selectedTextMaterial;
        SerializedProperty selectedBackgroundMaterial;

        SerializedProperty usePressedVisual;
        SerializedProperty pressedTextSize;
        SerializedProperty pressedTextMaterial;
        SerializedProperty pressedBackgroundMaterial;
        SerializedProperty holdPressedVisualFor;


        SerializedProperty useDisabledVisual;
        SerializedProperty disabledTextSize;
        SerializedProperty disabledTextMaterial;
        SerializedProperty disabledBackgroundMaterial;

        SerializedProperty useModules;

        SerializedProperty unSelectModuleContainers;
        SerializedProperty onSelectModuleContainers;
        SerializedProperty onPressModuleContainers;
        SerializedProperty onClickModuleContainers;

        SerializedProperty hideOverwrittenVariablesFromInspector;


        GUIStyle foldOutStyle = null;
        GUIStyle iconButtonStyle = null;
        GUIStyle defaultLabel = null;
        GUIStyle defaultMultilineLabel = null;
        GUIStyle headerLabel = null;


        static Color openedFoldoutTitleColor = new Color(124 / 255f, 170 / 255f, 239 / 255f, 0.9f);
        static Color toggledOffColor = new Color(0.75f, 0.75f, 0.75f); //settings that are turned off but still visible

        AnimBool showEventSettings;
        AnimBool showModuleSettings;
        AnimBool showAdvancedSettings;

        AnimBool showDisabledItemSettings;
        AnimBool showPressedItemSettings;
        AnimBool showSelectedItemSettings;
        AnimBool showNormalItemSettings;
        AnimBool showVisualSettings;

        Texture documentationIcon;
        Texture addIcon;
        Texture deleteIcon;
        readonly float iconSize = 20;





        void OnEnable()
        {
            GetReferences();

            documentationIcon = EditorGUIUtility.Load("Assets/Plugins/Tiny Giant Studio/Modular 3D Text/Utility/Editor Icons/Icon_Documentation.png") as Texture;
            addIcon = EditorGUIUtility.Load("Assets/Plugins/Tiny Giant Studio/Modular 3D Text/Utility/Editor Icons/Icon_Plus.png") as Texture;
            deleteIcon = EditorGUIUtility.Load("Assets/Plugins/Tiny Giant Studio/Modular 3D Text/Utility/Editor Icons/deleteIcon.png") as Texture;

            if (!settings)
                settings = StaticMethods.VerifySettings(settings);
        }

        public override void OnInspectorGUI()
        {
            GenerateStyle();
            soTarget.Update();
            EditorGUI.BeginChangeCheck();

            Warning();
            GUILayout.Space(10);
            MainSettings();
            GUILayout.Space(10);
            Styles();
            GUILayout.Space(6);
            Events();
            GUILayout.Space(6);
            ModuleSettings();
            GUILayout.Space(6);
            AdvancedSettings();


            if (EditorGUI.EndChangeCheck())
            {
                soTarget.ApplyModifiedProperties();

                myTarget.UpdateStyle();

                EditorUtility.SetDirty(myTarget);
            }
        }
        void Warning()
        {
            if (myTarget.ApplyNormalStyle().Item1 || myTarget.ApplyOnSelectStyle().Item1 || myTarget.ApplyPressedStyle().Item1 || myTarget.ApplyDisabledStyle().Item1)
                EditorGUILayout.HelpBox("Some values are overwritten by parent list.", MessageType.Info);
        }


        void MainSettings()
        {
            EditorGUILayout.PropertyField(text);
            EditorGUILayout.PropertyField(background);

            GUILayout.Space(5);

            GUILayout.BeginHorizontal();
            float defaultLabelWidth = EditorGUIUtility.labelWidth;
            EditorGUIUtility.labelWidth = 80;
            EditorGUILayout.PropertyField(interactable);
            if (myTarget.interactable)
            {
                EditorGUIUtility.labelWidth = 120;
                EditorGUI.indentLevel = 1;
                EditorGUILayout.PropertyField(interactableByMouse, new GUIContent("By mouse/touch"));
            }
            GUILayout.FlexibleSpace();
            EditorGUIUtility.labelWidth = defaultLabelWidth;
            GUILayout.EndHorizontal();
        }

        void Styles()
        {
            if (myTarget.hideOverwrittenVariablesFromInspector && myTarget.ApplyNormalStyle().Item1 && myTarget.ApplyOnSelectStyle().Item1 && myTarget.ApplyPressedStyle().Item1 && myTarget.ApplyDisabledStyle().Item1)
                return;

            EditorGUI.indentLevel = 0;
            GUILayout.BeginVertical(EditorStyles.helpBox);

            GUILayout.BeginVertical(EditorStyles.toolbar);
            GUILayout.BeginHorizontal();
            EditorGUILayout.PropertyField(useStyles, GUIContent.none, GUILayout.MaxWidth(25));
            showVisualSettings.target = EditorGUILayout.Foldout(showVisualSettings.target, "Style", true, foldOutStyle);
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();

            if (EditorGUILayout.BeginFadeGroup(showVisualSettings.faded))
            {
                GUILayout.Space(8);
                NormalStyle();
                GUILayout.Space(5);
                SelectedStyle();
                GUILayout.Space(5);
                PressedItemSettings();
                GUILayout.Space(5);
                DisabledtStyle();
            }

            EditorGUILayout.EndFadeGroup();
            GUILayout.EndVertical();
        }

        void Events()
        {
            EditorGUI.indentLevel = 2;
            GUILayout.BeginVertical(EditorStyles.helpBox);

            GUILayout.BeginVertical(EditorStyles.toolbar);
            GUILayout.BeginHorizontal();
            GUIContent content = new GUIContent("Events",
                           "On Click: \nMouse click/Touch finished or list pressed enter " +
                           "\n\nWhile being Clicked: \nWhen click is pressed down" +
                           "\n\nOn Select: \nMouse hover or selected in a list ready to be clicked" +
                           "\n\nOn Unselect: \nMouse/Touch moved away or list unselected");

            showEventSettings.target = EditorGUILayout.Foldout(showEventSettings.target, content, true, foldOutStyle);
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();

            if (EditorGUILayout.BeginFadeGroup(showEventSettings.faded))
            {
                EditorGUILayout.PropertyField(onClickEvents);
                EditorGUILayout.PropertyField(whileBeingClickedEvents);
                EditorGUILayout.PropertyField(onSelectEvents);
                EditorGUILayout.PropertyField(onUnselectEvents);
            }

            EditorGUILayout.EndFadeGroup();
            GUILayout.EndVertical();
        }

        void ModuleSettings()
        {
            GUILayout.BeginVertical(EditorStyles.helpBox);
            EditorGUI.indentLevel = 0;

            GUILayout.BeginVertical(EditorStyles.toolbar);

            GUILayout.BeginHorizontal();

            EditorGUILayout.PropertyField(useModules, GUIContent.none, GUILayout.MaxWidth(25));
            showModuleSettings.target = EditorGUILayout.Foldout(showModuleSettings.target, new GUIContent("Modules", "Modules provide an easy way animate characters. \n They are called in two events. \n1. When new characters are added to text. \n2. When a character is removed from the text. \nThis change can be done by code or ui or anything. The text string is a property."), true, foldOutStyle);

            Documentation("https://ferdowsur.gitbook.io/modular-3d-text/modules", "Modules");

            GUILayout.EndHorizontal();

            GUILayout.EndVertical();

            string tooltip_onClick = "";
            string tooltip_whileBeingClicked = "";
            string tooltip_onSelect = "";
            string tooltip_onUnSelect = "";

            Color contentDefaultColor = GUI.contentColor;
            if (!myTarget.useModules)
                GUI.contentColor = toggledOffColor;

            if (EditorGUILayout.BeginFadeGroup(showModuleSettings.faded))
            {
                GUILayout.Space(5);
                EditorGUI.indentLevel = 2;
                ModuleDrawer.BaseModuleContainerList("On Click", tooltip_onClick, myTarget.onClickModuleContainers, onClickModuleContainers, soTarget);
                //ModuleContainerList("On Click", tooltip_onClick, myTarget.onClickModuleContainers, onClickModuleContainers);
                GUILayout.Space(10);
                ModuleDrawer.BaseModuleContainerList("Being Clicked", tooltip_whileBeingClicked, myTarget.onPressModuleContainers, onPressModuleContainers, soTarget);
                //ModuleContainerList("Being clicked", tooltip_whileBeingClicked, myTarget.onPressModuleContainers, onPressModuleContainers);
                GUILayout.Space(10);
                ModuleDrawer.BaseModuleContainerList("On Select", tooltip_onSelect, myTarget.onSelectModuleContainers, onSelectModuleContainers, soTarget);
                //ModuleContainerList("On Select", tooltip_onSelect, myTarget.onSelectModuleContainers, onSelectModuleContainers);
                GUILayout.Space(10);
                ModuleDrawer.BaseModuleContainerList("On Un-Select", tooltip_onUnSelect, myTarget.unSelectModuleContainers, unSelectModuleContainers, soTarget);
                //ModuleContainerList("On Un-Select", tooltip_onUnSelect, myTarget.unSelectModuleContainers, unSelectModuleContainers);
            }

            EditorGUILayout.EndFadeGroup();
            GUILayout.EndVertical();

            GUI.contentColor = contentDefaultColor;
        }

        void AdvancedSettings()
        {
            GUILayout.BeginVertical(EditorStyles.helpBox);
            EditorGUI.indentLevel = 1;

            GUILayout.BeginVertical(EditorStyles.toolbar);
            GUILayout.BeginHorizontal();
            showAdvancedSettings.target = EditorGUILayout.Foldout(showAdvancedSettings.target, new GUIContent("Advanced settings", ""), true, foldOutStyle);
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();

            if (EditorGUILayout.BeginFadeGroup(showAdvancedSettings.faded))
            {
                EditorGUI.indentLevel = 1;
                MText_Editor_Methods.HorizontalField(hideOverwrittenVariablesFromInspector, "Hide overwritten values", "Buttons under list sometimes have styles overwritten. This hides these variables", FieldSize.extraLarge);
            }

            EditorGUILayout.EndFadeGroup();
            GUILayout.EndVertical();
        }


        void NormalStyle()
        {
            if (myTarget.ApplyNormalStyle().Item1 && myTarget.hideOverwrittenVariablesFromInspector)
                return;

            Color contentDefaultColor = GUI.contentColor;
            if (!myTarget.useStyles)
                GUI.contentColor = toggledOffColor;

            EditorGUI.indentLevel = 1;
            GUILayout.BeginVertical(EditorStyles.helpBox);

            GUILayout.BeginVertical(EditorStyles.toolbar);
            GUILayout.BeginHorizontal();
            showNormalItemSettings.target = EditorGUILayout.Foldout(showNormalItemSettings.target, "Normal", true, foldOutStyle);
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();

            if (EditorGUILayout.BeginFadeGroup(showNormalItemSettings.faded))
            {
                EditorGUI.indentLevel = 0;

                if (myTarget.ApplyNormalStyle().Item1)
                {
                    EditorGUILayout.HelpBox("Normal style visuals are being overwritten by parent list", MessageType.Info);
                    GUILayout.Space(5);
                }

                GUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Text Size", defaultLabel, GUILayout.MaxWidth(70));
                EditorGUILayout.PropertyField(normalTextSize, GUIContent.none);
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                MText_Editor_Methods.PriviewField(normalTextMaterial, myTarget.NormalTextMaterial, "Text");
                MText_Editor_Methods.PriviewField(normalBackgroundMaterial, myTarget.NormalBackgroundMaterial, "Background");
                GUILayout.EndHorizontal();
            }

            EditorGUILayout.EndFadeGroup();
            GUILayout.EndVertical();

            GUI.contentColor = contentDefaultColor;
        }
        void SelectedStyle()
        {
            if (myTarget.ApplyOnSelectStyle().Item1 && myTarget.hideOverwrittenVariablesFromInspector)
                return;

            Color contentDefaultColor = GUI.contentColor;
            if (!myTarget.useStyles || !myTarget.useSelectedVisual)
                GUI.contentColor = toggledOffColor;

            EditorGUI.indentLevel = 0;
            GUILayout.BeginVertical(EditorStyles.helpBox);

            GUILayout.BeginVertical(EditorStyles.toolbar);
            GUILayout.BeginHorizontal();
            GUIContent content = new GUIContent("Selected", "Mouse hover or selected in a list ready to be clicked");
            EditorGUILayout.PropertyField(useSelectedVisual, GUIContent.none, GUILayout.MaxWidth(25));
            showSelectedItemSettings.target = EditorGUILayout.Foldout(showSelectedItemSettings.target, content, true, foldOutStyle);
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();

            if (EditorGUILayout.BeginFadeGroup(showSelectedItemSettings.faded))
            {
                EditorGUI.indentLevel = 0;
                if (myTarget.ApplyOnSelectStyle().Item1)
                {
                    EditorGUILayout.HelpBox("On select style visuals are being overwritten by parent list", MessageType.Info);
                    GUILayout.Space(5);
                }

                GUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Text Size", defaultLabel, GUILayout.MaxWidth(70));
                EditorGUILayout.PropertyField(selectedTextSize, GUIContent.none);
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                MText_Editor_Methods.PriviewField(selectedTextMaterial, myTarget.SelectedTextMaterial, "Text");
                MText_Editor_Methods.PriviewField(selectedBackgroundMaterial, myTarget.SelectedBackgroundMaterial, "Background");
                GUILayout.EndHorizontal();
            }

            EditorGUILayout.EndFadeGroup();
            GUILayout.EndVertical();
            GUI.contentColor = contentDefaultColor;
        }
        void PressedItemSettings()
        {
            if (myTarget.ApplyPressedStyle().Item1 && myTarget.hideOverwrittenVariablesFromInspector)
                return;

            Color contentDefaultColor = GUI.contentColor;
            if (!myTarget.useStyles || !myTarget.usePressedVisual)
                GUI.contentColor = toggledOffColor;

            EditorGUI.indentLevel = 0;
            GUILayout.BeginVertical(EditorStyles.helpBox);

            GUILayout.BeginVertical(EditorStyles.toolbar);
            GUILayout.BeginHorizontal();
            GUIContent content = new GUIContent("Pressed", "When click/tocuh is pressed down or for limited time after click");
            EditorGUILayout.PropertyField(usePressedVisual, GUIContent.none, GUILayout.MaxWidth(25));
            showPressedItemSettings.target = EditorGUILayout.Foldout(showPressedItemSettings.target, content, true, foldOutStyle);
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();

            if (EditorGUILayout.BeginFadeGroup(showPressedItemSettings.faded))
            {
                EditorGUI.indentLevel = 0;
                if (myTarget.ApplyPressedStyle().Item1)
                {
                    EditorGUILayout.HelpBox("Pressed style visuals are being overwritten by parent list", MessageType.Info);
                    GUILayout.Space(5);
                }
                GUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Text Size", defaultLabel, GUILayout.MaxWidth(70));
                EditorGUILayout.PropertyField(pressedTextSize, GUIContent.none);
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                MText_Editor_Methods.PriviewField(pressedTextMaterial, myTarget.PressedTextMaterial, "Text");
                MText_Editor_Methods.PriviewField(pressedBackgroundMaterial, myTarget.PressedBackgroundMaterial, "Background");
                GUILayout.EndHorizontal();

                MText_Editor_Methods.HorizontalField(holdPressedVisualFor, "Hold pressed for", "How long this visual lasts. This is not for mouse/touch click", FieldSize.large);
            }

            EditorGUILayout.EndFadeGroup();
            GUILayout.EndVertical();
            GUI.contentColor = contentDefaultColor;
        }
        void DisabledtStyle()
        {
            if (myTarget.ApplyDisabledStyle().Item1 && myTarget.hideOverwrittenVariablesFromInspector)
                return;

            Color contentDefaultColor = GUI.contentColor;
            if (!myTarget.useStyles || !myTarget.UseDisabledVisual)
                GUI.contentColor = toggledOffColor;


            EditorGUI.indentLevel = 0;
            GUILayout.BeginVertical(EditorStyles.helpBox);

            GUILayout.BeginVertical(EditorStyles.toolbar);
            GUILayout.BeginHorizontal();
            GUIContent content = new GUIContent("Disabled", "Style when button isn't interactable");
            EditorGUILayout.PropertyField(useDisabledVisual, GUIContent.none, GUILayout.MaxWidth(25));
            showDisabledItemSettings.target = EditorGUILayout.Foldout(showDisabledItemSettings.target, content, true, foldOutStyle);
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();

            if (EditorGUILayout.BeginFadeGroup(showDisabledItemSettings.faded))
            {
                EditorGUI.indentLevel = 0;
                if (myTarget.ApplyDisabledStyle().Item1)
                {
                    EditorGUILayout.HelpBox("Disabled style visuals are being overwritten by parent list", MessageType.Info);
                    GUILayout.Space(5);
                }
                GUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Text Size", defaultLabel, GUILayout.MaxWidth(70));
                EditorGUILayout.PropertyField(disabledTextSize, GUIContent.none);
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                MText_Editor_Methods.PriviewField(disabledTextMaterial, myTarget.DisabledTextMaterial, "Text");
                MText_Editor_Methods.PriviewField(disabledBackgroundMaterial, myTarget.DisabledBackgroundMaterial, "Background");
                GUILayout.EndHorizontal();
            }
            EditorGUILayout.EndFadeGroup();
            GUILayout.EndVertical();
            GUI.contentColor = contentDefaultColor;
        }

        void ModuleContainerList(string label, string tooltip, List<ModuleContainer> moduleContainers, SerializedProperty serializedContainer)
        {
            EditorGUI.indentLevel = 0;

            GUILayout.BeginVertical(EditorStyles.helpBox);

            GUILayout.BeginVertical(EditorStyles.helpBox);

            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            EditorGUILayout.LabelField(new GUIContent(label, tooltip), headerLabel);
            GUILayout.FlexibleSpace();
            if (GUILayout.Button(addIcon, iconButtonStyle, GUILayout.MaxHeight(iconSize), GUILayout.MaxWidth(iconSize)))
                myTarget.EmptyEffect(moduleContainers);
            GUILayout.EndHorizontal();

            GUILayout.EndVertical();

            GUILayout.Space(5);

            //GUI.backgroundColor = Color.white;
            //GUI.contentColor = originalContent;

            for (int i = 0; i < moduleContainers.Count; i++)
            {
                if (serializedContainer.arraySize <= i) //no module
                    continue;

                GUILayout.BeginVertical(EditorStyles.helpBox);


                //GUILayout.BeginVertical("CN EntryBackEven");
                EditorGUI.indentLevel = 0;
                GUILayout.BeginHorizontal();
                EditorGUILayout.PropertyField(serializedContainer.GetArrayElementAtIndex(i).FindPropertyRelative("module"), GUIContent.none, GUILayout.MinWidth(10));
                GUILayout.Label(GUIContent.none, GUILayout.MaxWidth(5));
                if (GUILayout.Button(deleteIcon, iconButtonStyle, GUILayout.MinHeight(iconSize), GUILayout.MaxWidth(iconSize)))
                {
                    Undo.RecordObject(myTarget, "Update button");
                    moduleContainers.RemoveAt(i);
                    EditorUtility.SetDirty(myTarget);
                }
                GUILayout.EndHorizontal();

                EditorGUI.indentLevel = 0;
                if (i < moduleContainers.Count)
                {
                    if (moduleContainers[i].module != null)
                    {
                        if (moduleContainers[i].variableHolders != null)
                        {
                            if (moduleContainers[i].module.variableHolders != null)
                            {
                                if (moduleContainers[i].variableHolders.Length != moduleContainers[i].module.variableHolders.Length)
                                {
                                    VariableHolder[] newHolder = new VariableHolder[moduleContainers[i].module.variableHolders.Length];
                                    for (int k = 0; k < newHolder.Length; k++)
                                    {
                                        if (k < moduleContainers[i].variableHolders.Length)
                                        {
                                            newHolder[k] = moduleContainers[i].variableHolders[k];
                                        }
                                    }
                                    moduleContainers[i].variableHolders = newHolder;
                                }
                            }

                            for (int j = 0; j < moduleContainers[i].variableHolders.Length; j++)
                            {
                                DrawModuleVariableHolder(moduleContainers, serializedContainer, i, j);
                            }

                            string warning = moduleContainers[i].module.VariableWarnings(moduleContainers[i].variableHolders);
                            if (warning != null)
                            {
                                if (warning.Length > 0)
                                {
                                    EditorGUILayout.HelpBox(warning, MessageType.Warning);
                                }
                            }
                        }
                    }
                }

                GUILayout.EndVertical();
                if (i + 1 != moduleContainers.Count)
                {
                    EditorGUILayout.Space(5);
                }
            }
            GUILayout.EndVertical();
        }

        void DrawModuleVariableHolder(List<ModuleContainer> moduleContainers, SerializedProperty serializedContainer, int i, int j)
        {
            if (moduleContainers[i].module.variableHolders != null)
            {
                if (!ShowProperty(moduleContainers[i].module.variableHolders, j, moduleContainers[i].variableHolders))
                    return;

                GUIContent variableLabel = null;
                if (moduleContainers[i].module.variableHolders[j].variableName != null)
                    if (moduleContainers[i].module.variableHolders[j].variableName != string.Empty)
                        variableLabel = new GUIContent(moduleContainers[i].module.variableHolders[j].variableName);

                if (variableLabel == null)
                    variableLabel = new GUIContent("Unlabeled variable");

                ModuleVariableType type = moduleContainers[i].module.variableHolders[j].type;

                SerializedProperty property;
                string propertyName = ModuleDrawer.GetPropertyName(type);

                if (moduleContainers != null)
                {
                    if (serializedContainer.arraySize > i)
                    {
                        if (serializedContainer.GetArrayElementAtIndex(i).FindPropertyRelative("variableHolders").arraySize > j)
                        {
                            property = serializedContainer.GetArrayElementAtIndex(i).FindPropertyRelative("variableHolders").GetArrayElementAtIndex(j).FindPropertyRelative(propertyName);
                            EditorGUILayout.PropertyField(property, variableLabel);
                        }
                    }
                }
            }
        }

        //should check from the module in module container list
        bool ShowProperty(VariableHolder[] moduleVariables, int i, VariableHolder[] textVariables)
        {
            if (moduleVariables[i].hideIf == null)
                return true;

            if (!string.IsNullOrEmpty(moduleVariables[i].hideIf))
            {
                for (int j = 0; j < moduleVariables.Length; j++)
                {
                    if (j == i)
                        continue;

                    if (moduleVariables[j].type == ModuleVariableType.@bool)
                    {
                        if (moduleVariables[j].variableName == moduleVariables[i].hideIf)
                        {
                            if (textVariables[j] == null)
                                return true;

                            if (textVariables[j].boolValue == true)
                                return false;
                            else
                                return true;
                        }
                    }
                }
            }
            return true;
        }
        void Documentation(string URL, string subject)
        {
            GUIContent doc = new GUIContent(documentationIcon, subject + " documentation\n\nURL: " + URL);
            if (GUILayout.Button(doc, iconButtonStyle, GUILayout.Height(iconSize), GUILayout.Width(iconSize)))
            {
                Application.OpenURL(URL);
            }
        }






        void GetReferences()
        {
            myTarget = (Button)target;
            soTarget = new SerializedObject(target);

            showModuleSettings = new AnimBool(false);
            showModuleSettings.valueChanged.AddListener(Repaint);
            showAdvancedSettings = new AnimBool(false);
            showAdvancedSettings.valueChanged.AddListener(Repaint);


            onSelectEvents = soTarget.FindProperty("onSelect");
            onUnselectEvents = soTarget.FindProperty("onUnselect");
            whileBeingClickedEvents = soTarget.FindProperty("whileBeingClicked");
            onClickEvents = soTarget.FindProperty("onClick");

            interactable = soTarget.FindProperty("interactable");
            interactableByMouse = soTarget.FindProperty("interactableByMouse");

            text = soTarget.FindProperty("_text");
            background = soTarget.FindProperty("_background");


            useStyles = soTarget.FindProperty("useStyles");

            normalTextSize = soTarget.FindProperty("_normalTextSize");
            normalTextMaterial = soTarget.FindProperty("_normalTextMaterial");
            normalBackgroundMaterial = soTarget.FindProperty("_normalBackgroundMaterial");

            useSelectedVisual = soTarget.FindProperty("useSelectedVisual");
            selectedTextSize = soTarget.FindProperty("_selectedTextSize");
            selectedTextMaterial = soTarget.FindProperty("_selectedTextMaterial");
            selectedBackgroundMaterial = soTarget.FindProperty("_selectedBackgroundMaterial");

            usePressedVisual = soTarget.FindProperty("usePressedVisual");
            pressedTextSize = soTarget.FindProperty("_pressedTextSize");
            pressedTextMaterial = soTarget.FindProperty("_pressedTextMaterial");
            pressedBackgroundMaterial = soTarget.FindProperty("_pressedBackgroundMaterial");
            holdPressedVisualFor = soTarget.FindProperty("holdPressedVisualFor");

            useDisabledVisual = soTarget.FindProperty("_useDisabledVisual");
            disabledTextSize = soTarget.FindProperty("_disabledTextSize");
            disabledTextMaterial = soTarget.FindProperty("_disabledTextMaterial");
            disabledBackgroundMaterial = soTarget.FindProperty("_disabledBackgroundMaterial");



            useModules = soTarget.FindProperty("useModules");

            unSelectModuleContainers = soTarget.FindProperty("unSelectModuleContainers");
            onSelectModuleContainers = soTarget.FindProperty("onSelectModuleContainers");
            onPressModuleContainers = soTarget.FindProperty("onPressModuleContainers");
            onClickModuleContainers = soTarget.FindProperty("onClickModuleContainers");

            hideOverwrittenVariablesFromInspector = soTarget.FindProperty("hideOverwrittenVariablesFromInspector");

            GetAnimBoolReferences();
        }
        void GetAnimBoolReferences()
        {
            showEventSettings = new AnimBool(false);
            showEventSettings.valueChanged.AddListener(Repaint);

            showVisualSettings = new AnimBool(false);
            showVisualSettings.valueChanged.AddListener(Repaint);

            showNormalItemSettings = new AnimBool(false);
            showNormalItemSettings.valueChanged.AddListener(Repaint);

            showSelectedItemSettings = new AnimBool(false);
            showSelectedItemSettings.valueChanged.AddListener(Repaint);

            showPressedItemSettings = new AnimBool(false);
            showPressedItemSettings.valueChanged.AddListener(Repaint);

            showDisabledItemSettings = new AnimBool(false);
            showDisabledItemSettings.valueChanged.AddListener(Repaint);
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
                    fontStyle = FontStyle.Bold,
                };
                foldOutStyle.onNormal.textColor = openedFoldoutTitleColor;
            }

            if (defaultLabel == null)
            {
                defaultLabel = new GUIStyle(EditorStyles.whiteMiniLabel)
                {
                    fontStyle = FontStyle.Italic,
                    fontSize = 11
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
                //defaultMultilineLabel.normal.textColor = new Color(0.9f, 0.9f, 0.9f, 0.75f);
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
            iconButtonStyle = new GUIStyle(EditorStyles.miniButtonMid);
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