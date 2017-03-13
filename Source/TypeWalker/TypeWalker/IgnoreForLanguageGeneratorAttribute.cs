using System;

namespace TypeWalker
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class IgnoreForLanguageGeneratorAttribute : Attribute
    {
        public IgnoreForLanguageGeneratorAttribute(string languageId)
        {
            this.LanguageId = languageId;
        }

        public string LanguageId { get; private set; }
    }
}