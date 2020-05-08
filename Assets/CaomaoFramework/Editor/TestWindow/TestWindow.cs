
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;

namespace  UnityEditor
{
    internal class OtherTestWindow : OdinEditorWindow
    {
        private static OtherTestWindow window;
        private bool m_PackableListExpanded;
        private GUIContent packableListLabel;
        private ReorderableList m_PackableList;
        private List<Object> m_paths = new List<Object>();
        [MenuItem("CaomaoTools/Test窗口")]
        public static void OpenWindow()
        {
            window = GetWindow<OtherTestWindow>();
            window.position = GUIHelper.GetEditorWindowRect().AlignCenter(800, 600);
        }

        protected override void Initialize()
        {
            base.Initialize();
            this.packableListLabel = new GUIContent("对象拾取器");
            this.m_PackableList = new ReorderableList(this.m_paths, typeof(string), true, false, true, true);

        }
        [AssetSelector]
        private void AddPackable(ReorderableList list)
        {
            //ObjectSelector.get.Show(null, typeof(Object), null, false);
            //ObjectSelector.get.searchFilter = "t:sprite t:texture2d t:folder";
            //ObjectSelector.get.objectSelectorID = styles.packableSelectorHash;
        }



        protected override void OnGUI()
        {
            base.OnGUI();
            Rect rect = EditorGUILayout.GetControlRect();
            m_PackableListExpanded = EditorGUI.Foldout(rect, m_PackableListExpanded, this.packableListLabel, true);
            if (this.m_PackableListExpanded)
            {
                EditorGUI.indentLevel++;
                this.m_PackableList.DoLayoutList();
                EditorGUI.indentLevel--;
            }
        }
    }
}

