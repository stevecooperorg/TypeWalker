using System;
using System.Collections.Generic;
using TypeWalker;
using TypeWalker.Generators;

namespace AlternateNamespace
{
    public class DistinctClass
    {
        public NamespaceOfTestClasses.ReferencedClass Backreference { get; set; }
        public string Foo { get; set; }
    }
}

namespace NamespaceOfTestClasses
{
    public class BasicClass
    {
        public string StringField;

        public int GetterPrivateSetterString { get; private set; }

        [IgnoreForLanguageGenerator(KnockoutMappingGenerator.Id)]
        public string GetterSetterString { get; set; }

        public ICollection<ReferencedClass> NavigationArray { get; set; }
        public ReferencedClass NavigationProperty { get; set; }
        public ReferencedClass NavigationProperty2 { get; set; }
        public bool? NullableGetterSetterBool { get; set; }
        private DateTime PrivateGetterSetterDate { get; set; }
    }

    // we don't see this because it's generic and we'll only ever export fully-instantiated
    public class MyList<T> : List<T>
    {
    }

    public class ReferencedClass
    {
        public BasicClass BackReference { get; set; }
        public ReferencedClass SelfReference { get; set; }
    }

    public class Subclass : BasicClass
    {
        public string SubclassesOwnProperty { get; set; }
    }
}