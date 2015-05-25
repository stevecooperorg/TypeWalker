using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TypeWalker
{
    public class MemberEventArgs : EventArgs
    {
        public string MemberName { get; set; }

        public string MemberTypeName { get; set; }

        public string MemberTypeFullName { get; set; }

        public string MemberTypeNameSpaceName { get; set; }

        public bool IsPublic { get; set; }

        public ICollection<string> IgnoredByGenerators { get; private set; }

        public MemberEventArgs ()
	    {
            this.IgnoredByGenerators = new List<string>();
	    }
    }
}
