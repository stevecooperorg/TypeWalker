using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TypeWalker.Tests
{
    public static class StringAssert
    {
        /// <summary>
        /// Asserts that the two strings, when trimmed with line breaks, have the same content.
        /// </summary>
        /// <param name="assertion">The assertion to act on.</param>
        /// <param name="expected">The expected.</param>
        public static void HaveTrimmedContent(this string expected, string actual)
        {
            var actualTrimmed = TrimWithLineBreaks(actual);
            var expectedTrimmed = TrimWithLineBreaks(expected);

            ReportInequality(expectedTrimmed, actualTrimmed);
        }

        /// <summary>
        /// Equivalent to a normal string equivalence check, but also starts a WinMerge instance if we're debugging.
        /// </summary>
        /// <param name="expected">The expected.</param>
        /// <param name="actual">The actual.</param>
        public static void ReportInequality(string expected, string actual)
        {
            var comparison = StringComparison.InvariantCulture;
            if (String.Compare(expected, actual, comparison) != 0)
            {
                StartMergeProgram(expected, actual);
                var differenceAt = FirstDifferenceOnLine(expected, actual, StringComparison.InvariantCulture);
                if (differenceAt == -1)
                {
                    // not actually different, just the new line characters were different
                }
                else
                {
                    Assert.AreEqual(expected, actual, "Strings start to differ at line " + differenceAt);
                }
            }
        }

        public static IEnumerable<string> SplitToLines(this string input)
        {
            if (input == null)
            {
                yield break;
            }

            using (System.IO.StringReader reader = new System.IO.StringReader(input))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    yield return line;
                }
            }
        }

        private static int FirstDifferenceOnLine(string expected, string actual, StringComparison comparison)
        {
            var expectedLines = expected.SplitToLines().ToArray();
            var actualLines = actual.SplitToLines().ToArray();
            var numberOfComparableLines = Math.Min(expectedLines.Length, actualLines.Length);

            for (var lineIdx = 0; lineIdx < numberOfComparableLines; lineIdx++)
            {
                var expectedLine = expectedLines[lineIdx];
                var actualLine = actualLines[lineIdx];
                var comparisonResult = String.Compare(expectedLine, actualLine, comparison);
                if (comparisonResult != 0)
                {
                    // 1-index the lines, as most editors use line 1 for the first line
                    return lineIdx + 1;
                }
            }

            return -1;
        }

        /// <summary>
        /// Starts a winmerge instance in debug mode, showing the expected and actuals.
        /// </summary>
        /// <param name="expected">The expected.</param>
        /// <param name="actual">The actual.</param>
        private static void StartMergeProgram(string expected, string actual)
        {
            var mergePrograms = new[] {
                @"C:\Program Files (x86)\WinMerge\WinMergeU.exe",
                @"C:\Program Files (x86)\Beyond Compare 3\BCompare.exe"
            };

            var mergeProgram = mergePrograms.FirstOrDefault(p => File.Exists(p));

            if (mergeProgram != null)
            {
                // save out the files
                var testName = "test";
                var invalidChars = Path.GetInvalidPathChars();
                var testFilePrefix = new string(testName.Where(c => !invalidChars.Contains(c)).ToArray());
                var tmpFileExpected = Path.Combine(Path.GetTempPath(), testFilePrefix + "_expected.txt");
                var tmpFileActual = Path.Combine(Path.GetTempPath(), testFilePrefix + "_actual.txt");
                var args = string.Format("\"{0}\" \"{1}\"", tmpFileExpected, tmpFileActual);
                File.WriteAllText(tmpFileExpected, expected);
                File.WriteAllText(tmpFileActual, actual);
                if (System.Diagnostics.Debugger.IsAttached)
                {
                    // show the failed string in winmerge, etc
                    System.Diagnostics.Process.Start(mergeProgram, args);
                }
                Console.WriteLine();
                Console.WriteLine("\"" + mergeProgram + "\" " + args);
                Console.WriteLine();
            }
        }

        private static string TrimWithLineBreaks(string s)
        {
            return s.Trim(new[] { '\r', '\n', ' ', '\t' });
        }
    }
}