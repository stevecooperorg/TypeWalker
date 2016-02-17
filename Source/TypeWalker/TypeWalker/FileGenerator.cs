using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using TypeWalker.Extensions;
using TypeWalker.Generators;

namespace TypeWalker
{
    public class FileGenerator
    {
        private readonly AssemblyLoader assemblyLoader;

        public FileGenerator(AssemblyLoader assemblyLoader)
        {
            this.assemblyLoader = assemblyLoader;
        }

        private bool TryLoadTypes(string filePath, int lineNumber, string assemblyNameAndNamespaceReference, IRuntime runtime, out Type[] types)
        {
            if (string.IsNullOrWhiteSpace(assemblyNameAndNamespaceReference))
            {
                types = new Type[0];
                return true;
            }

            try
            {
                runtime.Log("Parsing " + assemblyNameAndNamespaceReference);
                var parts = Regex.Split(assemblyNameAndNamespaceReference, "::");
                if (parts.Length != 2)
                {
                    var error = string.Format("Unrecognised format: use AssemblyName::Namespace) in '{0}'", assemblyNameAndNamespaceReference);
                    runtime.ErrorInFile(filePath, lineNumber, error);
                    System.Environment.Exit(-1);
                }

                var assemblyName = parts[0];
                var namespaceName = parts[1];
                var assembly = assemblyLoader.Load(assemblyName);

                runtime.Log(string.Format("Generating from assembly {0}, namespace {1}", assemblyName, namespaceName ));

                types = assembly
                    .GetTypes()
                    .Where(TypeExtensions.IsExportableType)
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

        public bool TryGenerate(string filePath, string[] assemblyNames, IRuntime runtime, LanguageGenerator[] generators, out string result)
        {
            try
            {
                bool loadFailed = false;
                List<Type> allTypes = new List<Type>();
                int lineNumber = 0; 
                foreach (var assemblyName in assemblyNames)
                {
                    lineNumber++;
                    Type[] types;
                    if (TryLoadTypes(filePath, lineNumber, assemblyName, runtime, out types))
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

                var sb = new StringBuilder();
                foreach (var generator in generators)
                {
                    var thisResult = generator.Generate(allTypes);
                    sb.Append(thisResult).AppendLine();
                }

                result = sb.ToString();
                return !loadFailed;
            }
            catch
            {
                result = null;
                return false;
            }
        }
    }
}