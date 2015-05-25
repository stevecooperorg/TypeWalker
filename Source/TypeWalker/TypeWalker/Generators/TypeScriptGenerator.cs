using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TypeWalker.Generators
{
    public class TypeScriptGenerator: LanguageGenerator
    {
        public const string Id = "TypeScript";

        public TypeScriptGenerator(): base(new TypeScriptLanguage(), Id)
        {
        }

        public override string NamespaceStartFormat
        {
            get
            {
                return "declare module {NameSpaceName} {{" + Environment.NewLine;
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

        public override string TypeEndFormat
        {
            get
            {
                return "    }}" + Environment.NewLine;
            }
        }

        public override string MemberStartFormat
        {
            get
            {
                return "        {MemberName}: {MemberTypeFullName};" + Environment.NewLine;
            }
        }

        public override string MemberEndFormat
        {
            get
            {
                return string.Empty;
            }
        }
    }
}
