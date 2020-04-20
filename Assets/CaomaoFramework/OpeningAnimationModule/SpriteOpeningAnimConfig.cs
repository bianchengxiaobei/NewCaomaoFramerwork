using UnityEngine;
using System.Collections.Generic;
using Sirenix.OdinInspector;
namespace CaomaoFramework 
{
    [CreateAssetMenu(fileName = "SpriteOpeningAnimConfig", menuName = "CaomaoFramework/SpriteOpeningAnimConfig")]
    public class SpriteOpeningAnimConfig : CaomaoRuntimeGlobalSBConfig<SpriteOpeningAnimConfig>
    {     
        [LabelText("淡入时间")]
        public float FadeInTime;
        [LabelText("淡出时间")]
        public float FadeOutTime;
        [LabelText("持续时间")]
        public float Duration;
        [LabelText("Logo序列")]
        public List<SpriteSequence> SpriteSequences = new List<SpriteSequence>();
    }
}
