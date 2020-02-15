using System;
[AttributeUsage(AttributeTargets.Class)]
public class ModuleAttribute : Attribute
{ 
    public bool Update;
    public ModuleAttribute(bool _update = true)
    {
        this.Update = _update;
    }
}
