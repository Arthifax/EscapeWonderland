using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif
using TinyGiantStudio.Layout;
using TinyGiantStudio.Modules;


namespace TinyGiantStudio.Text
{
    [DisallowMultipleComponent]
    [AddComponentMenu("Tiny Giant Studio/Modular 3D Text/List", order: 20007)]
    [HelpURL("https://ferdowsur.gitbook.io/modular-3d-text/ui/list")]
    public class List : MonoBehaviour
    {
        [Tooltip("List is scrollable with keyboard when focused")]
        [SerializeField] bool autoFocusOnStart = true;
        [Tooltip("Selects first item when focused")]
        [SerializeField] bool autoFocusFirstItem = true;



        #region Style options -------------------------------------------------------------------

        [SerializeField]
        private bool _useStyle = false;
        public bool UseStyle
        {
            get { return _useStyle; }
            set
            {
                _useStyle = value;
                UpdateStyle();
            }
        }

        [SerializeField]
        private bool _useNormalItemVisual = false;
        public bool UseNormalItemVisual
        {
            get { return _useNormalItemVisual; }
            set
            {
                _useNormalItemVisual = value;
                UpdateStyle();
            }
        }


        [SerializeField]
        private Vector3 _normalTextSize = new Vector3(10, 10, 1f);
        public Vector3 NormalTextSize
        {
            get { return _normalTextSize; }
            set
            {
                _normalTextSize = value;
                UpdateStyle();
            }
        }

        [SerializeField]
        private Material _normalTextMaterial = null;
        public Material NormalTextMaterial
        {
            get { return _normalTextMaterial; }
            set
            {
                _normalTextMaterial = value;
                UpdateStyle();
            }
        }

        [SerializeField]
        private Material _normalBackgroundMaterial = null;
        public Material NormalBackgroundMaterial
        {
            get { return _normalBackgroundMaterial; }
            set
            {
                _normalBackgroundMaterial = value;
                UpdateStyle();
            }
        }



        [SerializeField]
        private bool _useSelectedItemVisual = false;
        public bool UseSelectedItemVisual
        {
            get { return _useSelectedItemVisual; }
            set
            {
                _useSelectedItemVisual = value;
                UpdateStyle();
            }
        }

        [SerializeField]
        private Vector3 _selectedTextSize = new Vector3(10.5f, 10.5f, 5f);
        public Vector3 SelectedTextSize
        {
            get { return _selectedTextSize; }
            set
            {
                _selectedTextSize = value;
                UpdateStyle();
            }
        }

        [SerializeField]
        private Material _selectedTextMaterial = null;
        public Material SelectedTextMaterial
        {
            get { return _selectedTextMaterial; }
            set
            {
                _selectedTextMaterial = value;
                UpdateStyle();
            }
        }

        [SerializeField]
        private Material _selectedBackgroundMaterial = null;
        public Material SelectedBackgroundMaterial
        {
            get { return _selectedBackgroundMaterial; }
            set
            {
                _selectedBackgroundMaterial = value;
                UpdateStyle();
            }
        }



        [SerializeField]
        private bool _usePressedItemVisual = false;
        public bool UsePressedItemVisual
        {
            get { return _usePressedItemVisual; }
            set
            {
                _usePressedItemVisual = value;
                UpdateStyle();
            }
        }

        [SerializeField]
        private Vector3 _pressedTextSize = new Vector3(10.5f, 10.5f, 5f);
        public Vector3 PressedTextSize
        {
            get { return _pressedTextSize; }
            set
            {
                _pressedTextSize = value;
                UpdateStyle();
            }
        }
        [SerializeField]
        private Material _pressedTextMaterial = null;
        public Material PressedTextMaterial
        {
            get { return _pressedTextMaterial; }
            set
            {
                _pressedTextMaterial = value;
                UpdateStyle();
            }
        }

        [SerializeField]
        private Material _pressedBackgroundMaterial = null;
        public Material PressedBackgroundMaterial
        {
            get { return _pressedBackgroundMaterial; }
            set
            {
                _pressedBackgroundMaterial = value;
                UpdateStyle();
            }
        }

        public float holdPressedVisualFor = 0.15f;



        [SerializeField]
        private bool _useDisabledItemVisual = false;
        public bool UseDisabledItemVisual
        {
            get { return _useDisabledItemVisual; }
            set
            {
                _useDisabledItemVisual = value;
                UpdateStyle();
            }
        }

        [SerializeField]
        private Vector3 _disabledTextSize = new Vector3(9, 9, 1);
        public Vector3 DisabledTextSize
        {
            get { return _disabledTextSize; }
            set
            {
                _disabledTextSize = value;
                UpdateStyle();
            }
        }
        [SerializeField]
        private Material _disabledTextMaterial = null;
        public Material DisabledTextMaterial
        {
            get { return _disabledTextMaterial; }
            set
            {
                _disabledTextMaterial = value;
                UpdateStyle();
            }
        }
        [SerializeField]
        private Material _disabledBackgroundMaterial = null;
        public Material DisabledBackgroundMaterial
        {
            get { return _disabledBackgroundMaterial; }
            set
            {
                _disabledBackgroundMaterial = value;
                UpdateStyle();
            }
        }

        #endregion Style Options -------------------------------------------------------------------




        public bool useModules = true;
        public bool ignoreChildModules = false;

        public bool ignoreChildUnSelectModuleContainers = false;
        public bool applyUnSelectModuleContainers = true;
        public List<ModuleContainer> unSelectModuleContainers = new List<ModuleContainer>();

        public bool ignoreChildOnSelectModuleContainers = false;
        public bool applyOnSelectModuleContainers = true;
        public List<ModuleContainer> onSelectModuleContainers = new List<ModuleContainer>();

        public bool ignoreChildOnPressModuleContainers = false;
        public bool applyOnPressModuleContainers = true;
        public List<ModuleContainer> onPressModuleContainers = new List<ModuleContainer>();

        public bool ignoreChildOnClickModuleContainers = false;
        public bool applyOnClickModuleContainers = true;
        public List<ModuleContainer> onClickModuleContainers = new List<ModuleContainer>();

        public int selectedItem = 0;

        private float returnToSelectedTime = 0;
        bool pressed = false;
        public bool selected = false; //used to check before press
        int counterToCheckIfAllItemsAreDisabledToAvoidInfiniteLoop = 0;
        int previousSelection = 0;




        #region remember inspector layout
#if UNITY_EDITOR
        [HideInInspector] public bool showAnimationSettings = false;
        [HideInInspector] public bool showKeyboardSettings = false;
#endif
        #endregion remember inspector layout


        #region Unity things
        void Awake()
        {
            if (!autoFocusOnStart)
            {
                this.enabled = false;
#if ENABLE_INPUT_SYSTEM
                if (GetComponent<PlayerInput>())
                    GetComponent<PlayerInput>().enabled = false;
#endif
                if (GetComponent<ButtonInputSystemLocal>())
                    GetComponent<ButtonInputSystemLocal>().enabled = false;

                return;
            }

            this.enabled = true;
#if ENABLE_INPUT_SYSTEM
            if (GetComponent<PlayerInput>())
                GetComponent<PlayerInput>().enabled = true;
#endif
            if (GetComponent<ButtonInputSystemLocal>())
                GetComponent<ButtonInputSystemLocal>().enabled = true;






            if (autoFocusFirstItem)
                SelectTheFirstSelectableItem();
            else
                UnselectEverything();


        }

        void Start()
        {
            if (!autoFocusOnStart)
                return;

            ButtonInputSystemGlobal buttonInputProcessor = ButtonInputSystemGlobal.Instance;
            if (buttonInputProcessor != null)
            {
                buttonInputProcessor.selectedInputSystem = GetComponent<ButtonInputSystemLocal>();
            }
        }

        void Update()
        {
            if (transform.childCount == 0)
                return;

            if (pressed)
            {
                if (Time.time > returnToSelectedTime)
                {
                    pressed = false;
                    if (transform.GetChild(selectedItem).GetComponent<Button>())
                        transform.GetChild(selectedItem).GetComponent<Button>().SelectedButtonVisualUpdate();
                }
            }
        }

        void OnDisable()
        {
#if ENABLE_INPUT_SYSTEM
            if (GetComponent<PlayerInput>())
                GetComponent<PlayerInput>().enabled = false;
#elif ENABLE_LEGACY_INPUT_MANAGER
            if (GetComponent<ButtonInputProcessor>())
                GetComponent<ButtonInputProcessor>().enabled = false;
#endif
        }
        #endregion Unity things





        #region Public
        /// <summary>
        /// Updates the list positioning
        /// </summary>
        [ContextMenu("Update List")]
        public void UpdateList()
        {
            if (GetComponent<LayoutGroup>())
                GetComponent<LayoutGroup>().UpdateLayout();
        }



        /// <summary>
        /// Toggles focus
        /// </summary>
        public void Focus()
        {
            FocusToggle();
        }
        /// <summary>
        /// Focuses/defocuses the list
        /// </summary>
        /// <param name="enable"></param>
        public void Focus(bool enable)
        {
            pressed = false;
            selected = false;

            if (enable)
            {

                if (autoFocusFirstItem)
                    SelectTheFirstSelectableItem();

                //UnselectEverything();//this was here
                UnselectEverythingExceptSelected();

                if (gameObject.activeInHierarchy)
                    StartCoroutine(FocusRoutine());
                else
                {
                    this.enabled = true;
#if ENABLE_INPUT_SYSTEM
                    if (GetComponent<PlayerInput>())
                        GetComponent<PlayerInput>().enabled = true;
#elif ENABLE_LEGACY_INPUT_MANAGER
                    if (GetComponent<ButtonInputProcessor>())
                        GetComponent<ButtonInputProcessor>().enabled = true;
#endif
                }
            }
            else
            {
                UnselectEverything();
                this.enabled = false;
#if ENABLE_INPUT_SYSTEM
                if (GetComponent<PlayerInput>())
                    GetComponent<PlayerInput>().enabled = false;
#elif ENABLE_LEGACY_INPUT_MANAGER
                if (GetComponent<ButtonInputProcessor>())
                    GetComponent<ButtonInputProcessor>().enabled = false;
#endif
            }
        }
        /// <summary>
        /// Focuses/defocus the list with a single frame delay if true is passed as second parameter
        /// </summary>
        /// <param name="enable"></param>
        /// <param name="delay"></param>
        public void Focus(bool enable, bool delay)
        {
            pressed = false;
            selected = false;

            if (enable)
            {
                UnselectEverything();

                if (autoFocusFirstItem)
                    SelectTheFirstSelectableItem();

                if (delay && gameObject.activeInHierarchy)
                    StartCoroutine(FocusRoutine());
                else
                    this.enabled = true;
            }
            else
            {
                UnselectEverything();
                this.enabled = enable;
            }
        }
        /// <summary>
        /// Switches the focus mode
        /// </summary>
        [ContextMenu("Toggle Focus")]
        public void FocusToggle()
        {
            if (this.enabled)
                Focus(false, true);
            else
                Focus(true, true);
        }

        //coroutine to delay a single frame to avoid pressing "enter" key in one list to apply to another list just getting enabled
        private IEnumerator FocusRoutine()
        {
            yield return null;
            this.enabled = true;
#if ENABLE_INPUT_SYSTEM
            if (GetComponent<PlayerInput>())
                GetComponent<PlayerInput>().enabled = true;
#elif ENABLE_LEGACY_INPUT_MANAGER
            if (GetComponent<ButtonInputProcessor>())
                GetComponent<ButtonInputProcessor>().enabled = true;
#endif
        }



        /// <summary>
        /// Processes the select item for the list. Doesn't let the item's components know it was selected
        /// Call the AlertSelectedItem(int) to update the list
        /// </summary>
        /// <param name="number"></param>
        public void SelectItem(int number)
        {
            if (transform.childCount > number)
            {
                selected = true;

                UpdateList();

                selectedItem = number;
            }
        }
        /// <summary>
        /// Alerts the list item that it was selected. Doesn't alert the list.
        /// Call the SelectItem(int) to update the list
        /// </summary>
        /// <param name="number"></param>
        public void AlertSelectedItem(int number)
        {
            if (transform.childCount > number && number >= 0)
            {
                Transform itemTransform = transform.GetChild(number);

                //Unity objects should not use null propagation. Remember this, just use if statement. - note to self
                if (itemTransform.GetComponent<Button>())
                    itemTransform.GetComponent<Button>().SelectButton();

                if (itemTransform.GetComponent<InputField>())
                    itemTransform.GetComponent<InputField>().Focus(true);

                if (itemTransform.GetComponent<Slider>())
                    itemTransform.GetComponent<Slider>().Focus(true);

                if (itemTransform.GetComponent<HorizontalSelector>())
                    itemTransform.GetComponent<HorizontalSelector>().Focus(true);
            }
        }



        public void UnselectItem(int i)
        {
            if (transform.childCount <= i || i < 0)
                return;

            if (transform.GetChild(i).GetComponent<Button>())
            {
                if (transform.GetChild(i).GetComponent<Button>().interactable)
                    transform.GetChild(i).GetComponent<Button>().UnselectButton();
                else
                    transform.GetChild(i).GetComponent<Button>().Uninteractable();
            }

            if (transform.GetChild(i).GetComponent<InputField>())
            {
                transform.GetChild(i).GetComponent<InputField>().Focus(false);
            }

            if (transform.GetChild(i).gameObject.GetComponent<Slider>())
            {
                if (transform.GetChild(i).gameObject.GetComponent<Slider>().interactable)
                    transform.GetChild(i).gameObject.GetComponent<Slider>().Focus(false);
                else
                    transform.GetChild(i).gameObject.GetComponent<Slider>().Uninteractable();
            }

            if (transform.GetChild(i).gameObject.GetComponent<HorizontalSelector>())
            {
                if (transform.GetChild(i).gameObject.GetComponent<HorizontalSelector>().interactable)
                    transform.GetChild(i).gameObject.GetComponent<HorizontalSelector>().Focus(false);
            }

        }

        public void UnselectEverything()
        {
            selectedItem = transform.childCount;
            UpdateList();
            for (int i = 0; i < transform.childCount; i++)
            {
                UnselectItem(i);
            }
        }

        /// <summary>
        /// Keeping the selected item value means the previously selected item can still be pressed after selected and scrolling via keyboard continues from previously selected one instead of starting from 0
        /// </summary>
        public void UnselectEverythingDontChangeSelectedItemValue()
        {
            UpdateList();
            for (int i = 0; i < transform.childCount; i++)
            {
                UnselectItem(i);
            }
        }

        public void UnselectEverythingExceptSelected()
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                if (i != selectedItem)
                    UnselectItem(i);
            }
        }

        public void PressSelectedItem()
        {
            if (selected)
                PresstItem(selectedItem);
        }

        public void PresstItem(int i)
        {
            if (transform.childCount > i)
            {
                pressed = true;

                //if (audioSource && itemSelectionSoundEffect)
                //    audioSource.PlayOneShot(itemSelectionSoundEffect);
                returnToSelectedTime = Time.time + holdPressedVisualFor;
                AlertPressedUIItem();
            }
        }

        /// <summary>
        /// Reapplies all variables/style choices, updating them.
        /// </summary>
        public void UpdateStyle()
        {
            //selectedItem = transform.childCount;
            UpdateList();
            for (int i = 0; i < transform.childCount; i++)
            {
                if (selectedItem != i)
                    UnselectItem(i);
            }
            AlertSelectedItem(selectedItem);
        }
        #endregion Public




        #region Navigation
        private void Scrolled()
        {
            selected = true;
            SelectItem(selectedItem);
            AlertSelectedItem(selectedItem);

            if (selectedItem != previousSelection)
                UnselectItem(previousSelection);
        }
        public void ScrollUp()
        {
            //Debug.Log("Scrolled down");

            previousSelection = selectedItem;

            selectedItem--;
            if (selectedItem < 0)
                selectedItem = transform.childCount - 1;

            while (!IsItemSelectable(selectedItem) && transform.childCount > 0 && counterToCheckIfAllItemsAreDisabledToAvoidInfiniteLoop < transform.childCount)
            {
                counterToCheckIfAllItemsAreDisabledToAvoidInfiniteLoop++;

                selectedItem--;
                if (selectedItem < 0)
                    selectedItem = transform.childCount - 1;
            }
            counterToCheckIfAllItemsAreDisabledToAvoidInfiniteLoop = 0;

            Scrolled();
        }
        public void ScrollDown()
        {
            //Debug.Log("Scrolled up");

            previousSelection = selectedItem;

            selectedItem++;
            if (selectedItem > transform.childCount - 1)
                selectedItem = 0;

            while (!IsItemSelectable(selectedItem) && transform.childCount > 0 && counterToCheckIfAllItemsAreDisabledToAvoidInfiniteLoop < transform.childCount)
            {
                counterToCheckIfAllItemsAreDisabledToAvoidInfiniteLoop++;

                selectedItem++;
                if (selectedItem > transform.childCount - 1)
                    selectedItem = 0;
            }
            counterToCheckIfAllItemsAreDisabledToAvoidInfiniteLoop = 0;

            Scrolled();
        }

        public void ScrollLeft()
        {
            if (IsItemSelectable(selectedItem) && transform.childCount > 0)
            {
                if (transform.GetChild(selectedItem).GetComponent<HorizontalSelector>())
                    transform.GetChild(selectedItem).GetComponent<HorizontalSelector>().Decrease();
                else if (transform.GetChild(selectedItem).GetComponent<Slider>())
                    transform.GetChild(selectedItem).GetComponent<Slider>().DecreaseValue();
            }
        }
        public void ScrollRight()
        {
            if (IsItemSelectable(selectedItem) && transform.childCount > 0)
            {
                if (transform.GetChild(selectedItem).GetComponent<HorizontalSelector>())
                    transform.GetChild(selectedItem).GetComponent<HorizontalSelector>().Increase();
                else if (transform.GetChild(selectedItem).GetComponent<Slider>())
                    transform.GetChild(selectedItem).GetComponent<Slider>().IncreaseValue();
            }
        }
        #endregion Navigation



        /// <summary>
        /// Used by ScrollList() method only
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        private bool IsItemSelectable(int i)
        {
            if (transform.childCount > i)
            //if (transform.childCount > i && i >= 0)
            {
                Transform target = transform.GetChild(i);

                if (!target.gameObject.activeSelf)
                    return false;

                //it's a horizontal selector
                if (target.GetComponent<HorizontalSelector>())
                    return target.GetComponent<HorizontalSelector>().interactable;

                //it's a button
                else if (target.GetComponent<Button>())
                    return target.GetComponent<Button>().interactable;

                //it's a input Field
                else if (target.GetComponent<InputField>())
                    return target.GetComponent<InputField>().interactable;

                //it's a slider
                else if (target.gameObject.GetComponent<Slider>())
                    return target.GetComponent<Slider>().interactable;


            }

            return false;
        }

        void AlertPressedUIItem()
        {
            if (transform.GetChild(selectedItem).GetComponent<Button>())
                transform.GetChild(selectedItem).GetComponent<Button>().PressButtonDontCallList();
        }

        void SelectTheFirstSelectableItem()
        {
            selected = true;

            if (selectedItem > transform.childCount - 1)
                selectedItem = 0;

            while (!IsItemSelectable(selectedItem) && counterToCheckIfAllItemsAreDisabledToAvoidInfiniteLoop < transform.childCount)
            {
                counterToCheckIfAllItemsAreDisabledToAvoidInfiniteLoop++;

                selectedItem++;
                if (selectedItem > transform.childCount - 1)
                    selectedItem = 0;
            }
            counterToCheckIfAllItemsAreDisabledToAvoidInfiniteLoop = 0;

            SelectItem(selectedItem);
            AlertSelectedItem(selectedItem);
        }








        /// <summary>
        /// Create an empty effect and adds it to MText_ModuleContainer List
        /// </summary>
        public void EmptyEffect(List<ModuleContainer> moduleList)
        {
#if UNITY_EDITOR
            Undo.RecordObject(this, "Update list");
#endif
            ModuleContainer module = new ModuleContainer();
            moduleList.Add(module);
        }


#if UNITY_EDITOR
        /// <summary>
        /// Editor only
        /// </summary>
        public void LoadDefaultSettings()
        {
            AssetSettings settings = StaticMethods.VerifySettings(null);

            if (settings)
            {
                NormalTextSize = settings.defaultListNormalTextSize;
                NormalTextMaterial = settings.defaultListNormalTextMaterial;
                NormalBackgroundMaterial = settings.defaultListNormalBackgroundMaterial;

                SelectedTextSize = settings.defaultListSelectedTextSize;
                SelectedTextMaterial = settings.defaultListSelectedTextMaterial;
                SelectedBackgroundMaterial = settings.defaultListSelectedBackgroundMaterial;

                PressedTextSize = settings.defaultListPressedTextSize;
                PressedTextMaterial = settings.defaultListPressedTextMaterial;
                PressedBackgroundMaterial = settings.defaultListPressedBackgroundMaterial;

                DisabledTextSize = settings.defaultListDisabledTextSize;
                DisabledTextMaterial = settings.defaultListDisabledTextMaterial;
                DisabledBackgroundMaterial = settings.defaultListDisabledBackgroundMaterial;
            }
        }
#endif
    }
}