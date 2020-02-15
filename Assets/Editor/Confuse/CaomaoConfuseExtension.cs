using UnityEngine;
using Mono.Cecil;
using System;
public static class CaomaoConfuseExtension
{
    public static string GetScopeName(this TypeReference typeRef)
    {
        if (typeRef == null)
        {
            Debug.LogWarning("Type is null!");
            return null;
        }
        string result;
        try
        {
            ModuleDefinition module = typeRef.Scope as ModuleDefinition;
            if (module != null)
            {
                result = module.Assembly.Name.Name;
            }
            else
            {
                result = typeRef.Scope.Name;
            }
        }
        catch (Exception)
        {
            Debug.LogWarning("Cannot find the assembly name for the type: " + typeRef.FullName);
            result = null;
        }
        return result;
    }
}