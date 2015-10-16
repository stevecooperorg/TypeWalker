using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollectionTestClasses
{
    public class CollectionTestClass
    {
        public string[] NavigationArray { get; set; }
        public ICollection<string> NavigationCollection { get; set; }
        public List<string> NavigationList { get; set; }
    }
}
