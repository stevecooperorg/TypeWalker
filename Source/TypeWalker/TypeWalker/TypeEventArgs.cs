using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TypeWalker
{
    public class TypeEventArgs : EventArgs
    {
        public string TypeName { get; set; }
        public string NameSpaceName { get; set; }
    }
}
