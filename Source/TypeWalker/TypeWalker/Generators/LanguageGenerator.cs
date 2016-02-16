using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TypeWalker.Extensions;

namespace TypeWalker.Generators
{
    public abstract class LanguageGenerator
    {
        private readonly Language language;
        private readonly string id;

        protected LanguageGenerator(Language language, string id)
        {
            this.language = language;
            this.id = id;
        }

        public abstract string NamespaceStartFormat { get; }

        public abstract string NamespaceEndFormat { get; }

        public abstract string DerivedTypeStartFormat { get; }

        public abstract string TerminalTypeStartFormat { get; }

        public abstract string TypeEndFormat { get; }

        public abstract string MemberStartFormat { get; }

        public abstract string MemberEndFormat { get; }

        public abstract bool ExportsNonPublicMembers { get; }

        public string Generate(IEnumerable<Type> startingTypes)
        {
            var types = new List<TypeEventArgs>();
            var typeCollector = new Visitor();
            typeCollector.TypeVisited += (sender, args) => { types.Add(args); };
            typeCollector.Visit(startingTypes, this.language);

            types.Sort((t1, t2) => string.Compare(t1.FullTypeName, t2.FullTypeName, StringComparison.InvariantCultureIgnoreCase));

            var allTypes = types.Select(t => t.Type).ToList();

            var trace = new StringBuilder();
            var visitor = new Visitor();

            visitor.NameSpaceVisiting += (sender, args) => { trace.AppendFormatObject(NamespaceStartFormat, args); };
            visitor.NameSpaceVisited += (sender, args) => { trace.AppendFormatObject(NamespaceEndFormat, args); };

            visitor.TypeVisiting += (sender, args) => {
                if (args.BaseTypeInfo != null)
                {
                    trace.AppendFormatObject(DerivedTypeStartFormat, args);
                }
                else
                {
                    trace.AppendFormatObject(TerminalTypeStartFormat, args);
                }
            };
            visitor.TypeVisited += (sender, args) => { trace.AppendFormatObject(TypeEndFormat, args); };

            Func<MemberEventArgs, bool> include = args =>
                (this.ExportsNonPublicMembers || args.IsPublic) &&  args.IsOwnProperty && !args.IgnoredByGenerators.Contains(this.id);

            visitor.MemberVisiting += (sender, args) => {
                if (include(args))
                {
                    trace.AppendFormatObject(MemberStartFormat, args);
                }
            };

            visitor.MemberVisited += (sender, args) =>
            {
                if (include(args))
                {
                    trace.AppendFormatObject(MemberEndFormat, args);
                };
            };

            visitor.Visit(allTypes, this.language);

            var languageOutput = trace.ToString().Trim();

            return languageOutput;
        }
    }
}