using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.ProjectWindowCallback;

namespace CaomaoFramework.NewbieHelpEditor
{
    public static class CreateNewbieHelpData
    {
        [MenuItem("Assets/Create/CaomaoFramework/NewbieHelp Graph",false)]
        public static void CreateBigNewbieHelpGraph()
        {
            var renameAction = new NewNewbieHelpGraphAction();
            ProjectWindowUtil.StartNameEditingIfProjectWindowExists(0,renameAction,
                $"New NewbieHelp{NewbieEditorConst.NewbieGraphExtension}",null,null);
        }
    }
    /// <summary>
    /// 重新命名指令
    /// </summary>
    public class NewNewbieHelpGraphAction : EndNameEditAction
    {
        public override void Action(int instanceId, string pathName, string resourceFile)
        {
            var graph = new NewbieGraphData();
            Debug.Log("r3r");
            //设置数据
            CaomaoEditorHelper.WriteGraphDataToDisk(pathName, graph);
            //刷新项目数据
            AssetDatabase.Refresh();
            //选择搞物体
            UnityEngine.Object obj = AssetDatabase.LoadAssetAtPath<Object>(pathName);
            Selection.activeObject = obj;
        }
    }
}
