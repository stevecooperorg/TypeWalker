namespace TypeWalker.Generators
{
    /// <summary>
    /// C# rules for code gen.
    /// </summary>
    public class CSharpLanguage : Language
    {
        private static System.CodeDom.Compiler.CodeDomProvider codeDomProvider;

        static CSharpLanguage()
        {
            CSharpLanguage.codeDomProvider = System.CodeDom
                .Compiler
                .CodeDomProvider
                .CreateProvider("CSharp");
        }

        /// <summary>
        /// The name of a type in standard C# format.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>
        /// The name.
        /// </returns>
        public override TypeInfo GetTypeInfo(System.Type type)
        {
            string typeName = type.FullName.Replace(type.Namespace + ".", "");
            var typeReference = new System.CodeDom.CodeTypeReference(typeName);
            var nameSpace = type.Namespace != "System" ? type.Namespace : "";
            var result = nameSpace + CSharpLanguage.codeDomProvider.GetTypeOutput(typeReference);
            return new TypeInfo(typeName, nameSpace);
        }
    }
}