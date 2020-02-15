using UnityEngine;
using UnityEditor;
using Sirenix.Utilities.Editor;
public class CaomaoWeatherCard : CaomaoCardBase
{
    public WeatherDataDetail data;
    public bool m_bMain;
    public string CenterWendu;
    public string fengli = "";
    private GUIStyle centerWendu;
    private GUIStyle dateStyle;
    private string LowToHighWendu;
    public CaomaoWeatherCard(WeatherDataDetail data, string curWendu = "", bool bMain = false)
    {
        this.data = data;
        this.m_bMain = bMain;
        this.CenterWendu = curWendu;
        if (this.data == null)
        {
            Debug.LogWarning("NO Weather");
            return;
        }
        var fengxiang = this.data.fengxiang.StartsWith("无") ? "风力" : this.data.fengxiang;
        var fl = this.data.fengli.Remove(0, 9);
        var startIndex = fl.Length - 3;
        fl = fl.Remove(startIndex, 3);
        this.fengli = fengxiang + fl;
        this.LowToHighWendu = $"{this.data.low.Trim()}~{this.data.high.Trim()}";
        this.centerWendu = new GUIStyle(SirenixGUIStyles.BoldTitleCentered);
        this.centerWendu.fontSize = 30;
        this.dateStyle = new GUIStyle(SirenixGUIStyles.BoldLabelCentered);
    }
    public override void DrawCard()
    {
        if (this.data == null)
        {
            GUILayout.Label("没有天气数据", SirenixGUIStyles.BoldTitleCentered);
            return;
        }
        if (this.m_bMain)
        {
            GUILayout.Label(this.data.date, this.dateStyle);
        }
        else
        {
            GUILayout.Label(this.data.date, SirenixGUIStyles.CenteredGreyMiniLabel);
        }
        GUILayout.Space(10);
        //图片
        GUILayout.Label(GetWeatherTypeGUIContent(this.data.type), SirenixGUIStyles.LabelCentered);
        GUILayout.Space(10);
        if (this.m_bMain)
        {
            GUILayout.Label($"{this.CenterWendu}℃", this.centerWendu);
        }
        GUILayout.Space(10);
        GUILayout.Label(this.LowToHighWendu, SirenixGUIStyles.LabelCentered);
        GUILayout.Space(10);
        GUILayout.Label(this.data.type, SirenixGUIStyles.LabelCentered);
        GUILayout.Space(10);
        GUILayout.Label(this.fengli, SirenixGUIStyles.LabelCentered);
    }
    public static GUIContent GetWeatherTypeGUIContent(string type)
    {
        //Debug.Log(type);
        GUIContent result = null;
        switch (type)
        {
            case "多云":
                result = CaomaoEditorIcon.DuoYunGUIContent;
                break;
            case "小雨":
                result = CaomaoEditorIcon.XiaoYuGUIContent;
                break;
            case "大雨":
                result = CaomaoEditorIcon.DaYuGUIContent;
                break;
            case "中雨":
            case "小到中雨":
                result = CaomaoEditorIcon.ZhongYuGUIContent;
                break;
            case "暴雨":
            case "大到暴雨":
                result = CaomaoEditorIcon.BaoYuGUIContent;
                break;
            case "大暴雨":
                result = CaomaoEditorIcon.DaBaoYuGUIContent;
                break;
            case "特大暴雨":
                result = CaomaoEditorIcon.TeDaBaoYuGUIContent;
                break;
            case "阴":
                result = CaomaoEditorIcon.YinGUIContent;
                break;
            case "晴":
                result = CaomaoEditorIcon.QingGUIContent;
                break;
            case "小雪":
                result = CaomaoEditorIcon.XiaoXueGUIContent;
                break;
            case "中雪":
                result = CaomaoEditorIcon.ZhongXueGUIContent;
                break;
            case "大雪":
                result = CaomaoEditorIcon.DaXueGUIContent;
                break;
            case "暴雪":
                result = CaomaoEditorIcon.BaoXueGUIContent;
                break;
            case "雨夹雪":
                result = CaomaoEditorIcon.YuJiaXueGUIContent;
                break;
            case "雷阵雨":
                result = CaomaoEditorIcon.LeiZhenYuGUIContent ;
                break;
            case "雷阵雨伴有冰雹":
                result = CaomaoEditorIcon.LeiZhenYuBingBaoGUIContent;
                break;
        }
        return result;
    }
}