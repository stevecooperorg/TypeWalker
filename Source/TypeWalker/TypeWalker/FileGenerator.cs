using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using TypeWalker.Generators;

namespace TypeWalker
{
    
    public class FileGenerator
    {
        private AssemblyLoader assemblyLoader;

        public FileGenerator(AssemblyLoader assemblyLoader)
        {
            this.assemblyLoader = assemblyLoader;
        }

        private bool TryLoadTypes(string assemblyNameAndNamespaceReference, IRuntime runtime, out Type[] types)
        {
            try
            {
                var parts  =Regex.Split(assemblyNameAndNamespaceReference, "::");
                var assemblyName = parts[0];
                var namespaceName = parts[1];
                var assembly = assemblyLoader.Load(assemblyName);
                
                types = assembly
                    .GetTypes()
                    .Where(t => t.Namespace.StartsWith(namespaceName))
                    .ToArray();

                return true;
            }
            catch (ReflectionTypeLoadException ex)
            {
                runtime.Error(ex.ToString());
                foreach (var loaderException in ex.LoaderExceptions)
                {
                    runtime.Error(loaderException.ToString());
                }

                types = new Type[0];
                return false;
            }
            catch (Exception ex)
            {
                runtime.Error(ex.ToString());
                types = new Type[0];
                return false;
            }
        }

        public bool TryGenerate(string[] assemblyNames, IRuntime runtime, LanguageGenerator generator, out string result)
        {
            try
            {
                foreach (var assemblyName in assemblyNames)
                {
                    Type[] types;
                    if (TryLoadTypes(assemblyName, runtime, out types))
                    {
                        result = generator.Generate(types);
                        return true;
                    }
                    else
                    {
                        result = null;
                        return false;
                    }
                }

                result = null;
                return false;
            }
            catch
            {
                result = null;
                return false;
            }
        }

    }
}
