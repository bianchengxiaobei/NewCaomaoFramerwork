using UnityEditor;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using System.IO;
using UnityEngine;
namespace CaomaoFramework.LocalizationEditor
{
    public class CaomaoLocalizationWindow : OdinMenuEditorWindow
    {
        public static CaomaoLocalizationWindow Window;
        private CaomaoLocalizationOperator operatorIns;
        [MenuItem("CaomaoTools/本地化窗口")]
        private static void OpenWindow()
        {
            Window = GetWindow<CaomaoLocalizationWindow>();
            Window.position = GUIHelper.GetEditorWindowRect().AlignCenter(800, 600);
        }
        protected override void Initialize()
        {
            base.Initialize();
            operatorIns = new CaomaoLocalizationOperator(new CaomaoHeader("国际本地化"));
        }
        protected override OdinMenuTree BuildMenuTree()
        {
            var tree = new OdinMenuTree(true);
            var customMenuStyle = new OdinMenuStyle
            {
                BorderPadding = 0f,
                AlignTriangleLeft = true,
                TriangleSize = 16f,
                TrianglePadding = 0f,
                Offset = 20f,
                Height = 23,
                IconPadding = 0f,
                BorderAlpha = 0.323f
            };
            tree.DefaultMenuStyle = customMenuStyle;
            tree.Config.DrawSearchToolbar = true;
            tree.AddObjectAtPath("本地化操作", this.operatorIns);
            tree.AddAllAssetsAtPath("本地化ScriptableObject文件",
      this.operatorIns.SBFolderPath, true, false);
            tree.AddAllAssetsAtPath("本地化Excel文件",
                this.operatorIns.ExcelFolderPath, true, false);
            tree.AddAssetAtPath("脚本常量模板", this.operatorIns.TemplateFilePath);
            tree.EnumerateTree().AddThumbnailIcons();
            return tree;
        }
    }
}
