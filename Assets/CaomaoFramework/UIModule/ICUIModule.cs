using System;
using UnityEngine;
namespace CaomaoFramework
{
    public interface ICUIModule
    {
        void Init();
        void AddUI(string uiName, UIBase ui);
        UIBase GetUI(string type);
        void ShowUI<T>(string defaultName = null) where T : UIBase;

        void PreLoadUI<T>(Action<T> onFinished = null,string defaultName = null) where T : UIBase;
    }
}
