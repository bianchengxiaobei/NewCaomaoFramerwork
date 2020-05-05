using Sirenix.Utilities;
[CaomaoEditorConfig]
public class CaomaoDataExportGobalConfig : GlobalConfig<CaomaoDataExportGobalConfig>
{
    //public string ExcelDataFolderPath;
    //public string JsonDataFolderPath;
    public string ExportFolderPath;//导出配置文件存放的目录
    public string ExportClassPath;//配置文件转.cs类存放的目录
    public string WithNamespaceTemplate;
    public string NoNamespaceTemplate;
}