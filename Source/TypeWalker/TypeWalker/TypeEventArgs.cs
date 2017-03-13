using System;
using System.Collections.Generic;

namespace TypeWalker
{
    public class TypeEventArgs : EventArgs
    {
        public TypeEventArgs BaseTypeInfo { get; set; }
        public string FullTypeName { get { return NameSpaceName + "." + TypeName; } }
        public ICollection<string> InterfaceNames { get; set; }
        public string NameSpaceName { get; set; }
        public System.Type Type { get; set; }
        public string TypeName { get; set; }
    }
}