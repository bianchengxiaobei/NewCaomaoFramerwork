using UnityEngine;
using Sirenix.OdinInspector;
using CaomaoFramework.Confuse;
[CreateAssetMenu(fileName = "ConfuseSetting", menuName = "CaomaoEditorAssets/ConfuseSetting")]
public class ConfuseSetting : ScriptableObject
{
    [LabelText("开启混淆")]
    [PropertyOrder(-1)]
    public bool bEnable = true;//默认开启
    [LabelText("是否il2cpp打包模式")]
    public bool bIL2cpp;
    [Button]
    public void Test()
    {
        //var c = new CaomaoConfuse();
        //c.Test();
        CaomaoWeatherWindow.Window.Close();
    }
}