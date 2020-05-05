using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using System.Data;
using System.IO;
namespace CaomaoFramework.LocalizationEditor
{
    public class CaomaoLocalizationOperator
    {
        
        private ICaomaoHeader Header;
        private Dictionary<string, LocalizationData> allSbData = new Dictionary<string, LocalizationData>();
        public CaomaoLocalizationOperator(ICaomaoHeader header)
        {
            this.Header = header;
            var path = CaomaoLocalizationGobalConfig.Instance.LocalizationExcelFolderPath;
            if (string.IsNullOrEmpty(path))
            {
                path = "Assets/Editor/LocalizationWindow/LocalizationExcel";
                CaomaoLocalizationGobalConfig.Instance.LocalizationExcelFolderPath =
                    path;
            }
            this.ExcelFolderPath = path;
            path = CaomaoLocalizationGobalConfig.Instance.LocalizationScriptableObjectPath;
            if (string.IsNullOrEmpty(path))
            {
                path = "Assets/CaomaoFramework/LocalizationModule/LocalizationData";
                CaomaoLocalizationGobalConfig.Instance.LocalizationScriptableObjectPath =
                    path;
            }
            this.SBFolderPath = path;
            path = CaomaoLocalizationGobalConfig.Instance.LocalizationTemplatePath;
            if (string.IsNullOrEmpty(path))
            {
                path = "Assets/Editor/LocalizationWindow/LocalizationConstTemplate.txt";
                CaomaoLocalizationGobalConfig.Instance.LocalizationTemplatePath =
                    path;
            }
            this.TemplateFilePath = path;
            this.ExcelFilePath = this.ExcelFolderPath + "/Localization.xlsx";
            this.Config = CaomaoLocalizationGobalConfig.Instance.SBConfig;
            this.InitSbData();
        }
        [PropertyOrder(-1)]
        [OnInspectorGUI]
        public void DrawHeader()
        {
            var rect = EditorGUI.IndentedRect(EditorGUILayout.BeginHorizontal());
            this.Header.Draw(rect.width,rect.height,rect.x,rect.y);
            EditorGUILayout.EndHorizontal();
            GUILayout.Space(10f);
        }

        [InfoBox("存放本地化Excel的文件夹，注意:如果自定义自己的文件夹，需要点击[更新本地化Excel文件目录按钮],否则会失效！")]
        [FolderPath]
        [LabelText("本地化Excel文件目录")]
        [InlineButton("UpdateExcelFolderPath", "更新本地化Excel文件目录")]
        [PropertyOrder(0)]
        public string ExcelFolderPath;
        [NonSerialized]
        public string ExcelFilePath;
        [InfoBox("存放本地化ScriptableObject的文件夹，注意:如果自定义自己的文件夹，需要点击[更新本地化SB文件目录按钮],否则会失效！")]
        [FolderPath]
        [LabelText("本地化SB文件目录")]
        [InlineButton("UpdateSBFolderPath", "更新本地化SB文件目录")]
        [PropertyOrder(1)]
        [Space(10f)]
        public string SBFolderPath;

        [InfoBox("存放本地化Template的文件夹，注意:如果自定义自己的文件夹，需要点击[更新本地化Template文件目录按钮],否则会失效！")]
        [FilePath]
        [LabelText("本地化Template文件目录")]
        [InlineButton("UpdateTemplateFilePath", "更新本地化Template文件目录")]
        [PropertyOrder(2)]
        [Space(10f)]
        public string TemplateFilePath;

        [Button("编辑本地化Excel", ButtonSizes.Large)]
        [GUIColor(0.4f, 0.8f, 1f)]
        [ButtonGroup]
        [PropertyOrder(3)]
        public void OpenLocalizationExcel()
        {
            var obj = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(
                this.ExcelFilePath);
            if (obj == null)
            {
                EditorUtility.DisplayDialog("本地化Excel文件不存在", "请检查在该文件夹下是否存在Localization.xlsx", "确定");
                return;
            }
            AssetDatabase.OpenAsset(obj);
        }

        [Button("生成常量脚本", ButtonSizes.Large)]
        [GUIColor(0.4f, 0.8f, 1f)]
        [ButtonGroup]
        [PropertyOrder(3)]
        public void GenLocalizationConst()
        {
            //var templatePath = $"Assets/{this.TemplateFilePath}";
            var templateText = AssetDatabase.LoadAssetAtPath<TextAsset>(this.TemplateFilePath);
            if (templateText == null)
            {
                Debug.LogError("Template文件不存在:"+this.TemplateFilePath);
                return;
            }
            CaomaoScriptGenerateModule.Instance.Restart(templateText.text);
            int col = 0;int row = 0;
            var excel = CaomaoEditorHelper.ReadExcelRow(this.ExcelFilePath, ref col, ref row);
            if (excel == null)
            {
                Debug.LogError("No Excel:" + this.ExcelFilePath);
                return;
            }
            List<object[]> allV = new List<object[]>();
            for (int r = 1; r < row; r++)
            {
                var nextRow = excel[r];
                var constValue = nextRow[0].ToString();
                if (string.IsNullOrEmpty(constValue))
                {
                    //Debug.LogError("Const常量为null:"+r);
                    continue;
                }
                Debug.Log(constValue);
                var id = nextRow[1].ToString();
                if (string.IsNullOrEmpty(id))
                {
                    //Debug.LogError("Id为null:" + r);
                    continue;
                }
                Debug.Log(id);
                var objects = new object[]
                {
                    constValue,
                    id
                };
                allV.Add(objects);
            }
            CaomaoScriptGenerateModule.Instance.AddVariable("variables", allV.ToArray());
            var script = CaomaoScriptGenerateModule.Instance.Parse();
            File.WriteAllText(Application.dataPath +"/CaomaoFramework/LocalizationModule/LocalizationConst.cs" ,script);
            AssetDatabase.Refresh();
        }

        [Button("Excel转ScriptableObject",ButtonSizes.Large)]
        [GUIColor(0.91f,0.58f,0.48f)]
        [ButtonGroup]
        [PropertyOrder(3)]
        public void UpdateLocalizationToSB()
        {
            int col = 0;
            int row = 0;
            var excel = CaomaoEditorHelper.ReadExcelRow(this.ExcelFilePath,ref col,ref row);
            if (excel == null)
            {
                Debug.LogError("No Excel:" + this.ExcelFilePath);
                return;
            }
            DataRow firstRow = excel[0];
            for (int c = 2; c < col; c++)
            {
                var lang = firstRow[c] as string;
                if (string.IsNullOrEmpty(lang))
                {
                    continue;
                }
                Debug.Log(lang);
                this.AddConfigName(lang);              
            }
            this.InitSbData();
            if (this.allSbData.Count > 0)
            {
                //第一行，第一列是id，接下来是各国语言
                for (int r = 1; r < row; r++)
                {
                    var nextRow = excel[r];
                    var id = nextRow[1].ToString();
                    if (string.IsNullOrEmpty(id))
                    {
                        continue;
                    }
                    for (int c = 2; c < col; c++)
                    {
                        var index = c - 2;
                        if (index < this.Config.Count)
                        {
                            var content = excel[r][c].ToString();
                            if (string.IsNullOrEmpty(content))
                            {
                                continue;
                            }
                            Debug.Log(content);
                            var lang = this.Config[index].Language;
                            LocalizationData data = null;
                            this.allSbData.TryGetValue(lang, out data);
                            if (data != null)
                            {
                                data.AddData(id.ToString(), content);
                            }
                        }                       
                    }
                }
                this.SaveLocalizationData();
            }            
        }
        [TableList]
        [LabelText("本地化语言设置")]
        [PropertyOrder(4)]
        [Space(10f)]
        [SerializeField]
        public List<CaomaoLocalizationSBConfig> Config = new List<CaomaoLocalizationSBConfig>();


        private void UpdateExcelFolderPath()
        {
            var temp = CaomaoLocalizationGobalConfig.Instance.LocalizationExcelFolderPath;
            if (this.ExcelFolderPath != temp)
            {
                //更新
                CaomaoLocalizationGobalConfig.Instance.LocalizationExcelFolderPath = this.ExcelFolderPath;
                EditorUtility.SetDirty(CaomaoLocalizationGobalConfig.Instance);
            }
        }
        private void UpdateSBFolderPath()
        {
            var temp = CaomaoLocalizationGobalConfig.Instance.LocalizationScriptableObjectPath;
            if (this.SBFolderPath != temp)
            {
                //更新
                CaomaoLocalizationGobalConfig.Instance.LocalizationScriptableObjectPath = this.SBFolderPath;
                EditorUtility.SetDirty(CaomaoLocalizationGobalConfig.Instance);
            }
        }

        private void UpdateTemplateFilePath()
        {
            var temp = CaomaoLocalizationGobalConfig.Instance.LocalizationTemplatePath;
            if (this.TemplateFilePath != temp)
            {
                //更新
                CaomaoLocalizationGobalConfig.Instance.LocalizationTemplatePath = this.TemplateFilePath;
                EditorUtility.SetDirty(CaomaoLocalizationGobalConfig.Instance);
            }
        }


        private void SaveLocalizationData()
        {
            foreach (var c in this.allSbData.Values)
            {
                EditorUtility.SetDirty(c);
            }
        }
        private void AddConfigName(string lang)
        {
            foreach (var c in this.Config)
            {
                if (c.Language == lang)
                {
                    return;
                }
            }
            //添加,创建SB
            var sbName = $"Localization_{lang}";
            var config = new CaomaoLocalizationSBConfig(lang, sbName);
            this.Config.Add(config);
            CaomaoLocalizationGobalConfig.Instance.AddConfig(config);
            var t = ScriptableObject.CreateInstance<LocalizationData>();
            t.language = lang;
            Debug.Log("新增一门语言:" + lang);
            AssetDatabase.CreateAsset(t, $"{this.SBFolderPath}/{sbName}.asset");
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
        private void InitSbData()
        {
            string path = "";
            if (this.SBFolderPath.StartsWith("Assets/"))
            {
                path = Application.dataPath + "/" + this.SBFolderPath.Remove(0,7);
            }
            else
            {
                path = Application.dataPath + "/" + this.SBFolderPath;
            }
            if (Directory.Exists(path))
            {
                var allFiles = Directory.GetFiles(path);
                foreach (var file in allFiles)
                {
                    if (file.EndsWith(".meta"))
                    {
                        continue;
                    }
                    var name = Path.GetFileName(file);
                    var sbPath = $"{this.SBFolderPath}/{name}";
                    var temp = AssetDatabase.LoadAssetAtPath<LocalizationData>(sbPath);
                    if (temp == null)
                    {
                        Debug.LogError("No Assets:" + sbPath);
                        continue;
                    }
                    if (!this.allSbData.ContainsKey(temp.language))
                    {
                        this.allSbData.Add(temp.language, temp);
                    }
                }
            }
            else
            {
                Debug.LogError("No Dir:" + path);
            }
        }
    }
}
