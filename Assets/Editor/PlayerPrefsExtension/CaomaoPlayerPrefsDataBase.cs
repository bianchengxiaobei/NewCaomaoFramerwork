using UnityEngine;
using UnityEditor;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using Sirenix.OdinInspector;
using System;
namespace CaomaoFramework.PlayerPrefsExt
{
    public class CaomaoPlayerPrefsDataBase
    {
        public string Key;
        public int Index;
        public static Action<int> ModifyCallback;
        public const string DefaultString = "String";
        public const int DefaultInt = int.MaxValue;
        public const float DefaultFloat = float.MaxValue;
        private const string ControllKey = "ControllKey";
        private const float TextFieldWidth = 250f;
        private const float ToggleWidth = 20f;
        private const float KeyLabelWidth = 30f;
        private const float ValueLabelWidth = 40f;
        private const float TypeLabelWidth = 60f;
        private const float IconWidth = 34f;
        private bool m_bLock = false;
        private bool m_bSelected = false;
        private bool m_bIsEditorVaule = false;
        private bool m_bIsEditorKey = false;
        private bool m_modified = false;//是否修改了
        public CaomaoPlayerPrefsDataBase(string _key,int _index)
        {
            this.Key = _key;
            this.Index = _index;
        }
        public void Draw()
        {
            GUILayout.Space(2);
            Rect controlRect = EditorGUILayout.GetControlRect();
            Rect toggleRect = controlRect.SetWidth(ToggleWidth);
            controlRect.xMin += ToggleWidth;
            Rect keyRect = controlRect.SetWidth(KeyLabelWidth);
            controlRect.xMin += KeyLabelWidth;
            Rect keyTextField = controlRect.SetWidth(TextFieldWidth);
            controlRect.xMin += TextFieldWidth + 5f;
            Rect valueRect = controlRect.SetWidth(ValueLabelWidth);
            controlRect.xMin += ValueLabelWidth;
            Rect valueTextField = controlRect.SetWidth(TextFieldWidth);
            GUIHelper.PushGUIEnabled(!this.m_bLock);
            this.m_bSelected = EditorGUI.Toggle(toggleRect,this.m_bSelected);
            GUIHelper.PopGUIEnabled();
            GUI.Label(keyRect,"key:", SirenixGUIStyles.LeftAlignedGreyMiniLabel);
            if (this.m_bIsEditorKey == false)
            {
                if (GUI.Button(keyTextField, this.Key, EditorStyles.boldLabel))
                {
                    this.m_bIsEditorKey = true;
                }
            }
            else
            {
                GUI.SetNextControlName(ControllKey);
                string text = EditorGUI.TextField(keyTextField, this.Key, EditorStyles.textField);
                var controllName = GUI.GetNameOfFocusedControl();
                if (controllName == ControllKey && (Event.current == Event.KeyboardEvent("return") 
                    || Event.current.OnKeyUp(KeyCode.Return)))
                {
                    this.m_bIsEditorKey = false;
                }
                //修改了
                if (text != this.Key)
                {
                    this.m_modified = true;
                    ModifyCallback(this.Index);
                }
            }
            
            GUI.Label(valueRect, "value:", SirenixGUIStyles.LeftAlignedGreyMiniLabel);
            this.DrawValue(valueTextField);
            controlRect.xMin += TextFieldWidth + 5f;
            Rect typeRect = controlRect.SetWidth(TypeLabelWidth);          
            GUI.Label(typeRect, "type:"+this.GetValueType(), SirenixGUIStyles.LeftAlignedGreyMiniLabel);                   
            controlRect.xMin += TypeLabelWidth + 5f;
            //Buttons  
            //保存            
            Rect saveRect = controlRect.SetWidth(IconWidth);
            GUIHelper.PushGUIEnabled(this.m_modified);
            if (SirenixEditorGUI.IconButton(saveRect, EditorIcons.Checkmark, "保存PlayerPrefs"))
            {
                
            }
            GUIHelper.PopGUIEnabled();
            controlRect.xMin += IconWidth + 5f;
            Rect cancelRect = controlRect.SetWidth(IconWidth * 2);
            if (GUI.Button(cancelRect, "取消修改"))
            {

            }
            controlRect.xMin += IconWidth * 2 + 5;
            //删除
            Rect deleteRect = controlRect.SetWidth(IconWidth);
            if (GUI.Button(deleteRect, "删除"))
            {

            }
            controlRect.xMin += IconWidth + 5f;
            //显示数据是否有效
            
        }
        public virtual void DrawValue(Rect rect)
        {

        }
        public virtual string GetValueType()
        {
            return null;
        }
        public void CancelModify()
        {
            this.m_modified = false;
            this.m_bIsEditorKey = false;
            this.m_bIsEditorVaule = false;
        }
    }
    public enum EPlayerPrefsType
    {
        None,
        Int,
        Float,
        String,
    }
}