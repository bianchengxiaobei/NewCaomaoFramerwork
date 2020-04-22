using UnityEngine;
using System.Collections;
namespace CaomaoFramework 
{
    public interface INewbieHelpModule
    {
        void SetVaildArea(RectTransform area);
        void SetUIToRoot(Transform uiTransform);
    }
}
