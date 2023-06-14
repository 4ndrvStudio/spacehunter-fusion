using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SUI.BCS
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Numerics;

    public class BcsEncoder
    {
        
        private Dictionary<string, TypeInterface> types = new Dictionary<string, TypeInterface>();

        public void RegisterType<T>(string typeName, Func<BcsWriter, T, object, object[], object> encodeCallback, Func<object, bool> validateCallback = null)
        {
            TypeInterface typeInterface = new TypeInterface();
            typeInterface.EncodeCallback = (writer, data, options, parameters) => encodeCallback(writer, (T)data, options, parameters);
            typeInterface.ValidateCallback = validateCallback ?? (obj => true);
            types[typeName] = typeInterface;
        }

        public byte[] Serialize(string typeName, object data, object options = null)
        {
            if (typeName is string || typeName is Array)
            {
                (string name, object[] parameters) = ParseTypeName(typeName);
                TypeInterface typeInterface = GetTypeInterface(name);
                BcsWriter writer = new BcsWriter();
                typeInterface.EncodeCallback(writer, data, options, parameters);
                return writer.ToBytes();
            }
            throw new ArgumentException($"Invalid type name: {typeName}");
        }

        private (string, object[]) ParseTypeName(object name)
        {
            if (name is Array nameArray)
            {
                string _typeName = (string)nameArray.GetValue(0);
                object[] parameters = new object[nameArray.Length - 1];
                Array.Copy(nameArray, 1, parameters, 0, nameArray.Length - 1);
                return (_typeName, parameters);
            }
            if (name is string typeName)
            {
                return (typeName, new object[0]);
            }
            throw new ArgumentException($"Illegal type passed as a name of the type: {name}");
        }

        private TypeInterface GetTypeInterface(string typeName)
        {
            if (types.TryGetValue(typeName, out TypeInterface typeInterface))
            {
                List<string> chain = new List<string>();
                while (typeInterface is string)
                {
                    if (chain.Contains(typeInterface.ToString()))
                    {
                        throw new Exception($"Recursive definition found: {string.Join(" -> ", chain)} -> {typeInterface}");
                    }
                    chain.Add(typeInterface.ToString());
                    typeInterface = types[typeInterface.ToString()];
                }
                if (typeInterface == null)
                {
                    throw new Exception($"Type {typeName} is not registered");
                }
                return typeInterface;
            }
            throw new Exception($"Type {typeName} is not registered");
        }

        private class TypeInterface
        {
            public Func<BcsWriter, object, object, object[], object> EncodeCallback { get; set; }
            public Func<object, bool> ValidateCallback { get; set; }
        }
    }

  public class BcsWriter
{
    private MemoryStream memoryStream;
    private BinaryWriter binaryWriter;

    public BcsWriter()
    {
        memoryStream = new MemoryStream();
        binaryWriter = new BinaryWriter(memoryStream);
    }

    public void WriteByte(byte value)
    {
        binaryWriter.Write(value);
    }

    public void WriteUInt16(ushort value)
    {
        binaryWriter.Write(value);
    }

    public void WriteUInt32(uint value)
    {
        binaryWriter.Write(value);
    }

    public void WriteUInt64(ulong value)
    {
        binaryWriter.Write(value);
    }

    public void WriteBytes(byte[] bytes)
    {
        binaryWriter.Write(bytes);
    }

    public BcsWriter WriteVec<T>(List<T> vector, Action<BcsWriter, T, int, int> cb)
    {
        WriteULEB((ulong)vector.Count);
        for (int i = 0; i < vector.Count; i++)
        {
            cb(this, vector[i], i, vector.Count);
        }
        return this;
    }

    public BcsWriter WriteULEB(ulong value)
    {
        var encodedValue = UlebEncode(value);
        foreach (var el in encodedValue)
        {
            WriteByte(el);
        }
        return this;
    }

    private byte[] UlebEncode(ulong num)
    {
        var list = new List<byte>();
        if (num == 0)
        {
            return new byte[] { 0 };
        }
        while (num > 0)
        {
            byte el = (byte)(num & 127);
            num >>= 7;
            if (num > 0)
            {
                el |= 128;
            }
            list.Add(el);
        }
        return list.ToArray();
    }

    public byte[] ToBytes()
    {
        binaryWriter.Flush();
        return memoryStream.ToArray();
    }
}



}

