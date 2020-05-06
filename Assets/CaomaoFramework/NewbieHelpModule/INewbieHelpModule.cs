using UnityEngine;
using System.Collections;
namespace CaomaoFramework 
{
    public interface INewbieHelpModule
    {
        void StartNewbieHelp(int newbieMainId);
        void SetVaildArea(RectTransform area);
        void SetUIToRoot(RectTransform uiTransform);
    }
}
