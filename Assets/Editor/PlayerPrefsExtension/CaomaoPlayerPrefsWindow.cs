using Microsoft.Win32;
using PlistCS;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
namespace CaomaoFramework.PlayerPrefsExt
{
    public enum EOpptionType
    {
        新建 = 1,
    }
    public class CaomaoPlayerPrefsWindow : OdinEditorWindow
    {
        private static CaomaoPlayerPrefsWindow Window;
        private string[] m_arraykey;
        private List<CaomaoPlayerPrefsDataBase> AllPlayerPrefsData = new List<CaomaoPlayerPrefsDataBase>();
        private const string Unity_Graphics_Quality = "UnityGraphicsQuality";
        private const string Unity_Player_SessionId = "unity.player_sessionid";
        private const string Unity_Player_SessionCount = "unity.player_session_count";
        private const string Unity_Cloud_UserId = "unity.cloud_userid";
        private static GUIStyle btnStyle;
        private GUIContent m_optionGui;
        private GUIContent m_selectAll;
        private GUIContent m_cancelSelectAll;
        private GUIContent m_sort;
        private CaomaoPlayerPrefsOptionSeletor m_oOptionSeletor = new CaomaoPlayerPrefsOptionSeletor();
        private const float ToolbarHeight = 21f;
        private const float ToolbarMeunWidth = 100;
        private EPlayerPrefsType m_eDataFilterType = EPlayerPrefsType.None;
        private string m_sSearchKeyName = "";
        private ICaomaoHeader Header;
        public static int ModifedIndex = int.MinValue;
        private bool m_bRefresh = false;
        [MenuItem("CaomaoTools/PlayerPrefsWindow")]
        public static void OpenWindow()
        {
            Window = GetWindow<CaomaoPlayerPrefsWindow>();
            Window.position = GUIHelper.GetEditorWindowRect().AlignCenter(1000, 600);
        }
        protected override void Initialize()
        {
            base.Initialize();
            this.LoadPlayerPrefs();
            this.m_optionGui = new GUIContent("选项");
            this.m_selectAll = new GUIContent("全选");
            this.m_cancelSelectAll = new GUIContent("不选择");
            this.m_sort = new GUIContent("排序");
            btnStyle = new GUIStyle(EditorStyles.toolbarDropDown);
            btnStyle.fixedHeight = 21f;
            btnStyle.stretchHeight = false;
            m_oOptionSeletor.EnableSingleClickToSelect();
            m_oOptionSeletor.SelectionConfirmed += delegate (IEnumerable<int> types)
            {
                var type = (EOpptionType)types.FirstOrDefault<int>();
                switch (type)
                {
                    case EOpptionType.新建:

                        break;
                }
            };
            this.Header = new CaomaoCallbackHeader("PlayerPrefs工具",this.DrawOpeartorPlayerPrefsData);
        }
        protected override void OnGUI()
        {
            this.Header.Draw(this.position.width,this.position.height);
            this.DrawFirstToolbar();
            base.OnGUI();
        }
        protected override void DrawEditor(int index)
        {
            //this.DrawOpeartorPlayerPrefsData();
            this.DrawPlayerPrefsData();
            GUILayout.FlexibleSpace();
        }
        private OdinSelector<int> SelectType(Rect arg)
        {          
            m_oOptionSeletor.SetSelection(1);
            m_oOptionSeletor.ShowInPopup(new Rect(0f, ToolbarHeight, ToolbarMeunWidth, 0f));
            return m_oOptionSeletor;
        }
        private void DrawFirstToolbar()
        {
            //选项
            GUILayout.Space(1);           
            Rect rect = GUILayoutUtility.GetRect(0f, ToolbarHeight, SirenixGUIStyles.ToolbarBackground);
            rect = rect.SetHeight(ToolbarHeight);
            var opptionWidth = SirenixGUIStyles.LeftAlignedCenteredLabel.CalcSize(this.m_optionGui).x * 2f;
            Rect leftRect = rect.AlignLeft(opptionWidth);
            OdinSelector<int>.DrawSelectorDropdown(leftRect, this.m_optionGui, this.SelectType, btnStyle); 
            //搜索
            Rect filterRect = rect.AlignRight(opptionWidth * 2);
            Rect searchRect = rect.SetXMax(filterRect.xMin).SetXMin(opptionWidth);
            searchRect = searchRect.HorizontalPadding(5f).AlignMiddle(16);
            this.m_sSearchKeyName = SirenixEditorGUI.SearchField(searchRect, this.m_sSearchKeyName);
            //数据类型过滤
            EditorGUI.BeginChangeCheck();
            this.m_eDataFilterType = EnumSelector<EPlayerPrefsType>.DrawEnumField(filterRect, null, new GUIContent("Type Filter"), this.m_eDataFilterType, CaomaoPlayerPrefsWindow.btnStyle);
            if (EditorGUI.EndChangeCheck())
            {
                EditorPrefs.SetInt("CaomaoPlayerPrefsWindow.m_eDataFilterType", (int)this.m_eDataFilterType);
            }
        }
        private void DrawOpeartorPlayerPrefsData()
        {
            if (this.m_bRefresh == false)
            {
                //如果没有数据直接返回
                return;
            }
            SirenixEditorGUI.BeginHorizontalToolbar(22f, 4); 
            if (SirenixEditorGUI.ToolbarButton(this.m_selectAll, false))
            {

            }
            if (SirenixEditorGUI.ToolbarButton(this.m_cancelSelectAll, false))
            {

            }
            if (SirenixEditorGUI.ToolbarButton(this.m_sort, false))
            {

            }
            if (SirenixEditorGUI.ToolbarButton(CaomaoEditorIcon.SaveGUIContentSmall))
            {

            }          
            SirenixEditorGUI.EndHorizontalToolbar();
        }
        private void DrawPlayerPrefsData()
        {
            foreach (var data in this.AllPlayerPrefsData)
            {
                data.Draw();
            }
        }
        public void LoadPlayerPrefs()
        {
            this.m_bRefresh = false;
            var platform = CaomaoEditorHelper.GetDevelopPlatform();
            if (platform == EDevPlatformType.Windows)
            {
                RegistryKey unityKey = Registry.CurrentUser.CreateSubKey(
                    "Software\\Unity\\UnityEditor\\" + PlayerSettings.companyName + "\\" + PlayerSettings.productName);
                this.m_arraykey = unityKey.GetValueNames();
            }
            else if(platform == EDevPlatformType.Mac) 
            {
                string plistPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal) + "/Library/Preferences/unity." + PlayerSettings.companyName + "." + PlayerSettings.productName + ".plist";
                if (File.Exists(plistPath))
                {
                    FileInfo fi = new FileInfo(plistPath);
                    Dictionary<string, object> plist = (Dictionary<string, object>)Plist.readPlist(fi.FullName);
                    this.m_arraykey = new string[plist.Count];
                    plist.Keys.CopyTo(this.m_arraykey, 0);
                }
            }
            if (this.m_arraykey != null && this.m_arraykey.Length > 0)
            {
                this.m_bRefresh = true;
                this.AllPlayerPrefsData.Clear();
                int index = 0;
                for (int i = 0; i < this.m_arraykey.Length; i++)
                {
                    var keyContent = this.m_arraykey[i];
                    var keyName = keyContent.Substring(0, keyContent.LastIndexOf("_"));
                    if (keyName == Unity_Graphics_Quality || keyName == Unity_Player_SessionId
                        || keyName == Unity_Player_SessionCount || keyName == Unity_Cloud_UserId)
                    {
                        continue;
                    }
                    var data = CaomaoPlayerPrefsDataFactory.CreateCaomaoPlayerPrefsData(keyName,index++);
                    this.AllPlayerPrefsData.Add(data);
                }
            }
            CaomaoPlayerPrefsDataBase.ModifyCallback = this.SelectCallback;
        }
        public void SelectCallback(int index)
        {
            if (ModifedIndex >= 0)
            {
                if (ModifedIndex != index)
                {
                    this.AllPlayerPrefsData[ModifedIndex].CancelModify();
                    ModifedIndex = index;                    
                }
            }
            else
            {
                ModifedIndex = index;
            }
        }
    }
}
