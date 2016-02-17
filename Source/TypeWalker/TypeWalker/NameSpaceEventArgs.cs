using System;

namespace TypeWalker
{
    public class NameSpaceEventArgs : EventArgs
    {
        public string NameSpaceName { get; set; }
        public string Comment { get; set; }
    }
}