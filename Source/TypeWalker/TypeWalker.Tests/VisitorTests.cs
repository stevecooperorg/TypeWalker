using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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

            visitor.TypeVisiting += (sender, args) => { trace.AppendFormat("interface {0} {{", args.TypeName).AppendLine(); };
            visitor.TypeVisited +=  (sender, args) => { trace.AppendFormat("}}", args.TypeName).AppendLine(); };

            visitor.MemberVisiting += (sender, args) => { trace.AppendFormat("  {0} : {1};", args.MemberName, args.MemberTypeName).AppendLine(); };
            //visitor.MemberVisited +=  (sender, args) => { trace.AppendFormat("  }}").AppendLine(); };

            var namer = new CSharpLanguage();
            visitor.Visit(typeList, namer);
             
            var actual = trace.ToString().Trim();
            var expected = @"
interface BasicClass {
  GetterSetterString : String;
  GetterPrivateSetterString : Int32;
  NullableGetterSetterBool : Nullable<Boolean>;
  NavigationProperty : ReferencedClass;
  NavigationProperty2 : ReferencedClass;
  StringField : String;
}
interface ReferencedClass {
  SelfReference : ReferencedClass;
  BackReference : BasicClass;
}
".Trim();
            Assert.AreEqual(expected, actual);

        }
    }
}
