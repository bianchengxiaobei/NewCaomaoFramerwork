using System;
using System.Collections.Generic;
using UnityEngine;
namespace CaomaoFramework
{
    /// <summary>
    /// 红点系统
    /// </summary>
    [Module(false)]
    public class RedPointModule : IRedPointModule, IModule
    {
        public IRedPointData Root;
        private bool m_bInit = false;
        public void Init()
        {
            //通过数据初始化树状结构
            CaomaoDriver.ResourceModule.LoadAssetAsync(CaomaoGameGobalConfig.Instance.RedPointTreePath,(temp)=>
            {
                var asset = temp as RedPointTreeDataConfig;
                if (asset != null && asset.TreeNode != null)
                {
                    this.InitRedPointData(asset.TreeNode, null);
                    asset = null;
                    this.m_bInit = true;
                }
            },true);
        }
        private void InitRedPointData(RedPointTreeData data,IRedPointData treeNode)
        {
            if (treeNode == null)
            {
                treeNode = this.CreateRedPointData(data.Id, data.RedPointType);
                this.Root = treeNode;
                if (treeNode == null)
                {
                    Debug.LogError("RedPoint == null:" + data.RedPointType);
                    return;
                }
            }
            else
            {
                var temp = treeNode;
                treeNode = this.CreateRedPointData(data.Id, data.RedPointType);
                temp.AddChild(treeNode);
            }
            foreach (var c in data.Childs)
            {
                if (c != null)
                {
                    this.InitRedPointData(c,treeNode);
                }
            }
        }
        private IRedPointData CreateRedPointData(string id,ERedPointType type)
        {
            switch (type)
            {
                case ERedPointType.Simple:
                    return new SimpleRedPointData(id);
                case ERedPointType.Number:
                    return new NumberRedPointData(id);
            }
            return null;
        }
        /// <summary>
        /// 绑定红点Button（在UI初始化的时候绑定）
        /// </summary>
        /// <param name="button"></param>
        public void BindUI(CUIRedPointButton button)
        {
            if (button == null || this.m_bInit == false)
            {
                Debug.LogError("No Init");
                return;
            }
            //var data = this.GetData(button.LayerIds);
            var data = this.GetData(button.ID,this.Root);
            if (data != null)
            {
                Debug.Log(button.ID);
                button.BindData(data);
            }
            else
            {
                Debug.LogError("Null");
            }
        }
        public void BindUI(params CUIRedPointButton[] buttons) 
        {
            foreach (var b in buttons) 
            {
                this.BindUI(b);
            }
        }
        /// <summary>
        /// 通知界面红点消失或者显示
        /// </summary>
        /// <param name="id"></param>
        /// <param name="bShow"></param>
        public void NotifyRedPoint(string id,bool bShow)
        {
            var data = this.GetData(id, this.Root);
            if (data != null)
            {
                data.NotifyRedPoint(bShow);
            }
            else 
            {
                Debug.LogError("No data:" + id);
            }
        }

        ///// <summary>
        ///// 通过id找到RedPointData
        ///// </summary>
        ///// <param name="id"></param>
        ///// <returns></returns>
        //private IRedPointData GetData(List<string> ids)
        //{
        //    if (this.Root != null)
        //    {
        //        var temp = this.Root;
        //        foreach (var id in ids)
        //        {
        //            if (temp.GetChild(id, ref temp) == false)
        //            {
        //                return null;
        //            }
        //        }
        //        return temp;
        //    }
        //    return null;
        //}

        private IRedPointData GetData(string id,IRedPointData temp)
        {
            if (temp != null)
            {
                var result = temp;
                if (temp.GetChild(id, ref temp) == false) 
                {
                    foreach (var child in result.Childs.Values) 
                    {
                        return this.GetData(id, child);
                    }
                }
                return temp;
            }
            return null;
        }

        public void Update()
        {
            
        }
    }
}
