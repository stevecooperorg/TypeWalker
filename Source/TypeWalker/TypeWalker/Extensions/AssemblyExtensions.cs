using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace TypeWalker.Extensions
{
    public static class AssemblyExtensions
    {
        public static string GetAssemblyDirectory(this Assembly assembly)
        {
            var codebase = assembly.GetAssemblyPath();
            var directory = Path.GetDirectoryName(codebase);
            return directory;
        }

        public static string GetAssemblyPath(this Assembly assembly)
        {
            var codebase = new Uri(assembly.CodeBase).AbsolutePath;
            return codebase;
        }
    }
}
