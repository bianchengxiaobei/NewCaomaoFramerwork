using UnityEngine;
namespace CaomaoFramework
{
    public interface ILocalizationModule
    {
        void ChangeLanguage(SystemLanguage language);
        string GetString(string key);
        string GetString(string key, string defaultString);
    }
}
