using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
