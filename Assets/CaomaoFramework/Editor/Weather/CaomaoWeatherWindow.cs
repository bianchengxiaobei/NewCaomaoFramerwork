using System;
using UnityEditor;
using UnityEngine;
//using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;
using Sirenix.Utilities;
using System.Net;
using System.IO;
using System.Text;
using System.IO.Compression;
using System.Threading.Tasks;
/// <summary>
/// 天气预报
/// </summary>
public class CaomaoWeatherWindow : OdinEditorWindow
{
    public static EditorWindow Window;
    private const string SWeatherUrl = "http://wthrcdn.etouch.cn/weather_mini?city={0}";
    private const string IpUrl = "http://www.net.cn/static/customercare/yourip.asp";
    private const string SBaiDuUrl = "https://api.map.baidu.com/location/ip?ak=7InbTMxtSH23UgIszTBTMGnQ9LSZK5AO&ip={0}&coor=bd09ll";
    private string City = "";
    private string IPAddress = "";
    private string BaiDuUrl = "";
    private string WeatherUrl = "";
    private BaiduPositionData positionData;
    private WeatherJsonData weatherData;
    private bool m_bHasWeatherData = false;
    private CaomaoCardList<CaomaoWeatherCard> CardList;
    private ICaomaoHeader Header;
    private GUIContent m_provinceContent = GUIContent.none;
    private GUIContent m_cityContent = GUIContent.none;
    private GUIStyle toolbar;
    [MenuItem("CaomaoTools/WeatherWindow")]
    public static void ShowWindow()
    {
        Window = GetWindow<CaomaoWeatherWindow>();
        Window.position = GUIHelper.GetEditorWindowRect().AlignCenter(1000, 600);
    }
    protected override void Initialize()
    {
        base.Initialize();
     
        if (this.Header == null)
        {
            this.Header = new CaomaoCallbackHeader("天气预报模块",this.DrawToolbar);
        }
        if (this.CardList == null)
        {
            this.CardList = new CaomaoCardList<CaomaoWeatherCard>("实时天气预报", true);
        }
        if (this.toolbar == null)
        {
            this.toolbar = new GUIStyle()
            {
                padding = new RectOffset(0, 1, 0, 0),
                stretchHeight = true,
                stretchWidth = true,
                fixedHeight = 0f
            };
        }
        if (this.m_bHasWeatherData == false)
        {
            var data = CaomaoWeatherGlobalConfig.Instance.WeatherData;
            if (data != null && data.data != null && data.status == 1000 && data.IsToday(DateTime.Now.Day))
            {
                this.m_bHasWeatherData = true;
                this.weatherData = data;
                this.LoadWeatherData();
                return;
            }
            //加载天气数据
            var task = this.GetWeatherTask();
        }
    }
    protected override void OnGUI()
    {
        this.Header.Draw(this.position.width,this.position.height);
        base.OnGUI();      
    }
    protected override void DrawEditors()
    {
        //
        if (this.m_bHasWeatherData == false)
        {
            return;
        }
        this.CardList.Draw(this);
        base.DrawEditors();
    }
    public void DrawToolbar()
    {
        SirenixEditorGUI.BeginHorizontalToolbar(22f, 4);
        if (SirenixEditorGUI.ToolbarButton(this.m_provinceContent, false))
        {

        }
        if (SirenixEditorGUI.ToolbarButton(this.m_cityContent, false))
        {

        }
        if (SirenixEditorGUI.ToolbarButton("刷新", false))
        {

        }
        SirenixEditorGUI.EndHorizontalToolbar();
    }
    private void LoadWeatherData()
    {
        var first = new CaomaoWeatherCard(this.weatherData.data.forecast[0],this.weatherData.data.wendu,true);
        var second = new CaomaoWeatherCard(this.weatherData.data.forecast[1]);
        var third = new CaomaoWeatherCard(this.weatherData.data.forecast[2]);
        var four = new CaomaoWeatherCard(this.weatherData.data.forecast[3]);
        var five = new CaomaoWeatherCard(this.weatherData.data.forecast[4]);
        this.CardList.AddCard(first);
        this.CardList.AddCard(second);
        this.CardList.AddCard(third);
        this.CardList.AddCard(four);
        this.CardList.AddCard(five);

        this.m_cityContent = new GUIContent(CaomaoWeatherGlobalConfig.Instance.City);
        this.m_provinceContent = new GUIContent(CaomaoWeatherGlobalConfig.Instance.Province);
        
    }
    private async Task GetWeatherTask()
    {
        using (WebClient web = new WebClient())
        {
            //IP
            var htmlContent = await web.DownloadStringTaskAsync(IpUrl);
            this.IPAddress = GetIPFromHtml(htmlContent);
            this.BaiDuUrl = string.Format(SBaiDuUrl, this.IPAddress);
            //定位所在的城市
            var positionContent = await web.DownloadStringTaskAsync(this.BaiDuUrl);
            //this.positionData = JsonConvert.DeserializeObject<BaiduPositionData>(positionContent);
            this.positionData = JsonUtility.FromJson<BaiduPositionData>(positionContent);
            this.City = this.positionData.content.address_detail.city;
            this.WeatherUrl = string.Format(SWeatherUrl, this.City);
            var weatherdata = await web.DownloadDataTaskAsync(this.WeatherUrl);
            GZipStream stream = new GZipStream(new MemoryStream(weatherdata), CompressionMode.Decompress);
            byte[] temp = new byte[1024];
            var sb = new StringBuilder();
            int l = stream.Read(temp, 0, 1024);
            while (l > 0)
            {
                sb.Append(Encoding.UTF8.GetString(temp, 0, l));
                l = stream.Read(temp, 0, 1024);
            }
            var weatherContent = sb.ToString();
            //this.weatherData = JsonConvert.DeserializeObject<WeatherJsonData>(weatherContent);
            this.weatherData = JsonUtility.FromJson<WeatherJsonData>(weatherContent);
            this.m_bHasWeatherData = true;
            CaomaoWeatherGlobalConfig.Instance.WeatherData = this.weatherData;
            CaomaoWeatherGlobalConfig.Instance.City = this.positionData.content.address_detail.city;
            CaomaoWeatherGlobalConfig.Instance.Province = this.positionData.content.address_detail.province;
            this.LoadWeatherData();
            stream.Close();
        }
    }
    public static string GetIPFromHtml(string pageHtml)
    {
        //验证ipv4地址
        string reg = @"(?:(?:(25[0-5])|(2[0-4]\d)|((1\d{2})|([1-9]?\d)))\.){3}(?:(25[0-5])|(2[0-4]\d)|((1\d{2})|([1-9]?\d)))";
        string ip = "";
        Match m = Regex.Match(pageHtml, reg);
        if (m.Success)
        {
            ip = m.Value;
        }
        return ip;
    }
}
public class BaiduPositionData
{
    public string address;
    public BaiduPositionDataContent content;
    public int status;
}
public class BaiduPositionDataContent
{
    public string address;
    public BaiduPositionDataDetail address_detail;
    public BaiduPostionPoint point;
}
public class BaiduPositionDataDetail
{
    public string city;
    public int city_code;
    public string district;
    public string province;
    public string street;
    public string street_number;
}
public class BaiduPostionPoint
{
    public string x;
    public string y;
}
[Serializable]
public class WeatherJsonData
{
    public WeatherData data;
    public int status;
    public string desc;

    public bool IsToday(int day)
    {
        var s = this.data.forecast[0].date;
        var index = s.Length - 4;
        var d = s.Substring(0, index);
        var result = day.ToString() == d;
        return result;
    }
}
[Serializable]
public class WeatherData
{
    public WeatherDataDetail yesterday;
    public string city;
    public List<WeatherDataDetail> forecast;
    public string ganmao;
    public string wendu;
}
[Serializable]
public class WeatherDataDetail
{
    public string date;
    public string high;
    public string fengli;
    public string fl;
    public string low;
    public string fengxiang;
    public string type;
}