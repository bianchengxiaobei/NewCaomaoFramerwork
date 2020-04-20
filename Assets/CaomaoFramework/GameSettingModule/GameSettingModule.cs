using UnityEngine;
using System.Collections;
using DG.Tweening;
namespace CaomaoFramework 
{
    [Module(false)]
    public class GameSettingModule : IModule
    {
        public void Init()
        {
            //设置帧率这些
            Application.targetFrameRate = 30;
            //是否动画可以重复利用
            DOTween.defaultRecyclable = true;
        }

        public void Update()
        {

        }
    }
}

