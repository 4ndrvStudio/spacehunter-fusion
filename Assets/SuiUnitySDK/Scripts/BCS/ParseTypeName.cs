using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SUI.BCS
{
    using System;
    using System.Linq;
    public class TypeName
    {
        public string Name { get; set; }
        public List<string> Params { get; set; }
    }

    public class TypeParser
    {
        private string[] genericSeparators;

        public TypeName ParseTypeName(object name)
        {
            if (name is object[] nameArray)
            {
                return ParseArrayTypeName(nameArray);
            }
            else if (name is string nameString)
            {
                return ParseStringTypeName(nameString);
            }
            else
            {
                throw new Exception($"Illegal type passed as a name of the type: {name}");
            }
        }

        private TypeName ParseArrayTypeName(object[] nameArray)
        {
            var typeName = (string)nameArray[0];
            var parameters = nameArray.Skip(1).Select(p => (string)p).ToList();

            return new TypeName { Name = typeName, Params = parameters };
        }

        private TypeName ParseStringTypeName(string name)
        {
            if (genericSeparators == null)
            {
                genericSeparators = new string[] { "<", ">" };
            }

            var lBound = name.IndexOf(genericSeparators[0]);
             var rBound = name.ToCharArray().Reverse().ToList().IndexOf(genericSeparators[1].ToCharArray()[0]);

            if (lBound == -1 && rBound == -1)
            {
                return new TypeName { Name = name, Params = new List<string>() };
            }

            if (lBound == -1 || rBound == -1)
            {
                throw new Exception($"Unclosed generic in name '{name}'");
            }

            var typeName = name.Substring(0, lBound);
            var parameters = name.Substring(lBound + 1, name.Length - lBound - rBound - 2)
                .Split(',')
                .Select(p => p.Trim())
                .ToList();

            return new TypeName { Name = typeName, Params = parameters };
        }
    }

}
