using System.Collections.Generic;

public class CaomaoConfuseInheritData
{
    public Dictionary<ClassTypeData, List<ClassTypeData>> BaseClassTypes = new Dictionary<ClassTypeData, List<ClassTypeData>>();

    public void AddClassData(ClassTypeData key,HashSet<ClassTypeData> value)
    {
        if (this.BaseClassTypes.ContainsKey(key))
        {
            this.BaseClassTypes[key].AddRange(value);
        }
        else
        {
            this.BaseClassTypes.Add(key, new List<ClassTypeData>());
            this.BaseClassTypes[key].AddRange(value);
        }
    }
}