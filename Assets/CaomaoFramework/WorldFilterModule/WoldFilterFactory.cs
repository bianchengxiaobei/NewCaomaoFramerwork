using UnityEngine;
using System.Collections;
namespace CaomaoFramework
{
    public class WoldFilterFactory
    {
        public virtual IWordFilterModule CreateWordFilterModule()
        {
            return null;
        }
    }
}

