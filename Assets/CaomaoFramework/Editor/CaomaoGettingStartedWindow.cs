using System;
using System.IO;
using System.Collections.Generic;
using Sirenix.OdinInspector.Editor;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;
public class CaomaoGettingStartedWindow : OdinEditorWindow
{
    static GUIStyle sectionTitleLabelStyle;
    static GUIStyle cardTitleStyle;
    static GUIStyle cardStylePadding;
    static GUIStyle cardStyle;
    static GUIStyle headerBtnStyle;
    private Vector2 scrollPoss;
    private float width;
    private Rect footerRect;
    private Rect currSectionRect;
    private int currBtnCount;
    private SlidePageNavigationHelper<Page> pager;
    private ICaomaoHeader Header;
    private Page MainPage;
    //public const string CaomaoAssetsPath = "Assets/CaomaoFramework/";
    [MenuItem("CaomaoTools/Getting Started")]
    public static void ShowWindow()
    {
        var win = GetWindow<CaomaoGettingStartedWindow>();
        win.position = GUIHelper.GetEditorWindowRect().AlignCenter(715f, 660f);
        win.titleContent = new GUIContent("草帽游戏框架工具引导");
    }

    [MenuItem("CaomaoTools/打开Persistent文件夹")]
    public static void Open()
    {
        var path = Application.persistentDataPath;
        Application.OpenURL(path);
    }

    protected override void Initialize()
    {
        this.pager = new SlidePageNavigationHelper<Page>();
        this.pager.TabGroup.ExpandHeight = false;     
        this.InitMainPage();
        this.Header = new CaomaoCallbackHeader("Caomao Framework",this.DrawPaging);
        this.pager.PushPage(MainPage, "预览");
        this.WindowPadding = Vector4.zero;
        this.InitStyles();
    }
    protected override void OnGUI()
    {
        Rect rect = this.Header.Draw(this.position.width,this.position.height);
        SirenixEditorGUI.DrawBorders(rect, 0, 0, 0, 1, SirenixGUIStyles.BorderColor, true);
        this.DrawPages();
    }
    private void InitStyles()
    {
        //Setion头部
        var temp = new GUIStyle(SirenixGUIStyles.SectionHeaderCentered);
        temp.fontSize = 17;
        temp.margin = new RectOffset(0, 0, 10, 10);
        CaomaoGettingStartedWindow.sectionTitleLabelStyle = temp;
        //Card头部
        temp = new GUIStyle(SirenixGUIStyles.SectionHeader);
        temp.fontSize = 15;
        temp.fontStyle = FontStyle.Bold;
        temp.margin = new RectOffset(0, 0, 0, 4);
        CaomaoGettingStartedWindow.cardTitleStyle = temp;
        //card部分
        temp = new GUIStyle("sv_iconselector_labelselection");
        temp.padding = new RectOffset(15, 15, 15, 15);
        temp.margin = new RectOffset(0, 0, 0, 0);
        temp.stretchHeight = false;
        CaomaoGettingStartedWindow.cardStyle = temp;
        //CardPadding
        temp = new GUIStyle();
        temp.padding = new RectOffset(15, 15, 15, 15);
        temp.stretchHeight = false;
        CaomaoGettingStartedWindow.cardStylePadding = temp;
    } 
    private void DrawPaging()
    {
        Rect rect = GUILayoutUtility.GetRect(0f, 25f);
        this.pager.DrawPageNavigation(rect);
    }
    private void DrawPages()
    {
        GUIHelper.PushLabelWidth(10f);
        this.scrollPoss = EditorGUILayout.BeginScrollView(this.scrollPoss, GUILayoutOptions.ExpandHeight(true));
        Rect rect = EditorGUILayout.BeginVertical();
        if (this.width == 0f || Event.current.type == EventType.Repaint)
        {
            this.width = rect.width;
        }
        this.pager.BeginGroup();
        foreach (var page in this.pager.EnumeratePages)
        {
            if (page.BeginPage())
            {
                this.DrawPage(page.Value);
            }
            page.EndPage();
        }
        this.pager.EndGroup();
        EditorGUILayout.EndVertical();
        EditorGUILayout.EndScrollView();
        GUIHelper.PopLabelWidth();
    }
    private void DrawPage(Page page)
    {
        for (int i = 0; i < page.Sections.Count; i++)
        {
            var section = page.Sections[i];
            int colNum = (this.position.width < 470f) ? 1 : section.ColCount;
            if (string.IsNullOrEmpty(section.Title) == false)
            {
                GUILayout.Label(new GUIContent(section.Title), sectionTitleLabelStyle);
            }
            if (section.Cards.Count != 0)
            {
                bool flag = false;
                for (int j = 0; j < section.Cards.Count; j++)
                {
                    if (j % colNum == 0)
                    {
                        if (j != 0 && colNum != 0)
                        {
                            EditorGUILayout.EndHorizontal();
                            GUILayout.Space(10f);
                        }
                        Rect rect = EditorGUILayout.BeginHorizontal(new GUIStyle
                        {
                            padding = new RectOffset(5, 4, 0, 0)
                        });
                        this.currSectionRect = rect;
                        flag = true;
                    }
                    if (colNum == 0)
                    {
                        GUILayout.FlexibleSpace();
                    }
                    Rect cardBoxRect = EditorGUILayout.BeginVertical(cardStylePadding,
                        GUILayoutOptions.Width(this.width / (float)colNum - 12f));
                    if (Event.current.type == EventType.Repaint)
                    {
                        GUIHelper.PushColor(new Color(1f, 1f, 1f, EditorGUIUtility.isProSkin ? 0.25f : 0.45f), false);
                        cardStyle.Draw(cardBoxRect, GUIContent.none, 0);
                        GUIHelper.PopColor();
                    }
                    this.DrawCard(section.Cards[j]);
                    EditorGUILayout.EndVertical();
                    if (j % colNum == 0)
                    {
                        GUILayout.FlexibleSpace();
                    }
                }
                if (flag)
                {
                    EditorGUILayout.EndHorizontal();
                }
            }
            GUILayout.Space(8f);
            if (i != page.Sections.Count - 1)
            {
                SirenixEditorGUI.DrawThickHorizontalSeparator(10f, 0f);
            }
        }
    }
    private void DrawCard(Card card)
    {
        if (card.Title != null)
        {
            GUILayout.Label(card.Title, cardTitleStyle);
        }
        if (card.Description != null)
        {
            GUILayout.Label(card.Description, SirenixGUIStyles.MultiLineLabel);
        }
        if (card.Title != null || card.Description != null)
        {
            GUILayout.Space(5f);
        }
        this.currBtnCount = 0;
        bool needImport = string.IsNullOrEmpty(card.ModuleFolderPath) && !File.Exists(card.ModuleFolderPath);
        if (needImport)
        {
            GUIHelper.PushGUIEnabled(false);
        }
        for (int i = 0; i < card.CustomActions.Count; i++)
        {
            var action = card.CustomActions[i];
            if (string.IsNullOrEmpty(action.Name) == false)
            {
                if (this.Button(action.Name))
                {
                    var m = typeof(CaomaoGettingStartedWindow).GetMethod(action.Action);
                    if (m != null)
                    {
                        m.Invoke(this, null);
                    }
                    else
                    {
                        Debug.LogError($"No Method{action.Name}");
                    }
                }
            }
            else
            {
                Debug.LogError("card == null");
            }
        }
        if (card.SubPage != null && !string.IsNullOrEmpty(card.SubPage.Title) && this.Button(card.SubPage.Title))
        {
            this.pager.PushPage(card.SubPage, card.SubPage.Title);
        }
        if (needImport)
        {
            GUIHelper.PopGUIEnabled();
        }
        if (!needImport && string.IsNullOrEmpty(card.Package) == false)
        {
            bool enabled = File.Exists(card.Package);
            string txt = "Import " + Path.GetFileNameWithoutExtension(card.Package);
            GUIHelper.PushGUIEnabled(enabled);
            if (this.Button(txt))
            {
                UnityEditorEventUtility.DelayAction(()=>
                {
                    AssetDatabase.ImportPackage(card.Package, true);
                });
            }
            GUIHelper.PopGUIEnabled();
        }
    }
    private bool Button(string txt)
    {
        Rect rect = GUILayoutUtility.GetRect(0f, 26f);
        rect.y = this.currSectionRect.yMax - 15f;
        float y = rect.y;
        this.currBtnCount += 1;
        rect.y = y - (float)(this.currBtnCount * 26);
        rect.height = 22f;
        return GUI.Button(rect, txt, SirenixGUIStyles.Button);
    }
    private void InitMainPage()
    {
        //this.MainPage = new Page();
        //this.MainPage.Title = "草帽游戏框架";
        ////基本框架模块Setion
        //Section baseModule = new Section();
        //baseModule.Title = "基本模块";
        var data = AssetDatabase.LoadAssetAtPath<ScriptableObject>("Assets/StartPageData.asset") as StartPageData;
        this.MainPage =  data.mainPage;
    }
    public void A()
    {
        Debug.Log("A");
    }
}
[HideReferenceObjectPicker]
[Serializable]
public class Page
{
    public string Title;
    public List<Section> Sections = new List<Section>();
}
[HideReferenceObjectPicker]
[Serializable]
public class Section
{
    [FoldoutGroup("$Title", 0)]
    public string Title;
    [FoldoutGroup("$Title", 0)]
    public List<Card> Cards = new List<Card>();
    public int ColCount = 2;
}
[HideReferenceObjectPicker]
[Serializable]
public class Card
{
    [FoldoutGroup("$Title", 0)]
    public string Title;
    [Multiline]
    [FoldoutGroup("$Title", 0)]
    public string Description;
    [FoldoutGroup("$Title", 0)]
    public string ModuleFolderPath;
    [FoldoutGroup("$Title", 0)]
    public string Package;
    [FoldoutGroup("$Title", 0)]
    public Page SubPage = null;
    [FoldoutGroup("$Title", 0)]
    public List<BtnAction> CustomActions = new List<BtnAction>();
}
[HideReferenceObjectPicker]
[Serializable]
public class BtnAction
{
    public BtnAction(string name, string action)
    {
        this.Name = name;
        this.Action = action;
    }
    public string Name;
    public string Action;
}