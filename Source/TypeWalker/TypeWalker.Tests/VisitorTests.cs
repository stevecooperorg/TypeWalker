using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TypeWalker.Generators;

namespace TypeWalker.Tests
{
    [TestClass]
    public class VisitorTests
    {
        [TestMethod]
        public void Visitor_OutputIsCorrect()
        {
            var trace = new StringBuilder();
            var visitor = new Visitor();
            var typeList = new List<Type> { typeof(BasicClass) };

            visitor.NameSpaceVisiting += (sender, args) => { trace.AppendFormat("start namespace {0}", args.NameSpaceName).AppendLine(); };
            visitor.NameSpaceVisited += (sender, args) => { trace.AppendFormat("end namespace {0}", args.NameSpaceName).AppendLine().AppendLine(); };

            visitor.TypeVisiting += (sender, args) => { trace.AppendFormat("  start type {0}", args.TypeName).AppendLine(); };
            visitor.TypeVisited +=  (sender, args) => { trace.AppendFormat("  end type {0}", args.TypeName).AppendLine(); };

            visitor.MemberVisiting += (sender, args) => { trace.AppendFormat("    start property '{0}' of type '{1}'", args.MemberName, args.MemberTypeName).AppendLine(); };
            visitor.MemberVisited += (sender, args) => { trace.AppendFormat("    end property '{0}' of type '{1}'", args.MemberName, args.MemberTypeName).AppendLine(); };

            var namer = new CSharpLanguage();
            visitor.Visit(typeList, namer);
             
            var actual = trace.ToString().Trim();
            var expected = Results.Resources.VisitorTestsResult.Trim();
            Assert.AreEqual(expected, actual);

        }
    }
}
