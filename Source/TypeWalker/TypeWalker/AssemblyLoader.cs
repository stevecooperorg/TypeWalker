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
        private readonly string binDir;
        private bool eventsWired = false;
        private IRuntime runtime;

        public AssemblyLoader(string binDir, IRuntime runtime)
        {
            this.binDir = binDir;
            this.runtime = runtime;
        }

        public Assembly Load(string name)
        {
            var assembly = Assembly.ReflectionOnlyLoad(name);
            return assembly;
        }

        public void WireEvents()
        {
            if (eventsWired) { return; }

            System.AppDomain.CurrentDomain.ReflectionOnlyAssemblyResolve += (sender, args) =>
            {
                return Load(args.Name);
            };

            eventsWired = true; 
        }

        public static AssemblyLoader FromExecutingAssembly(IRuntime runtime)
        {
            var binDir = Assembly.GetExecutingAssembly().GetAssemblyDirectory();
            var loader = new AssemblyLoader(binDir, runtime);
            return loader;
        }
    }
}
