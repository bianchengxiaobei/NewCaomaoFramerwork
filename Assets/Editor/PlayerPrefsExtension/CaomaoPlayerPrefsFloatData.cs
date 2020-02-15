using UnityEngine;
using UnityEditor;
namespace CaomaoFramework.PlayerPrefsExt
{
    public class CaomaoPlayerPrefsFloatData : CaomaoPlayerPrefsDataBase
    {
        public float Value;
        public CaomaoPlayerPrefsFloatData(string key,float value,int index) : base(key,index)
        {
            this.Value = value;
        }
        public override void DrawValue(Rect rect)
        {
            EditorGUI.TextField(rect, this.Value.ToString(), EditorStyles.textField);
        }
        public override string GetValueType()
        {
            return "float";
        }
    }
}