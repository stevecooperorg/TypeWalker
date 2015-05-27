using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TypeWalker.Tests
{
    public static class StringAssert
    {
        public static void AreEqual(string expected, string actual)
        {
            if (expected != actual)
            {
                var expectedLines = expected.Split('\r').Select(s => s.Replace("\n", "")).ToList();
                var actualLines = actual.Split('\r').Select(s => s.Replace("\n", "")).ToList();

                for (var i = 1; i < Math.Min(expectedLines.Count, actualLines.Count); i++)
                {
                    if (expectedLines[i] != actualLines[i])
                    {
                        var fail = string.Format(@"
Fail on line {0}:
Expected <{1}>
Actual:  <{2}>
", i, expectedLines[i], actualLines[i]);
                        Assert.AreEqual(expected, actual, fail);
                    }
                }

                Assert.AreEqual(expected, actual);
            }
        }
    }
}
