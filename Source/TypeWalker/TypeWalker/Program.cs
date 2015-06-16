using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TypeWalker.Generators;

namespace TypeWalker
{
    public static class Program
    {
        [System.STAThread]
        public static void Main(string[] args)
        {
            var runtime = new ConsoleRuntime();

            string configFile = null;
            string language = null;
            string knockoutPrefix = null;
            string outputFile = null;

            var optionSet = new NDesk.Options.OptionSet() {
                    { "language=", "", v => language = v },
                    { "knockoutPrefix=", "", v => knockoutPrefix = v },
                    { "configFile=", "", v => configFile = v },
                    { "outputFile=", "", v => outputFile = v }
                };

            optionSet.Parse(args);

            if (!File.Exists (configFile))
            {
                runtime.Error("config file '{0}' does not exist: use, e.g., /configFile=<configFile.xml>", configFile);
                return;
            }

            
            runtime.Log("Reading config file: " + configFile);

            var assemblyLoader = new AssemblyLoader(runtime);

            LanguageGenerator generator;

            switch (language)
            {
                case TypeScriptGenerator.Id:
                    generator = new TypeScriptGenerator();
                    break;

                case KnockoutMappingGenerator.Id:
                    generator = new KnockoutMappingGenerator(knockoutPrefix);
                    break;

                default:
                    runtime.Error("Unknown language: {0}", language);
                    return;
            }

            var lines = File.ReadAllLines(configFile);
            var fileGenerator = new FileGenerator(assemblyLoader);
            
            string fileContent;
            fileGenerator.TryGenerate(lines, runtime, generator, out fileContent);

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
            //runtime.Error("not yet implemented, but running!");

        }
    }
}
