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

            switch (type.FullName)
            {
                case "System.String": return new TypeInfo("string", string.Empty);
                case "System.Int32": return new TypeInfo("number", string.Empty);
                case "System.Int64": return new TypeInfo("number", string.Empty);
                case "System.Int16": return new TypeInfo("number", string.Empty);
                case "System.Boolean": return new TypeInfo("boolean", string.Empty);
            }

            // for now, uses a C# style for anything else
            string typeName = type.FullName.Replace(type.Namespace + ".", "");
            var typeReference = new System.CodeDom.CodeTypeReference(typeName);
            var nameSpace = type.Namespace != "System" ? type.Namespace : "";
            return new TypeInfo(typeName, nameSpace);
        }
    }
}