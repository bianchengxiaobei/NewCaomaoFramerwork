using CaomaoFramework;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 人物提示界面（点击关闭进入下一个新手引导）
/// </summary>
public class NewbieHelpTipStep : NewbieHelpStep
{
    public List<NewbieHelpDialogTip> Tips { get; set; }//所有对话
    private int m_iCurIndex = 0;//当前对话所在的索引
    private static Button UITipObj;
    public static void Init() 
    {
        CaomaoDriver.ResourceModule.LoadGameObjectAsync("", (obj) =>
        {
            UITipObj = obj.transform.Find("bt_tip").GetComponent<Button>();            
            if (UITipObj == null) 
            {
                Debug.LogError("UITip == null");
            }
        });
    }
    public override void Enter()
    {
        //实例化对话。然后设置mask为最上层
        if (UITipObj != null)
        {
            if (UITipObj.onClick.GetPersistentEventCount() == 0) 
            {
                UITipObj.onClick.AddListener(this.NextDialogTip);
            }
            //显示并且能点击
            CaomaoDriver.NewbieHelpModule.SetVaildArea(UITipObj.image.rectTransform);
            UITipObj.transform.position = Vector3.zero;
        }
        else 
        {
            Debug.LogError("UITip没有初始化成功!");
            Init();
        }
    }
    /// <summary>
    /// 下一个对话，如果没有就直接结束
    /// </summary>
    public void NextDialogTip()
    {
        this.m_iCurIndex++;
        if (this.m_iCurIndex >= this.Tips.Count)
        {
            //说明结束了
            this.OnFinished();
            this.Clear();
        }
    }
    /// <summary>
    /// 清除对话资源
    /// </summary>
    private void Clear()
    {
        base.Clear();
        this.m_iCurIndex = 0;
        this.Tips.Clear();
    }
}

public class NewbieHelpDialogTip
{
    public string dialogContent;//对话内容
    public string characterName;//人物名字
    public string characterPath;////对话角色资源名称（addressable里面的名称）
}
