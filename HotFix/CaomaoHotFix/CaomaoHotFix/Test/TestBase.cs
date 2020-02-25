using System;
using System.Collections.Generic;
using CaomaoFramework;
using UnityEngine;
namespace CaomaoHotFix
{
    public class TestBase : ITestA
    {
        public void Start()
        {
            this.OnStart();
        }

        public virtual void OnStart()
        {
            Debug.Log("Base Start");
        }
    }
}
