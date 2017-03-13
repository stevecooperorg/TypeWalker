using System;

namespace TypeWalker.Generators
{
    public class KnockoutMappingGenerator : LanguageGenerator
    {
        public const string Id = "KnockoutMapping";

        public KnockoutMappingGenerator(string namespacePrefix)
            : base(new KnockoutLanguage(namespacePrefix), Id)
        {
        }

        public override string DerivedTypeStartFormat
        {
            get
            {
                return "    export interface {TypeName} extends {BaseTypeInfo.NameSpaceName}.{BaseTypeInfo.TypeName} {{" + Environment.NewLine;
            }
        }

        public override bool ExportsNonPublicMembers
        {
            get { return false; }
        }

        public override string MemberEndFormat
        {
            get
            {
                return string.Empty;
            }
        }

        public override string MemberStartFormat
        {
            get
            {
                return
            "        {MemberName}(): {MemberTypeFullName};" + Environment.NewLine +
            "        {MemberName}(value: {MemberTypeFullName}): void;" + Environment.NewLine;
            }
        }

        public override string NamespaceEndFormat
        {
            get
            {
                return "}}" + Environment.NewLine + Environment.NewLine;
            }
        }

        public override string NamespaceStartFormat
        {
            get
            {
                return "/* {Comment} */" + Environment.NewLine + "declare module {NameSpaceName} {{" + Environment.NewLine;
            }
        }

        public override string TerminalTypeStartFormat
        {
            get
            {
                return "    export interface {TypeName} {{" + Environment.NewLine;
            }
        }

        public override string TypeEndFormat { get { return "    }}" + Environment.NewLine; } }

        private class KnockoutLanguage : Language
        {
            private string namespacePrefix;
            private TypeScriptLanguage typescript;

            public KnockoutLanguage(string namespacePrefix)
            {
                this.typescript = new TypeScriptLanguage();
                this.namespacePrefix = namespacePrefix;
            }

            public override TypeInfo GetTypeInfo(Type type)
            {
                var original = this.typescript.GetTypeInfo(type);

                var correctNamespace = string.IsNullOrWhiteSpace(original.NameSpaceName)
                    ? original.NameSpaceName
                    : this.namespacePrefix + "." + original.NameSpaceName;

                var info = new TypeInfo(original.Name, correctNamespace);

                return info;
            }
        }
    }
}