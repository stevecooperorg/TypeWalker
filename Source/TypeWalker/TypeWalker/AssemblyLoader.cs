using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TypeWalker.Extensions;

namespace TypeWalker
{
    public class AssemblyLoader
    {
        private IRuntime runtime;

        public AssemblyLoader(IRuntime runtime)
        {
            this.runtime = runtime;
        }

        public Assembly Load(string name)
        {
            var assembly = AppDomain.CurrentDomain.Load(name); // Assembly.ReflectionOnlyLoad(name);
            return assembly;
        }

        public static AssemblyLoader FromExecutingAssembly(IRuntime runtime)
        {
            var loader = new AssemblyLoader(runtime);
            return loader;
        }
    }
}
