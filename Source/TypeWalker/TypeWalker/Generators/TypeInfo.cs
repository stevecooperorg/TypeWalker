﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TypeWalker.Generators
{

    public class TypeInfo
    {
        public string Name { get; set; }
        public string NameSpaceName { get; set; }
        public TypeInfo(string name, string nameSpaceName)
        {
            this.Name = name;
            this.NameSpaceName = nameSpaceName;
        }

        public string FullName
        {
            get
            {
                if (string.IsNullOrWhiteSpace(this.NameSpaceName))
                {
                    return Name;
                }
                else
                {
                    return string.Format("{0}.{1}", this.NameSpaceName, this.Name);
                }
            }
        }
    }
}
