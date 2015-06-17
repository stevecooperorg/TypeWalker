using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TypeWalker.Generators;

namespace TypeWalker.Tests
{
    [TestClass]
    public class Tests
    {
        [TestMethod]
        public void TypescriptGenerator_GeneratesWorkingTypescript()
        {
            var generator = new TypeScriptGenerator();
            var actual = generator.Generate(new List<Type> { typeof(NamespaceOfTestClasses.BasicClass) });
            var expected = Results.Resources.TypeScriptResult;
            StringAssert.HaveTrimmedContent(expected, actual);
        }

        [TestMethod]
        public void KnockoutMappingGenerator_GeneratesWorkingTypescript()
        {
            var generator = new KnockoutMappingGenerator("KOGenerated");
            var actual = generator.Generate(new List<Type> { typeof(NamespaceOfTestClasses.BasicClass) });
            var expected = Results.Resources.KnockoutResult;
            StringAssert.HaveTrimmedContent(expected, actual);
        }

        private string ReferenceToClass(Type t)
        {
            var assemblyName = t.Assembly.GetName().Name;
            var namespaceOfType = t.Namespace;
            var fullReference = string.Format("{0}::{1}", assemblyName, namespaceOfType);
            return fullReference;                
        }

        [TestMethod]
        public void FileGenerator_HandlesSeparateTypesNeatly()
        {
            // don't want two separate namespaces to duplicate used types;
            var rt = new DebugRuntime();
            var loader = AssemblyLoader.FromExecutingAssembly(rt);
            var fileGenerator = new FileGenerator(loader);
            string actual;

            var roots = new[] { 
                ReferenceToClass(typeof(NamespaceOfTestClasses.BasicClass)),                
                ReferenceToClass(typeof(AlternateNamespace.DistinctClass))
            };

            foreach (var root in roots)
            {
                Console.WriteLine(root);
            }

            fileGenerator.TryGenerate("test file", roots, rt, new LanguageGenerator[] { new TypeScriptGenerator() }, out actual);
            Assert.IsNotNull(actual, "Doesn't look like it generated");
            StringAssert.HaveTrimmedContent(Results.Resources.FileGeneratorResults, actual);
        }
    }
}
