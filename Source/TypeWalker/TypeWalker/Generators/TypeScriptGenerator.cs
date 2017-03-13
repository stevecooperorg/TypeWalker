using System;

namespace TypeWalker.Generators
{
    public class TypeScriptGenerator : LanguageGenerator
    {
        public const string Id = "TypeScript";

        public TypeScriptGenerator() : base(new TypeScriptLanguage(), Id)
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
                return "        {MemberName}: {MemberTypeFullName};" + Environment.NewLine;
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

        public override string TypeEndFormat
        {
            get
            {
                return "    }}" + Environment.NewLine;
            }
        }
    }
}