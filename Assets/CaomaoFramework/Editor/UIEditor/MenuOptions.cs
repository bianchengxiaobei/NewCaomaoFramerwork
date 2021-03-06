using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using UnityEditor.Experimental.SceneManagement;
using CaomaoFramework;
namespace CaomaoFramework.UIEditor
{
    public static class MenuOptions
    {
        private const string kUILayerName = "UI";

        private const string kStandardSpritePath = "UI/Skin/UISprite.psd";
        private const string kBackgroundSpritePath = "UI/Skin/Background.psd";
        private const string kInputFieldBackgroundPath = "UI/Skin/InputFieldBackground.psd";
        private const string kKnobPath = "UI/Skin/Knob.psd";
        private const string kCheckmarkPath = "UI/Skin/Checkmark.psd";
        private const string kDropdownArrowPath = "UI/Skin/DropdownArrow.psd";
        private const string kMaskPath = "UI/Skin/UIMask.psd";


        private const float kWidth = 160f;
        private const float kThickHeight = 30f;
        private const float kThinHeight = 20f;
        private static Vector2 s_ThickElementSize = new Vector2(kWidth, kThickHeight);
        private static Vector2 s_VertialTabSize = new Vector2(100,500);//创建Vertial Tab的时候大小
        private static Vector2 s_HorTabSize = new Vector2(500, 100);//创建Hor Tab的时候大小

        private static Vector2 s_VertialTabButtonSize = new Vector2(80,0);
        private static Vector2 s_HorTabButtonSize = new Vector2(0, 80);

        private static float s_VertialTabLayoutHeight = 70f;
        private static float s_HorizontalTabLayoutWidth = 70f;



        private static Color s_DefaultSelectableColor = new Color(1f, 1f, 1f, 1f);
        private static Color s_TextColor = new Color(50f / 255f, 50f / 255f, 50f / 255f, 1f);

        static private DefaultControls.Resources s_StandardResources;

        static private DefaultControls.Resources GetStandardResources()
        {
            if (s_StandardResources.standard == null)
            {
                s_StandardResources.standard = AssetDatabase.GetBuiltinExtraResource<Sprite>(kStandardSpritePath);
                s_StandardResources.background = AssetDatabase.GetBuiltinExtraResource<Sprite>(kBackgroundSpritePath);
                s_StandardResources.inputField = AssetDatabase.GetBuiltinExtraResource<Sprite>(kInputFieldBackgroundPath);
                s_StandardResources.knob = AssetDatabase.GetBuiltinExtraResource<Sprite>(kKnobPath);
                s_StandardResources.checkmark = AssetDatabase.GetBuiltinExtraResource<Sprite>(kCheckmarkPath);
                s_StandardResources.dropdown = AssetDatabase.GetBuiltinExtraResource<Sprite>(kDropdownArrowPath);
                s_StandardResources.mask = AssetDatabase.GetBuiltinExtraResource<Sprite>(kMaskPath);
            }
            return s_StandardResources;
        }



        [MenuItem("GameObject/UI/普通红点Button",false)]
        public static void AddCUIRedPointButton(MenuCommand menuCommand) 
        {
            GameObject go = CreateSimpleRedButton(GetStandardResources());
            PlaceUIElementRoot(go, menuCommand);
        }


        [MenuItem("GameObject/UI/数字红点Button", false)]
        public static void AddCUINumberRedPointButton(MenuCommand menuCommand) 
        {
            var go = CreateNumberRedButton(GetStandardResources());
            PlaceUIElementRoot(go, menuCommand);
        }

        [MenuItem("GameObject/UI/Vertical Tab（带Text）", false)]
        public static void AddTextTab(MenuCommand menuCommand)
        {
            var go = CreateVerticalTab(GetStandardResources(),true);
            PlaceUIElementRoot(go, menuCommand);
        }
        [MenuItem("GameObject/UI/Vertical Tab(不带Text)", false)]
        public static void AddNoTextTab(MenuCommand menuCommand)
        {
            var go = CreateVerticalTab(GetStandardResources(),false);
            PlaceUIElementRoot(go, menuCommand);
        }
        [MenuItem("GameObject/UI/Horizontal Tab(带Text)", false)]
        public static void AddHorTextTab(MenuCommand menuCommand)
        {
            var go = CreateHorTab(GetStandardResources(), true);
            PlaceUIElementRoot(go, menuCommand);
        }

        [MenuItem("GameObject/UI/Horizontal Tab(不带Text)")]
        public static void AddHorNoTextTab(MenuCommand menuCommand)
        {
            var go = CreateHorTab(GetStandardResources(), false);
            PlaceUIElementRoot(go, menuCommand);
        }




        public static GameObject CreateHorTab(DefaultControls.Resources resources, bool bText)
        {
            GameObject root = CreateUIElementRoot(CaomaoUIEditorConfig.Instance.tabName, s_HorTabSize);
            root.name = "Horizontal Tab";
            //添加Image
            var rootImage = root.AddComponent<Image>();
            rootImage.raycastTarget = false;
            //添加Tab
            var tab = root.AddComponent<CUITab>();
            //添加VerticalLayout
            var vertical = root.AddComponent<HorizontalLayoutGroup>();
            //vertical.childControlHeight = true;
            vertical.childControlWidth = true;
            vertical.childForceExpandWidth = false;
            vertical.childForceExpandHeight = true;
            vertical.childAlignment = TextAnchor.MiddleCenter;

            //创建子TabButton
            for (int i = 0; i < 6; i++)
            {
                var tabButton = CreateUIElementRoot(CaomaoUIEditorConfig.Instance.tabButtonName, s_HorTabButtonSize);
                //添加image
                var tabButtonImage = tabButton.AddComponent<Image>();
                tabButtonImage.color = Color.red;
                if (tabButtonImage.color.a == 0)
                {
                    var tempRedColor = Color.red;
                    tempRedColor.a = 1;
                    tabButtonImage.color = tempRedColor;
                }
                tabButtonImage.raycastTarget = true;
                //添加highSpriteToggle
                var highSprite = tabButton.AddComponent<CUIHighSpriteTabToggle>();
                highSprite.Index = i;
                highSprite.Parent = tab;
                highSprite.transition = Selectable.Transition.None;
                //添加LayoutElement
                var layoutElement = tabButton.AddComponent<LayoutElement>();
                //layoutElement.minHeight = s_VertialTabLayoutHeight;
                layoutElement.minWidth = s_HorizontalTabLayoutWidth;
                tabButton.name = $"Tab{i.ToString()}";
                if (bText)
                {
                    //创建Text
                    var tabButtonText = new GameObject(CaomaoUIEditorConfig.Instance.textName);
                    var rectTransform = tabButtonText.AddComponent<RectTransform>();
                    SetParentAndAlign(tabButtonText, tabButton);
                    var text = tabButtonText.AddComponent<Text>();
                    text.text = "Tab" + i.ToString();
                    text.raycastTarget = false;
                    text.alignment = TextAnchor.MiddleCenter;
                    SetDefaultTextValues(text);
                    rectTransform.anchorMin = Vector2.zero;
                    rectTransform.anchorMax = Vector2.one;
                    rectTransform.sizeDelta = Vector2.zero;
                    highSprite.lb_content = text;
                }
                SetParentAndAlign(tabButton, root);
                //highSprite.enabled = true;
            }
            return root;
        }


        /// <summary>
        /// 创建vertical的Tab
        /// </summary>
        /// <param name="resources"></param>
        /// <returns></returns>
        public static GameObject CreateVerticalTab(DefaultControls.Resources resources,bool bText)
        {
            GameObject root = CreateUIElementRoot(CaomaoUIEditorConfig.Instance.tabName, s_VertialTabSize);
            root.name = "Vertical Tab";
            //添加Image
            var rootImage = root.AddComponent<Image>();
            rootImage.raycastTarget = false;
            //添加Tab
            var tab = root.AddComponent<CUITab>();
            //添加VerticalLayout
            var vertical = root.AddComponent<VerticalLayoutGroup>();
            vertical.childControlHeight = true;
            vertical.childForceExpandWidth = true;
            vertical.childForceExpandHeight = false;
            vertical.childAlignment = TextAnchor.MiddleCenter;

            //创建子TabButton
            for (int i = 0; i < 6; i++)
            {
                var tabButton = CreateUIElementRoot(CaomaoUIEditorConfig.Instance.tabButtonName,s_VertialTabButtonSize);
                //添加image
                var tabButtonImage = tabButton.AddComponent<Image>();
                tabButtonImage.color = Color.red;
                if (tabButtonImage.color.a == 0)
                {
                    var tempRedColor = Color.red;
                    tempRedColor.a = 1;
                    tabButtonImage.color = tempRedColor;
                }
                tabButtonImage.raycastTarget = true;
                //添加highSpriteToggle
                var highSprite = tabButton.AddComponent<CUIHighSpriteTabToggle>();
                highSprite.Index = i;
                highSprite.Parent = tab;
                highSprite.transition = Selectable.Transition.None;             
                //添加LayoutElement
                var layoutElement = tabButton.AddComponent<LayoutElement>();
                layoutElement.minHeight = s_VertialTabLayoutHeight;
                //tabButton.name = "Tab" + i.ToString();
                tabButton.name = $"Tab{i.ToString()}";
                if (bText)
                {
                    //创建Text
                    var tabButtonText = new GameObject(CaomaoUIEditorConfig.Instance.textName);
                    var rectTransform = tabButtonText.AddComponent<RectTransform>();
                    SetParentAndAlign(tabButtonText, tabButton);
                    var text = tabButtonText.AddComponent<Text>();
                    text.text = "Tab" + i.ToString();
                    text.raycastTarget = false;
                    text.alignment = TextAnchor.MiddleCenter;
                    SetDefaultTextValues(text);
                    rectTransform.anchorMin = Vector2.zero;
                    rectTransform.anchorMax = Vector2.one;
                    rectTransform.sizeDelta = Vector2.zero;
                    highSprite.lb_content = text;
                }
                SetParentAndAlign(tabButton, root);
                highSprite.enabled = true;
            }
            return root;
        }









        public static GameObject CreateNumberRedButton(DefaultControls.Resources resources)
        {
            GameObject buttonRoot = CreateUIElementRoot(CaomaoUIEditorConfig.Instance.buttonName, s_ThickElementSize);
            //Text
            GameObject childText = new GameObject(CaomaoUIEditorConfig.Instance.textName);
            childText.AddComponent<RectTransform>();
            SetParentAndAlign(childText, buttonRoot);

            Text text = childText.AddComponent<Text>();
            text.text = "数字红点按钮";
            text.raycastTarget = false;
            text.alignment = TextAnchor.MiddleCenter;
            SetDefaultTextValues(text);

            RectTransform textRectTransform = childText.GetComponent<RectTransform>();
            textRectTransform.anchorMin = Vector2.zero;
            textRectTransform.anchorMax = Vector2.one;
            textRectTransform.sizeDelta = Vector2.zero;

            //RedPoint
            GameObject childRedPointImage = new GameObject("sp_redPoint");
            var childRPRectTransform = childRedPointImage.AddComponent<RectTransform>();
            SetParentAndAlign(childRedPointImage, buttonRoot);

            Image redPointImage = childRedPointImage.AddComponent<Image>();
            redPointImage.sprite = resources.knob;
            redPointImage.color = Color.red;
            redPointImage.type = Image.Type.Simple;
            redPointImage.raycastTarget = false;

            childRPRectTransform.anchorMin = Vector2.one;
            childRPRectTransform.anchorMax = Vector2.one;
            childRPRectTransform.sizeDelta = Vector2.one * 20f;

            //RedPointNumberText
            GameObject numberText = new GameObject("lb_number");
            var numberTextRect =  numberText.AddComponent<RectTransform>();
            SetParentAndAlign(numberText, childRedPointImage);
            var numberTextComp = numberText.AddComponent<Text>();
            numberTextComp.text = "1";
            numberTextComp.raycastTarget = false;
            numberTextComp.alignment = TextAnchor.MiddleCenter;
            SetDefaultTextValues(numberTextComp);
            numberTextRect.anchorMin = Vector2.zero;
            numberTextRect.anchorMax = Vector2.one;
            numberTextRect.sizeDelta = Vector2.zero;

            //Button
            Image image = buttonRoot.AddComponent<Image>();
            image.sprite = resources.standard;
            image.type = Image.Type.Sliced;
            image.color = s_DefaultSelectableColor;

            CUINumberRedPointButton bt = buttonRoot.AddComponent<CUINumberRedPointButton>();
            bt.ID = "Root";
            bt.RedPointImage = redPointImage;
            bt.lb_number = numberTextComp;
            SetDefaultColorTransitionValues(bt);

            return buttonRoot;
        }
        public static GameObject CreateSimpleRedButton(DefaultControls.Resources resources)
        {
            GameObject buttonRoot = CreateUIElementRoot(CaomaoUIEditorConfig.Instance.buttonName, s_ThickElementSize);
            //Text
            GameObject childText = new GameObject(CaomaoUIEditorConfig.Instance.textName);
            childText.AddComponent<RectTransform>();
            SetParentAndAlign(childText, buttonRoot);

            Text text = childText.AddComponent<Text>();
            text.text = "红点按钮";
            text.raycastTarget = false;
            text.alignment = TextAnchor.MiddleCenter;
            SetDefaultTextValues(text);

            RectTransform textRectTransform = childText.GetComponent<RectTransform>();
            textRectTransform.anchorMin = Vector2.zero;
            textRectTransform.anchorMax = Vector2.one;
            textRectTransform.sizeDelta = Vector2.zero;

            //RedPoint
            GameObject childRedPointImage = new GameObject("sp_redPoint");
            var childRPRectTransform = childRedPointImage.AddComponent<RectTransform>();
            SetParentAndAlign(childRedPointImage, buttonRoot);

            Image redPointImage = childRedPointImage.AddComponent<Image>();
            redPointImage.sprite = resources.knob;
            redPointImage.color = Color.red;
            redPointImage.type = Image.Type.Simple;
            redPointImage.raycastTarget = false;

            childRPRectTransform.anchorMin = Vector2.one;
            childRPRectTransform.anchorMax = Vector2.one;
            childRPRectTransform.sizeDelta = Vector2.one * 20f;

            //Button
            Image image = buttonRoot.AddComponent<Image>();
            image.sprite = resources.standard;
            image.type = Image.Type.Sliced;
            image.color = s_DefaultSelectableColor;

            CUISimpleRedPointButton bt = buttonRoot.AddComponent<CUISimpleRedPointButton>();
            bt.ID = "Root";
            bt.RedPointImage = redPointImage;
            SetDefaultColorTransitionValues(bt);          

            return buttonRoot;
        }
        private static GameObject CreateUIElementRoot(string name, Vector2 size)
        {
            GameObject child = new GameObject(name);
            RectTransform rectTransform = child.AddComponent<RectTransform>();
            rectTransform.sizeDelta = size;
            return child;
        }
        private static void PlaceUIElementRoot(GameObject element, MenuCommand menuCommand)
        {
            GameObject parent = menuCommand.context as GameObject;
            bool explicitParentChoice = true;
            if (parent == null)
            {
                parent = GetOrCreateCanvasGameObject();
                explicitParentChoice = false;

                // If in Prefab Mode, Canvas has to be part of Prefab contents,
                // otherwise use Prefab root instead.
                PrefabStage prefabStage = PrefabStageUtility.GetCurrentPrefabStage();
                if (prefabStage != null && !prefabStage.IsPartOfPrefabContents(parent))
                    parent = prefabStage.prefabContentsRoot;
            }
            if (parent.GetComponentsInParent<Canvas>(true).Length == 0)
            {
                // Create canvas under context GameObject,
                // and make that be the parent which UI element is added under.
                GameObject canvas = MenuOptions.CreateNewUI();
                canvas.transform.SetParent(parent.transform, false);
                parent = canvas;
            }

            // Setting the element to be a child of an element already in the scene should
            // be sufficient to also move the element to that scene.
            // However, it seems the element needs to be already in its destination scene when the
            // RegisterCreatedObjectUndo is performed; otherwise the scene it was created in is dirtied.
            SceneManager.MoveGameObjectToScene(element, parent.scene);

            Undo.RegisterCreatedObjectUndo(element, "Create " + element.name);

            if (element.transform.parent == null)
            {
                Undo.SetTransformParent(element.transform, parent.transform, "Parent " + element.name);
            }

            GameObjectUtility.EnsureUniqueNameForSibling(element);

            // We have to fix up the undo name since the name of the object was only known after reparenting it.
            Undo.SetCurrentGroupName("Create " + element.name);

            GameObjectUtility.SetParentAndAlign(element, parent);
            if (!explicitParentChoice) // not a context click, so center in sceneview
                SetPositionVisibleinSceneView(parent.GetComponent<RectTransform>(), element.GetComponent<RectTransform>());

            Selection.activeGameObject = element;
        }
        private static void SetPositionVisibleinSceneView(RectTransform canvasRTransform, RectTransform itemTransform)
        {
            SceneView sceneView = SceneView.lastActiveSceneView;

            // Couldn't find a SceneView. Don't set position.
            if (sceneView == null || sceneView.camera == null)
                return;

            // Create world space Plane from canvas position.
            Vector2 localPlanePosition;
            Camera camera = sceneView.camera;
            Vector3 position = Vector3.zero;
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRTransform, new Vector2(camera.pixelWidth / 2, camera.pixelHeight / 2), camera, out localPlanePosition))
            {
                // Adjust for canvas pivot
                localPlanePosition.x = localPlanePosition.x + canvasRTransform.sizeDelta.x * canvasRTransform.pivot.x;
                localPlanePosition.y = localPlanePosition.y + canvasRTransform.sizeDelta.y * canvasRTransform.pivot.y;

                localPlanePosition.x = Mathf.Clamp(localPlanePosition.x, 0, canvasRTransform.sizeDelta.x);
                localPlanePosition.y = Mathf.Clamp(localPlanePosition.y, 0, canvasRTransform.sizeDelta.y);

                // Adjust for anchoring
                position.x = localPlanePosition.x - canvasRTransform.sizeDelta.x * itemTransform.anchorMin.x;
                position.y = localPlanePosition.y - canvasRTransform.sizeDelta.y * itemTransform.anchorMin.y;

                Vector3 minLocalPosition;
                minLocalPosition.x = canvasRTransform.sizeDelta.x * (0 - canvasRTransform.pivot.x) + itemTransform.sizeDelta.x * itemTransform.pivot.x;
                minLocalPosition.y = canvasRTransform.sizeDelta.y * (0 - canvasRTransform.pivot.y) + itemTransform.sizeDelta.y * itemTransform.pivot.y;

                Vector3 maxLocalPosition;
                maxLocalPosition.x = canvasRTransform.sizeDelta.x * (1 - canvasRTransform.pivot.x) - itemTransform.sizeDelta.x * itemTransform.pivot.x;
                maxLocalPosition.y = canvasRTransform.sizeDelta.y * (1 - canvasRTransform.pivot.y) - itemTransform.sizeDelta.y * itemTransform.pivot.y;

                position.x = Mathf.Clamp(position.x, minLocalPosition.x, maxLocalPosition.x);
                position.y = Mathf.Clamp(position.y, minLocalPosition.y, maxLocalPosition.y);
            }

            itemTransform.anchoredPosition = position;
            itemTransform.localRotation = Quaternion.identity;
            itemTransform.localScale = Vector3.one;
        }

        static public GameObject CreateNewUI()
        {
            // Root for the UI
            var root = new GameObject("Canvas");
            root.layer = LayerMask.NameToLayer(kUILayerName);
            Canvas canvas = root.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            root.AddComponent<CanvasScaler>();
            root.AddComponent<GraphicRaycaster>();

            // Works for all stages.
            StageUtility.PlaceGameObjectInCurrentStage(root);
            bool customScene = false;
            PrefabStage prefabStage = PrefabStageUtility.GetCurrentPrefabStage();
            if (prefabStage != null)
            {
                root.transform.SetParent(prefabStage.prefabContentsRoot.transform, false);
                customScene = true;
            }

            Undo.RegisterCreatedObjectUndo(root, "Create " + root.name);

            // If there is no event system add one...
            // No need to place event system in custom scene as these are temporary anyway.
            // It can be argued for or against placing it in the user scenes,
            // but let's not modify scene user is not currently looking at.
            if (!customScene)
                CreateEventSystem(false);
            return root;
        }
        private static void CreateEventSystem(bool select)
        {
            CreateEventSystem(select, null);
        }

        private static void CreateEventSystem(bool select, GameObject parent)
        {
            StageHandle stage = parent == null ? StageUtility.GetCurrentStageHandle() : StageUtility.GetStageHandle(parent);
            var esys = stage.FindComponentOfType<EventSystem>();
            if (esys == null)
            {
                var eventSystem = new GameObject("EventSystem");
                if (parent == null)
                    StageUtility.PlaceGameObjectInCurrentStage(eventSystem);
                else
                    GameObjectUtility.SetParentAndAlign(eventSystem, parent);
                esys = eventSystem.AddComponent<EventSystem>();
                eventSystem.AddComponent<StandaloneInputModule>();

                Undo.RegisterCreatedObjectUndo(eventSystem, "Create " + eventSystem.name);
            }

            if (select && esys != null)
            {
                Selection.activeGameObject = esys.gameObject;
            }
        }

        static public GameObject GetOrCreateCanvasGameObject()
        {
            GameObject selectedGo = Selection.activeGameObject;

            // Try to find a gameobject that is the selected GO or one if its parents.
            Canvas canvas = (selectedGo != null) ? selectedGo.GetComponentInParent<Canvas>() : null;
            if (IsValidCanvas(canvas))
                return canvas.gameObject;

            // No canvas in selection or its parents? Then use any valid canvas.
            // We have to find all loaded Canvases, not just the ones in main scenes.
            Canvas[] canvasArray = StageUtility.GetCurrentStageHandle().FindComponentsOfType<Canvas>();
            for (int i = 0; i < canvasArray.Length; i++)
                if (IsValidCanvas(canvasArray[i]))
                    return canvasArray[i].gameObject;

            // No canvas in the scene at all? Then create a new one.
            return MenuOptions.CreateNewUI();
        }
        static bool IsValidCanvas(Canvas canvas)
        {
            if (canvas == null || !canvas.gameObject.activeInHierarchy)
                return false;

            // It's important that the non-editable canvas from a prefab scene won't be rejected,
            // but canvases not visible in the Hierarchy at all do. Don't check for HideAndDontSave.
            if (EditorUtility.IsPersistent(canvas) || (canvas.hideFlags & HideFlags.HideInHierarchy) != 0)
                return false;

            if (StageUtility.GetStageHandle(canvas.gameObject) != StageUtility.GetCurrentStageHandle())
                return false;

            return true;
        }
        private static void SetParentAndAlign(GameObject child, GameObject parent)
        {
            if (parent == null)
                return;

            child.transform.SetParent(parent.transform, false);
            SetLayerRecursively(child, parent.layer);
        }
        private static void SetLayerRecursively(GameObject go, int layer)
        {
            go.layer = layer;
            Transform t = go.transform;
            for (int i = 0; i < t.childCount; i++)
                SetLayerRecursively(t.GetChild(i).gameObject, layer);
        }

        private static void SetDefaultColorTransitionValues(Selectable slider)
        {
            ColorBlock colors = slider.colors;
            colors.highlightedColor = new Color(0.882f, 0.882f, 0.882f);
            colors.pressedColor = new Color(0.698f, 0.698f, 0.698f);
            colors.disabledColor = new Color(0.521f, 0.521f, 0.521f);
        }
        private static void SetDefaultTextValues(Text lbl)
        {
            lbl.color = s_TextColor;
            lbl.font = CaomaoUIEditorConfig.Instance.textFont;
        }
    }
}

