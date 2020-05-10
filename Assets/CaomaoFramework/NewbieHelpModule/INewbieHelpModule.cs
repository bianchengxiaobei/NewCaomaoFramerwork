using UnityEngine;
using System.Collections;
namespace CaomaoFramework 
{
    public interface INewbieHelpModule
    {
        void StartNewbieHelp(int newbieMainId);
        void SetVaildArea(RectTransform area);
        Vector3 SetUIToRoot(RectTransform uiTransform, int index = 0);
        void SetUIGlowTip(RectTransform center, Vector2? targetSize = null, Vector2? orginSize = null);
        void SetUIGlowTipNoVisiable();
    }
}
