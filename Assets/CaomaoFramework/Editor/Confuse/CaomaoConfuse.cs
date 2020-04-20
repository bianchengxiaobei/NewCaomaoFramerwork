using UnityEngine;
using UnityEditor;
using Mono.Cecil;
using Mono.Cecil.Rocks;
namespace CaomaoFramework.Confuse
{
    public class CaomaoConfuse
    {
        private ConfuseSetting setting;
        private CaomaoConfuseMachine machine = new CaomaoConfuseMachine();
        public bool Confusing()
        {
            //加载Setting
            this.setting = AssetDatabase.LoadAssetAtPath<ConfuseSetting>("Assets/CaomaoFramework/Editor/Config/ConfuseSetting.asset");
            if (this.setting == null)
            {
                Debug.LogError("No Confuse Setting Data!");
                return false;
            }
            else if (this.setting.bEnable == false)
            {
                Debug.LogWarning("没有开启混淆功能!");
                return false;
            }
            return true;
        }
        public void Test()
        {
            var d = machine.ReadAssembly("./Library/ScriptAssemblies/Assembly-CSharp.dll");
            var all = d.MainModule.GetAllTypes();
            foreach (var type in all)
            {
                if (type.FullName != "<Module>")
                {
                    var key = type.Interfaces;
                }
            }
        }
    }
}
