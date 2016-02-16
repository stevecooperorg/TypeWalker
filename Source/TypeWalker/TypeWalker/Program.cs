using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TypeWalker.Generators;

namespace TypeWalker
{
    public static class Program
    {
        [System.STAThread]
        public static int Main(string[] args)
        {
            var runtime = new ConsoleRuntime();

            string configFile = null;
            var languages = new List<String>();
            string knockoutPrefix = null;
            string outputFile = null;

            var optionSet = new NDesk.Options.OptionSet() {
                    { "language=", "", v => languages.Add(v) },
                    { "knockoutPrefix=", "", v => knockoutPrefix = v },
                    { "configFile=", "", v => configFile = v },
                    { "outputFile=", "", v => outputFile = v }
                };

            optionSet.Parse(args);

            if (!File.Exists (configFile))
            {
                runtime.Error("config file '{0}' does not exist: use, e.g., /configFile=<configFile.xml>", configFile);
                return -1;
            }

            runtime.Log("Reading config file: " + configFile);

            var assemblyLoader = new AssemblyLoader(runtime);

            var generators = languages.Select(language => GetGenerator(language, knockoutPrefix, runtime)).ToArray();

            var lines = File.ReadAllLines(configFile);
            var fileGenerator = new FileGenerator(assemblyLoader);
            
            string fileContent;
            var loaded = fileGenerator.TryGenerate(configFile, lines, runtime, generators, out fileContent);

            if (!loaded)
            {
                runtime.Error("Could not generate the file for " + configFile);
                return -2;
            }

            var fullOutputFile = Path.GetFullPath(outputFile);

            if (!File.Exists(fullOutputFile) || File.ReadAllText(fullOutputFile) != fileContent)
            {
                runtime.Log("TypeWalker is writing a new version of " + fullOutputFile);
                File.WriteAllText(fullOutputFile, fileContent);
            }
            else
            {
                runtime.Log("TypeWalker output file is up to date: " + fullOutputFile);
            }

            return 0;
        }

        private static LanguageGenerator GetGenerator(string language, string knockoutPrefix, IRuntime runtime)
        {
                switch (language)
                {
                    case TypeScriptGenerator.Id:
                        return new TypeScriptGenerator();
                       
                    case KnockoutMappingGenerator.Id:
                        return new KnockoutMappingGenerator(knockoutPrefix);
                       
                    default:
                        runtime.Error("Unknown language: {0}", language);
                        System.Environment.Exit(-1);
                        return (LanguageGenerator)null;
            }       
        }
    }
}
