using UnityEngine;
using System;
using UnityEditor;
using System.Collections.Generic;
using Mono.Cecil;
using Mono.Cecil.Mdb;
using Mono.Cecil.Pdb;
using Mono.Cecil.Rocks;
using System.IO;
public class CaomaoConfuseMachine
{
    private List<CaomaoAssemblyFileData> AssemblyFileInfo = new List<CaomaoAssemblyFileData>();
    private HashSet<string> AssemblyDependencySearchPaths = new HashSet<string>();
    private DefaultAssemblyResolver Resolver = new DefaultAssemblyResolver();
    private List<CaomaoAssemblyInfo> Assemblys = new List<CaomaoAssemblyInfo>();
    private CaomaoConfuseInheritData InheritData = new CaomaoConfuseInheritData();
    public CaomaoConfuseMachine()
    {
        if (this.Resolver == null)
        {
            return;
        }
        foreach (var path in this.AssemblyDependencySearchPaths)
        {
            if (!string.IsNullOrEmpty(path))
            {
                this.Resolver.AddSearchDirectory(path);
            }            
        }
        foreach (var assemble in this.AssemblyFileInfo)
        {
            if (!string.IsNullOrEmpty(assemble.Path))
            {
                this.Resolver.AddSearchDirectory(Path.GetDirectoryName(assemble.Path));
            }
        }
    }

    public void LoadAssembly()
    {
        foreach (var assemblyData in this.AssemblyFileInfo)
        {
            try
            {
                if (File.Exists(assemblyData.Path) == false)
                {
                    Debug.LogWarning($"不存在程序集{assemblyData.Path}");
                    continue;
                }
                if (EditorUserBuildSettings.activeBuildTarget == BuildTarget.WSAPlayer)
                {
                    var pdbFilePath = Path.ChangeExtension(assemblyData.Path, ".pdb");
                    File.Delete(pdbFilePath);
                }
                var info = new CaomaoAssemblyInfo();
                info.AssemblyDefinition = this.ReadAssembly(assemblyData.Path);
                this.Assemblys.Add(info);
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }        
        }
    }
    public AssemblyDefinition ReadAssembly(string assemblyPath)
    {
        var readerParam = this.GetReaderParameters(assemblyPath);
        return AssemblyDefinition.ReadAssembly(assemblyPath, readerParam);
    }
    public void InitClassTypeData()
    {
        foreach (var ssembly in this.Assemblys)
        {
            foreach (var type in ssembly.AllTypeDefinition)
            {
                if (type.FullName == "<Module>")
                {
                    continue;
                }
                //Class
                HashSet<ClassTypeData> baseClasses = new HashSet<ClassTypeData>();
                this.InitBaseType(baseClasses, type);
                this.InheritData.AddClassData(new ClassTypeData(type), baseClasses);
                //Method
                HashSet<MethodData> methodInfos = new HashSet<MethodData>();

            }
        }
    }
    private void InitBaseType(HashSet<ClassTypeData> classes,TypeDefinition type)
    {
        foreach (var iface in type.Interfaces)
        {
            var ifaceTypeDef = this.GetTypeDefinition(iface.InterfaceType);
            if (ifaceTypeDef != null)
            {
                this.InitBaseType(classes,ifaceTypeDef);
                classes.Add(new ClassTypeData(ifaceTypeDef));
            }
        }
        TypeDefinition baseType = this.GetTypeDefinition(type.BaseType);
        if (baseType != null && baseType.FullName != "System.Object")
        {
            this.InitBaseType(classes, baseType);
            classes.Add(new ClassTypeData(baseType));
        }
    }
    private void InitMethodData(HashSet<MethodData> methods,TypeDefinition type)
    {
        foreach (var iface in type.Interfaces)
        {
            var ifaceTypeDef = this.GetTypeDefinition(iface.InterfaceType);
            if (ifaceTypeDef != null)
            {
                this.InitMethodData(methods, ifaceTypeDef);
            }
            else
            {
                
            }
        }
    }
    private TypeDefinition GetTypeDefinition(TypeReference type)
    {
        if (type == null)
        {
            return null;
        }
        TypeDefinition typeDef = type as TypeDefinition;
        if (typeDef == null)
        {
            var assemblyName = type.GetScopeName();
            if (string.IsNullOrEmpty(assemblyName) == false)
            {
                foreach (var assembly in this.Assemblys)
                {
                    if (assembly.AssemblyName == assemblyName)
                    {
                        typeDef = assembly.AssemblyDefinition.
                            MainModule.GetType($"{type.Namespace}.{type.Name}");
                    }
                }
            }          
        }
        return typeDef;
    }
    private ReaderParameters GetReaderParameters(string filePath)
    {
        ReaderParameters result = new ReaderParameters();
        result.InMemory = true;
        result.ReadWrite = true;
        result.AssemblyResolver = this.Resolver;
        if (File.Exists(Path.ChangeExtension(filePath, ".dll.mdb")))
        {
            result.ReadSymbols = true;
            result.SymbolReaderProvider = new MdbReaderProvider();
        }
        if (File.Exists(Path.ChangeExtension(filePath, ".pdb")))
        {
            result.ReadSymbols = true;
            result.SymbolReaderProvider = new PdbReaderProvider();
        }
        result.ReadSymbols = false;
        result.SymbolReaderProvider = null;
        return result;
    }
}
