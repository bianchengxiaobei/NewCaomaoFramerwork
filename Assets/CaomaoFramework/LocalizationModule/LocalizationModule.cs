using System;
using System.Collections.Generic;
using UnityEngine;
namespace CaomaoFramework
{
    [Module(false)]
    public class LocalizationModule : ILocalizationModule, IModule
    {
        public SystemLanguage m_eLanguage = SystemLanguage.Unknown;
        private const string SLanguage = "Language";
        //private const string SLocalizationFolderPath = "Assets/CaomaoFramework/LocalizationModule";
        private Dictionary<string, string> m_stringDic = new Dictionary<string, string>();//字典
        public SystemLanguage Language
        {
            get
            {
                if (this.m_eLanguage == SystemLanguage.Unknown)
                {
                    if (PlayerPrefs.HasKey(SLanguage) && this.m_eLanguage != SystemLanguage.Unknown)
                    {
                        this.m_eLanguage = (SystemLanguage)PlayerPrefs.GetInt(SLanguage);
                    }
                    else
                    {
                        this.m_eLanguage = Application.systemLanguage;
                        PlayerPrefs.SetInt(SLanguage, (int)this.m_eLanguage);
                    }
                }
                return this.m_eLanguage;
            }
            private set
            {
                if (this.m_eLanguage == value)
                {
                    return;
                }
                this.m_eLanguage = value;
                PlayerPrefs.SetInt(SLanguage,(int)this.m_eLanguage);
                //改变字典
                this.ChangeLangDic();
            }
        }

        public void ChangeLanguage(SystemLanguage language)
        {
            if (this.Language == language)
            {
                return;
            }
            this.Language = language;
        }

        public string GetString(string key)
        {
            string result = "";
            if (this.m_stringDic.Count == 0)
            {
                Debug.LogError("没有初始化本地化数据："+key);
                return result;
            }           
            if (!this.m_stringDic.TryGetValue(key, out result))
            {
                Debug.LogError("本地化数据不存在关键字:" + key);
            }
            return result;
        }

        public string GetString(string key, string defaultString)
        {
            var result = this.GetString(key);
            if (string.IsNullOrEmpty(result))
            {
                return defaultString;
            }
            return result;
        }

        private void ChangeLangDic()
        {
            CaomaoDriver.ResourceModule.LoadAsset(this.GetLocalizationFileLabel(), (asset) =>
            {
                var dic = ((LocalizationData)asset).data;
                foreach (var data in dic)
                {
                    this.m_stringDic[data.Key] = data.Value;
                }
            });
        }

        public void Init()
        {
            //加载当前选择的语言
            this.m_stringDic.Clear();
            this.ChangeLangDic();
        }


        private string GetLocalizationFileLabel()
        {
            var result = "";
            switch (this.Language)
            {
                case SystemLanguage.Chinese:
                case SystemLanguage.ChineseSimplified:
                case SystemLanguage.ChineseTraditional:
                    result = "Localization_中国";
                    break;
            }
            return result;
        }

        public void Update()
        {
            
        }
    }
}
