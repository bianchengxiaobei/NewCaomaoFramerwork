using UnityEngine;
using System.Collections.Generic;
using System;

namespace CaomaoFramework 
{
    [Serializable]
    public class NewbieHelpButtonNoContentData
    {
        public string ButtonPath;//button的层级路径。从Root开始

    }
    [Serializable]
    public class NewbieHelpButtonWithContentData : NewbieHelpButtonNoContentData
    {
        public string Content;//提示
    }
}

