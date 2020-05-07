using UnityEngine;
using System.Collections.Generic;
using System;

namespace CaomaoFramework 
{
    [Serializable]
    public class NewbieHelpButtonNoContentData
    {
        public string ButtonPath;//button的层级路径。从UIRoot开始

    }
    [Serializable]
    public class NewbieHelpButtonWithContentData : NewbieHelpButtonNoContentData
    {
        public string Content;//提示
        public float x;
        public float y;//提示所在的坐标位置
    }
}

