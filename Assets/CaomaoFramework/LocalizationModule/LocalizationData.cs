using UnityEngine;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using System;
[CreateAssetMenu(fileName = "LocalizationData", menuName = "CaomaoFramewrok/LocalizationData")]
[Serializable]
public class LocalizationData : SerializedScriptableObject
{
    public string language;
    public Dictionary<string, string> data = new Dictionary<string, string>();


    public void AddData(string id, string content)
    {
        if (this.data.ContainsKey(id))
        {
            this.data[id] = content;
            return;
        }
        this.data.Add(id, content);
    }
}
