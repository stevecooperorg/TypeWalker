using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TypeWalker.Tests
{
    public class BasicClass
    {
        public string GetterSetterString { get; set; }
        public int GetterPrivateSetterString { get; private set; }
        private DateTime PrivateGetterSetterDate { get; set; }
        public bool? NullableGetterSetterBool { get; set; }

        public string StringField;


    }
}
