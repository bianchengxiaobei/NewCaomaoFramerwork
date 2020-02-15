using UnityEngine;
using UnityEditor;
namespace CaomaoFramework.PlayerPrefsExt
{
    public class CaomaoPlayerPrefsIntData : CaomaoPlayerPrefsDataBase
    {
        public int Value;
        public CaomaoPlayerPrefsIntData(string key,int value,int index):base(key,index)
        {
            this.Value = value;
        }
        public override void DrawValue(Rect rect)
        {
            EditorGUI.TextField(rect, this.Value.ToString(), EditorStyles.textField);
        }
        public override string GetValueType()
        {
            return "int";
        }
    }
}
