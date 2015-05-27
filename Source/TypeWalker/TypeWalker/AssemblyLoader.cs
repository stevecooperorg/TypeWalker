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
        private AppDomain domain;

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
            var assembly = domain.Load(name); // Assembly.ReflectionOnlyLoad(name);
            return assembly;
        }

        public void WireEvents()
        {
            var evidence = AppDomain.CurrentDomain.Evidence;
            var domainInfo = new AppDomainSetup
            {
                ApplicationBase = this.binDir,
                ApplicationName = "assemblyLoader",
                
            };

            this.domain = AppDomain.CurrentDomain;//.CreateDomain("assemblyLoader", evidence, domainInfo);

            if (eventsWired) { return; }

            //this.domain.ReflectionOnlyAssemblyResolve += (sender, args) =>
            //{
            //    return domain.Load(args.Name);
            //    //return Load(args.Name);
            //};

            this.domain.AssemblyResolve += (sender, args) =>
            {
                return domain.Load(args.Name);
                //return Load(args.Name);
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
