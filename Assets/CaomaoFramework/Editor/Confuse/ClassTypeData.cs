using Mono.Cecil;
using UnityEngine;
using System;
public class ClassTypeData
{
  
    private string m_sScope;
    private string m_orginalNamespace;
    public string FullName;
    public string HashName;
    public int HashCode;
    private TypeReference m_oTypeReference;

    public ClassTypeData(TypeReference type)
    {
        this.m_oTypeReference = type;
        this.HashName = string.IsNullOrEmpty(this.m_oTypeReference.Namespace) ? this.m_oTypeReference.Name
            : (this.m_oTypeReference.Namespace + "." + this.m_oTypeReference.Name);
        var declaringType = this.m_oTypeReference;
        while (declaringType.DeclaringType != null)
        {
            declaringType = declaringType.DeclaringType;
            this.HashName = declaringType.Name + "/" + this.HashName;           
        }
        this.m_orginalNamespace = declaringType.Namespace;
        if (!string.IsNullOrEmpty(this.m_orginalNamespace) && this.m_orginalNamespace != this.m_oTypeReference.Namespace)
        {
            this.FullName = this.m_orginalNamespace + "." + this.HashName;
        }
        else
        {
            this.FullName = this.HashName;
        }
        this.HashCode = this.CalculateHashCode();
    }

    public TypeDefinition TypeDefinition
    {
        get
        {
            return this.m_oTypeReference as TypeDefinition;
        }
    }
    public string Scope
    {
        get
        {
            if (string.IsNullOrEmpty(this.m_sScope) == false)
            {
                return this.m_sScope;
            }
            if (this.m_oTypeReference == null)
            {
                Debug.LogWarning("Type == null");
                return null;
            }
            try
            {
                var module = this.m_oTypeReference.Scope as ModuleDefinition;
                if (module != null)
                {
                    this.m_sScope = module.Assembly.Name.Name;
                }
                else
                {
                    this.m_sScope = this.m_oTypeReference.Scope.Name;
                }
            }
            catch (Exception e)
            {
                this.m_sScope = null;
                Debug.LogException(e);
            }
            return this.m_sScope;
        }
    }
    public bool bIsMonoBehavior;

    private int CalculateHashCode()
    {
        return this.Scope.GetHashCode() ^ this.m_orginalNamespace.GetHashCode()
            ^ this.FullName.GetHashCode() ^ this.HashName.GetHashCode();
    }

}