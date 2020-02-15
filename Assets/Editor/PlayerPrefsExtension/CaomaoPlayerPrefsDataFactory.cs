using UnityEngine;
using UnityEditor;
namespace CaomaoFramework.PlayerPrefsExt
{
    public static class CaomaoPlayerPrefsDataFactory
    {
        public static CaomaoPlayerPrefsDataBase CreateCaomaoPlayerPrefsData(string key,int index)
        {
            var sValue = PlayerPrefs.GetString(key, CaomaoPlayerPrefsDataBase.DefaultString);
            if (sValue != CaomaoPlayerPrefsDataBase.DefaultString)
            {
                return new CaomaoPlayerPrefsStringData(key,sValue,index);
            }
            else
            {
                var iValue = PlayerPrefs.GetInt(key, CaomaoPlayerPrefsDataBase.DefaultInt);
                if (iValue != CaomaoPlayerPrefsDataBase.DefaultInt)
                {
                    return new CaomaoPlayerPrefsIntData(key, iValue,index);
                }
                else
                {
                    var fValue = PlayerPrefs.GetFloat(key, CaomaoPlayerPrefsDataBase.DefaultFloat);
                    if (fValue != CaomaoPlayerPrefsDataBase.DefaultFloat)
                    {
                        return new CaomaoPlayerPrefsFloatData(key, fValue,index);
                    }
                }
            }
            return null;
        }
    }
}
