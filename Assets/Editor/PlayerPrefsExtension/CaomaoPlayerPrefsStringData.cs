using UnityEngine;
using UnityEditor;
namespace CaomaoFramework.PlayerPrefsExt
{
    public class CaomaoPlayerPrefsStringData : CaomaoPlayerPrefsDataBase
    {
        public string Value;
        public CaomaoPlayerPrefsStringData(string key,string value,int index) : base(key,index)
        {
            this.Value = value;
        }
        public override void DrawValue(Rect rect)
        {
            EditorGUI.TextField(rect, this.Value.ToString(), EditorStyles.textField);
        }
        public override string GetValueType()
        {
            return "string";
        }
    }
}