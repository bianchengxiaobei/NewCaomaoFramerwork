using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Mono.Cecil;
using Mono.Cecil.Rocks;
public class CaomaoAssemblyInfo
{
    private IEnumerable<TypeDefinition> m_AllTypeDefinitions;
    public AssemblyDefinition AssemblyDefinition { get; set; }

    public IEnumerable<TypeDefinition> AllTypeDefinition
    {
        get
        {
            if (this.m_AllTypeDefinitions == null)
            {
                this.m_AllTypeDefinitions = this.AssemblyDefinition.MainModule.GetAllTypes();
            }
            return this.m_AllTypeDefinitions;
        }
    }
    public string AssemblyName
    {
        get
        {
            return this.AssemblyDefinition.Name.Name;
        }
    }
}