using System;

namespace TypeWalker.Generators
{
    public class KnockoutMappingGenerator : LanguageGenerator
    {
        public const string Id = "KnockoutMapping";

        private class KnockoutLanguage: Language
        {
            TypeScriptLanguage typescript;
            string namespacePrefix;

            public KnockoutLanguage(string namespacePrefix)
            {
                this.typescript = new TypeScriptLanguage();
                this.namespacePrefix = namespacePrefix;
            }

            public override TypeInfo GetTypeInfo(Type type)
            {
                var info = this.typescript.GetTypeInfo(type);
                if (!string.IsNullOrWhiteSpace(info.NameSpaceName))
                {
                    info.NameSpaceName = this.namespacePrefix + "." + info.NameSpaceName;
                }

                return info;
            }
        }

        public KnockoutMappingGenerator(string namespacePrefix)
            : base(new KnockoutLanguage(namespacePrefix), Id)
        {
        }

        public override string NamespaceStartFormat
        {
            get
            { 
                return "/* {Comment} */" + Environment.NewLine + "declare module {NameSpaceName} {{" + Environment.NewLine; 
            }
        }

        public override string NamespaceEndFormat
        { 
            get
            {
                return "}}" + Environment.NewLine + Environment.NewLine; 
            }
        }

        public override string TypeStartFormat 
        {
            get 
            { 
                return "    export interface {TypeName} {{" + Environment.NewLine; 
            }
        }

        public override string TypeEndFormat { get { return "    }}" + Environment.NewLine; } }

        public override string MemberStartFormat
        {
            get
            {
                return
            "        {MemberName}(): {MemberTypeFullName};" + Environment.NewLine +
            "        {MemberName}(value: {MemberTypeFullName}): void;" + Environment.NewLine;
            }
        }

        public override string MemberEndFormat 
        {
            get
            {
                return string.Empty; 
            }
        }

        public override bool ExportsNonPublicMembers
        {
            get { return false ; }
        }
    }
}