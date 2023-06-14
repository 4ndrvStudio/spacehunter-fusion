using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SUI.BCS
{
    using System;
    
public class TypeInterface
{
    public string TypeName { get; set; }
    public List<string> Chain { get; set; }
}

public class TypeRegistry
{
    private Dictionary<string, TypeInterface> types;

    public TypeInterface GetTypeInterface(string type)
    {
        if (types.TryGetValue(type, out var typeInterface))
        {
            if (typeInterface.Chain != null)
            {
                var chain = new List<string>();
                while (typeInterface.Chain != null)
                {
                    if (chain.Contains(typeInterface.TypeName))
                    {
                        throw new Exception($"Recursive definition found: {string.Join(" -> ", chain)} -> {typeInterface.TypeName}");
                    }
                    chain.Add(typeInterface.TypeName);
                    typeInterface = types[typeInterface.TypeName];
                }
            }
            if (typeInterface == null)
            {
                throw new Exception($"Type {type} is not registered");
            }
            return new TypeInterface { TypeName = type, Chain = typeInterface.Chain };
        }
        throw new Exception($"Type {type} is not registered");
    }

    public TypeRegistry()
    {
        types = new Dictionary<string, TypeInterface>();
    }
}
}
