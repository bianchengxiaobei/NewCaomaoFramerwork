using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CaomaoFramework
{
    public interface IAudioManager
    {
        float VolumeMain { get; }
        float VolumeEffect { get; }
        bool Enable { get; }
        bool EnableEffect { get; }
    }
}
