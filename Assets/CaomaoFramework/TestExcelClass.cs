using UnityEngine;
using System.Collections.Generic;
using System;

[Serializable]
public class TestExcelClass
{
    public List<TestSubClass> A = new List<TestSubClass>();
    public long TestB;
    public List<int> TestC = new List<int>();
    public override string ToString()
    {
        var s = "";
        foreach (var a in this.A) 
        {
            s += a + ":";
        }
        return s;
    }
}
[Serializable]
public class TestSubClass 
{
    public float B;
    public List<TestSubClass2> TestSub = new List<TestSubClass2>();
    public override string ToString()
    {
        return B.ToString();
    }
}
[Serializable]
public class TestSubClass2 
{
    public bool TestBool;

    public override string ToString()
    {
        return this.TestBool.ToString();
    }
}