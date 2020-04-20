using UnityEngine;
using System.Collections;
using System;
using Sirenix.OdinInspector;
namespace CaomaoFramework 
{
    //PS:多物体可以用一个Go。然后变成子物体就行了
    [Serializable]
    public class SpriteSequence
    {
        [LabelText("序列Index")]
        public int Index;
        //[LabelText("序列中是否有多物体（Logo）")] 
        //public bool MutiObj;
        //[LabelText("Logo在层级的名称（如果是多物体用;分割）")]
        [LabelText("Logo在层级的名称")]
        public string ImageGName;
        [NonSerialized]
        public GameObject ImageLogo;


        public void SetVisiable(int index)
        {
            if (this.Index == index)
            {
                this.ImageLogo.SetActive(true);
            }
            else
            {
                this.ImageLogo.SetActive(false);
            }
        }
    }
}
