using System;
using System.IO;
using System.Reflection;

namespace TypeWalker.Extensions
{
    /// <summary>
    /// Extension to help find the location of given assemblies
    /// </summary>
    public static class AssemblyExtensions
    {
        /// <summary>
        /// Gets the full path to the directory containing the given assembly
        /// </summary>
        /// <param name="assembly">
        /// The assembly for which to the full path to the containing directory
        /// </param>
        /// <returns>
        /// The full path to the directory containing the given assembly
        /// </returns>
        public static string GetAssemblyDirectory(this Assembly assembly)
        {
            return Path.GetDirectoryName(assembly.GetAssemblyPath());
        }

        /// <summary>
        /// Gets the absolute path to the given assembly
        /// </summary>
        /// <param name="assembly">
        /// The assembly for which to get the absolute path
        /// </param>
        /// <returns>
        /// The absolute path to the given assembly
        /// </returns>
        public static string GetAssemblyPath(this Assembly assembly)
        {
            return new Uri(assembly.CodeBase).AbsolutePath;
        }
    }
}