using System;

namespace TypeWalker.Generators
{
    public abstract class Language
    {
        public abstract TypeInfo GetTypeInfo(Type type);
    }
}