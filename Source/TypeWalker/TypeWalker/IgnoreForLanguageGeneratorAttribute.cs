using System;

namespace TypeWalker
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class IgnoreForLanguageGeneratorAttribute : Attribute
    {
        public string LanguageId { get; private set; }

        public IgnoreForLanguageGeneratorAttribute(string languageId)
        {
            this.LanguageId = languageId;
        }
    }
}