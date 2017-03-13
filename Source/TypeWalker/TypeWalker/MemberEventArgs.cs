using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace TypeWalker
{
    [DebuggerDisplay("{MemberName} on {MemberTypeName} from ns {MemberTypeNameSpaceName}")]
    public class MemberEventArgs : EventArgs
    {
        public MemberEventArgs()
        {
            this.IgnoredByGenerators = new List<string>();
            this.IsPublic = true;
            this.IsOwnProperty = true;
        }

        public ICollection<string> IgnoredByGenerators { get; set; }

        /// <summary>
        /// Is this member declared on the class we're currently visiting, or is on the base?
        /// </summary>
        public bool IsOwnProperty { get; set; }

        public bool IsPublic { get; set; }
        public string MemberName { get; set; }

        public string MemberTypeFullName { get; set; }
        public string MemberTypeName { get; set; }
        public string MemberTypeNameSpaceName { get; set; }
    }
}