using UnityEngine;
namespace CaomaoFramework
{
    public interface ICUIModule
    {
        void Init();
        void AddUI(string uiName, UIBase ui);
        UIBase GetUI(string type);
    }
}
