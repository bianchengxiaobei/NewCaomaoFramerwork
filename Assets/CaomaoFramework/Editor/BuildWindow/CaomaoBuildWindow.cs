using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
/// <summary>
/// 打包窗口
/// </summary>
public class CaomaoBuildWindow : OdinEditorWindow
{

    private static CaomaoBuildWindow Window;
    private static string WindowTitle = "构建窗口";
    [LabelText("构建平台")]
    public BuildTarget buildTarget;
    [LabelText("构建平台组")]
    public BuildTargetGroup buildTargetGroup;
    [LabelText("渠道宏定义列表")]
    public List<string> QuDaoList = new List<string>();
    [LabelText("是否是测试模式")]
    public bool bDevelpment;
    [FolderPath(AbsolutePath =true)]
    public string ExportApkPath;

    private string orginScriptDefine;//原先的程序集的宏定义


    [Button("开始打包",ButtonSizes.Large)]
    public void StartBuild()
    {
        Debug.Log("StartBuild");
        if (string.IsNullOrEmpty(ExportApkPath))
        {
            Debug.LogError("No ExportPath");
            return;
        }
        this.orginScriptDefine = PlayerSettings.GetScriptingDefineSymbolsForGroup(this.buildTargetGroup);
        if (string.IsNullOrEmpty(this.orginScriptDefine))
        {
            Debug.Log("No Define");
        }
        else
        {
            Debug.Log(this.orginScriptDefine);
        }      
        foreach (var qudao in this.QuDaoList)
        {
            var buildOptioin = new BuildPlayerOptions();
            var exportPath = this.ExportApkPath + "/" + qudao.ToString() + this.GetBuildResultExtension();
            buildOptioin.locationPathName = exportPath;
            buildOptioin.target = this.buildTarget;
            buildOptioin.targetGroup = this.buildTargetGroup;
            buildOptioin.options = BuildOptions.ShowBuiltPlayer;//构建完成之后显示文件夹
            PlayerSettings.SetScriptingDefineSymbolsForGroup(this.buildTargetGroup,this.orginScriptDefine + ";" + qudao);
            Debug.Log(exportPath);
            var report = BuildPipeline.BuildPlayer(buildOptioin);
            if (report != null && report.summary.result == UnityEditor.Build.Reporting.BuildResult.Failed)
            {
                //只要有一个构建失败就直接结束

                break;
            }
        }
        //设置宏定义为原先的宏定义
        PlayerSettings.SetScriptingDefineSymbolsForGroup(this.buildTargetGroup, this.orginScriptDefine);
        this.UpdateConfigData();
        this.Close();
    }


    [Button("更新数据", ButtonSizes.Large)]
    public void OnClickUpdateConfigData()
    {
        this.UpdateConfigData();
    }


    private string GetBuildResultExtension()
    {
        switch (this.buildTarget)
        {
            case BuildTarget.Android:
                return ".apk";
            case BuildTarget.iOS:
                return ".ipa";
            case BuildTarget.StandaloneWindows:
            case BuildTarget.StandaloneWindows64:
                return ".exe";
        }
        return string.Empty;
    }




    [MenuItem("CaomaoTools/构建窗口")]
    public static void ShowWindow()
    {
        Window = GetWindow<CaomaoBuildWindow>(WindowTitle);
        Window.position = GUIHelper.GetEditorWindowRect().AlignCenter(800, 600);
    }



    protected override void Initialize()
    {
        base.Initialize();
        //加载数据
        if (CaomaoBuildConfig.Instance.buildTarget == 0)
        {
            //说明是第一次
            this.buildTarget = EditorUserBuildSettings.activeBuildTarget;
            CaomaoBuildConfig.Instance.buildTarget = this.buildTarget;
        }
        else
        {
            this.buildTarget = CaomaoBuildConfig.Instance.buildTarget;
            this.bDevelpment = CaomaoBuildConfig.Instance.bDevelpment;
            this.buildTargetGroup = CaomaoBuildConfig.Instance.buildTargetGroup;
            if (this.QuDaoList != null && this.QuDaoList.Count > 0)
            {
                this.QuDaoList.Clear();
            }
            this.QuDaoList.AddRange(CaomaoBuildConfig.Instance.QuDaoList);
            this.ExportApkPath = CaomaoBuildConfig.Instance.ExportApkPath;
        }              
    }
    protected override void OnGUI()
    {
        base.OnGUI();
    }

    public void UpdateConfigData()
    {
        CaomaoBuildConfig.Instance.buildTarget = this.buildTarget;
        CaomaoBuildConfig.Instance.bDevelpment = this.bDevelpment;
        CaomaoBuildConfig.Instance.buildTargetGroup = this.buildTargetGroup;
        CaomaoBuildConfig.Instance.QuDaoList.Clear();
        CaomaoBuildConfig.Instance.QuDaoList.AddRange(this.QuDaoList);
        CaomaoBuildConfig.Instance.ExportApkPath = this.ExportApkPath;
    }

}

