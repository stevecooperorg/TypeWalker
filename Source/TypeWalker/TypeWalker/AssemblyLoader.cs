using System;
using System.Reflection;

namespace TypeWalker
{
    public class AssemblyLoader
    {
        private IRuntime runtime;

        public AssemblyLoader(IRuntime runtime)
        {
            this.runtime = runtime;
        }

        public static AssemblyLoader FromExecutingAssembly(IRuntime runtime)
        {
            var loader = new AssemblyLoader(runtime);
            return loader;
        }

        public Assembly Load(string name)
        {
            var assembly = AppDomain.CurrentDomain.Load(name); // Assembly.ReflectionOnlyLoad(name);
            return assembly;
        }
    }
}