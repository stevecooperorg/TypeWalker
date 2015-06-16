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
            if (string.IsNullOrWhiteSpace(assemblyNameAndNamespaceReference))
            {
                types = new Type[0];
                return true;
            }

            try
            {
                var parts = Regex.Split(assemblyNameAndNamespaceReference, "::");
                var assemblyName = parts[0];
                var namespaceName = parts[1];
                var assembly = assemblyLoader.Load(assemblyName);

                runtime.Log(string.Format("Generating from assembly {0}, namespace {1}", assemblyName, namespaceName ));

                types = assembly
                    .GetTypes()
                    .Where(t => !t.IsAbstract && !t.IsInterface && !t.IsGenericTypeDefinition && !t.IsGenericType)
                    .Where(t => t.Namespace.StartsWith(namespaceName))
                    .ToArray();

                runtime.Log("Loaded " + types.Length.ToString() + " types");

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
                bool loadFailed = false;
                List<Type> allTypes = new List<Type>();
                foreach (var assemblyName in assemblyNames)
                {
                    Type[] types;
                    if (TryLoadTypes(assemblyName, runtime, out types))
                    {
                        allTypes.AddRange(types);
                    }
                    else
                    {
                        loadFailed = true;
                        break;
                    }
                }

                runtime.Log("Loaded a a total of " + allTypes.Count + " types.");
                result = generator.Generate(allTypes);
                return true;
            }
            catch
            {
                result = null;
                return false;
            }
        }

    }
}
