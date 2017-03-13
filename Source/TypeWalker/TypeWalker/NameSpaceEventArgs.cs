using System;

namespace TypeWalker
{
    public class NameSpaceEventArgs : EventArgs
    {
        public string Comment { get; set; }
        public string NameSpaceName { get; set; }
    }
}