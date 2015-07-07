using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TypeWalker;
using TypeWalker.Generators;

namespace NamespaceOfTestClasses
{
    // we don't see this because it's generic and we'll only ever export fully-instantiated
    public class MyList<T> : List<T>
    {
    }

    public class Subclass : BasicClass
    {
        public string SubclassesOwnProperty { get; set; }
    }

    public class BasicClass
    {
        [IgnoreForLanguageGenerator(KnockoutMappingGenerator.Id)]
        public string GetterSetterString { get; set; }
        public int GetterPrivateSetterString { get; private set; }
        private DateTime PrivateGetterSetterDate { get; set; }
        public bool? NullableGetterSetterBool { get; set; }

        public ICollection<ReferencedClass> NavigationArray { get; set; }

        public string StringField;

        public ReferencedClass NavigationProperty { get; set; }

        public ReferencedClass NavigationProperty2 { get; set; }
    }

    public class ReferencedClass
    {
        public ReferencedClass SelfReference { get; set; }
        public BasicClass BackReference { get; set; }

    }
}

namespace AlternateNamespace
{
    public class DistinctClass
    {
        public string Foo { get; set; }
        public NamespaceOfTestClasses.ReferencedClass Backreference { get; set; }
    }
}