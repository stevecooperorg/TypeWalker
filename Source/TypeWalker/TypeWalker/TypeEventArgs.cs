using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TypeWalker
{
    public class TypeEventArgs : EventArgs
    {
        public System.Type Type { get; set; }
        public string TypeName { get; set; }
        public ICollection<string> InterfaceNames { get; set; }
        public TypeEventArgs BaseTypeInfo { get; set; }
        public string NameSpaceName { get; set; }
        public string FullTypeName {  get { return NameSpaceName + "." + TypeName; } }
    }
}
