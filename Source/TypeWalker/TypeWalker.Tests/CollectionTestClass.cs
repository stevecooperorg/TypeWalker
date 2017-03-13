using System.Collections.Generic;

namespace CollectionTestClasses
{
    public class CollectionTestClass
    {
        public string[] NavigationArray { get; set; }
        public ICollection<string> NavigationCollection { get; set; }
        public List<string> NavigationList { get; set; }
    }
}