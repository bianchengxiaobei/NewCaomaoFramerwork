using UnityEngine;
using UnityEditor;
using Sirenix.Utilities;
[CaomaoEditorConfig]
public class CaomaoWeatherGlobalConfig : GlobalConfig<CaomaoWeatherGlobalConfig>
{
    public WeatherJsonData WeatherData;
    public string City;
    public string Province;
}