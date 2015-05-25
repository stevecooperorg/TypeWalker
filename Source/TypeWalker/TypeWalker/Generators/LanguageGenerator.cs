using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TypeWalker.Extensions;

namespace TypeWalker.Generators
{
    public abstract class LanguageGenerator
    {
        private readonly Language language;
        private readonly string id;

        public LanguageGenerator(Language language, string id)
        {
            this.language = language;
            this.id = id;
        }

        public abstract string NamespaceStartFormat { get; }

        public abstract string NamespaceEndFormat { get; }

        public abstract string TypeStartFormat { get; }

        public abstract string TypeEndFormat { get; }

        public abstract string MemberStartFormat { get; }

        public abstract string MemberEndFormat { get; }


        public string Generate(IEnumerable<Type> startingTypes)
        {
            var trace = new StringBuilder();
            var visitor = new Visitor();

            visitor.NameSpaceVisiting += (sender, args) => { trace.AppendFormatObject(NamespaceStartFormat, args); };
            visitor.NameSpaceVisited += (sender, args) => { trace.AppendFormatObject(NamespaceEndFormat, args); };

            visitor.TypeVisiting += (sender, args) => { trace.AppendFormatObject(TypeStartFormat, args); };
            visitor.TypeVisited += (sender, args) => { trace.AppendFormatObject(TypeEndFormat, args); };

            visitor.MemberVisiting += (sender, args) => { trace.AppendFormatObject(MemberStartFormat, args); };
            visitor.MemberVisited += (sender, args) => { trace.AppendFormatObject(MemberEndFormat, args); };

            visitor.Visit(startingTypes, this.language);

            var languageOutput = trace.ToString().Trim();

            return languageOutput;
        }
    }
}
