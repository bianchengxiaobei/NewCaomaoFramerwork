using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TestText : Text
{
    protected override void OnPopulateMesh(VertexHelper toFill)
    {
        base.OnPopulateMesh(toFill);
        //5
        //end 3
        //start 0
        var gen = this.cachedTextGenerator;
        Debug.Log("顶点:"+toFill.currentVertCount);
        Debug.Log("索引:"+toFill.currentIndexCount);
        Debug.Log(gen.characterCount);
        Debug.Log(gen.characters.Count);
        var charList = gen.characters;
        foreach (var temp in charList)
        {
            Debug.Log(temp);
        }
    }
}
