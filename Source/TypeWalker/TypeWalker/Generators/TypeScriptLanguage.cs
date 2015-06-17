using System;
using TypeWalker.Extensions;

namespace TypeWalker.Generators
{
    public class TypeScriptLanguage : Language
    {
        private static System.CodeDom.Compiler.CodeDomProvider codeDomProvider;

        static TypeScriptLanguage()
        {
            TypeScriptLanguage.codeDomProvider = System.CodeDom
                .Compiler
                .CodeDomProvider
                .CreateProvider("CSharp");
        }

        public override TypeInfo GetTypeInfo(Type type)
        {
            if (type.IsNullableType())
            {
                var baseType = type.GenericTypeArguments[0];
                return GetTypeInfo(baseType);
            }

            string typescriptName;
            if (type.IsArrayType())
            {
                var elementType = type.GetElementType();
                var typeInfo = GetTypeInfo(elementType);
                var arrayResult = new TypeInfo(typeInfo.Name + "[]", typeInfo.NameSpaceName);
                return arrayResult;
            }
            else if (type.IsGenericEnumerableType() && type != typeof(string))
            {
                return new TypeInfo("generic[]", string.Empty);
            }
            else if (type.IsDictionaryType())
            {
                return new TypeInfo("any", string.Empty);
            }
            else if (type.IsEnum)
            {
                return new TypeInfo("number", string.Empty);
            }
            else if (type.IsEnumerableType() && type != typeof(string))
            {
                return new TypeInfo("any[]", string.Empty);
            }
            else if (TryGetTypeName(type.FullName, out typescriptName)) 
            {
                return new TypeInfo(typescriptName, string.Empty);
            }
            else
            {
                // for now, uses a C# style for anything else
                string typeName = type.FullName.Replace(type.Namespace + ".", "");
                var typeReference = new System.CodeDom.CodeTypeReference(typeName);
                var nameSpace = type.Namespace != "System" ? type.Namespace : "";
                return new TypeInfo(typeName, nameSpace);
            }
        }

        private bool TryGetTypeName(string typeFullName, out string typescriptName)
        {
            switch (typeFullName)
            {
                case "System.String": typescriptName = "string"; return true;
                case "System.Byte": typescriptName = "string"; return true;
                case "System.Int32": typescriptName = "number"; return true;
                case "System.Int64": typescriptName = "number"; return true;
                case "System.Int16": typescriptName = "number"; return true;
                case "System.Boolean": typescriptName = "boolean"; return true;
                case "System.DateTime": typescriptName = "Date"; return true;
                default: typescriptName = null; return false;
            }
        }
    }
}