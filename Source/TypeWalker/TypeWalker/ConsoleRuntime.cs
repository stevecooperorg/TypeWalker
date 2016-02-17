using System;
using System.IO;
using System.Reflection;
using TypeWalker.Extensions;

namespace TypeWalker
{
    public class ConsoleRuntime : IRuntime
    {
        private readonly string exeName;

        public ConsoleRuntime()
        {
            var asmPath = Assembly.GetExecutingAssembly().GetAssemblyPath();
            this.exeName = Path.GetFileNameWithoutExtension(asmPath);
        }
        public void ErrorInFile(string file, int lineNumber, string message, params object[] args)
        {
            this.WriteBuildError("Error", file, lineNumber, message, args);
        }

        public void Error(string message, params object[] args)
        {
            this.WriteBuildError("Error", exeName, 1, message, args);
        }

        public void Warn(string message, params object[] args)
        {
            this.WriteBuildError("Warning", exeName, 1, message, args);
        }

        public void Log(string message)
        {
            Console.WriteLine(exeName + ": " + message);
        }
        private void WriteBuildError(string type, string filePath, int lineNumber, string message, params object[] args)
        {
            // what messages comes from the client?
            var substititedMessage = string.Format(message, args);

            // what message does MSBuild recognise?
            var msBuildMessage = string.Format(@"{0}({1}) : {2}: {3}.", filePath, lineNumber, type, substititedMessage);

            // write out the message with the appropriate colour
            ConsoleColor color = type == "Error" ? ConsoleColor.Red : ConsoleColor.Yellow;
            Console.ForegroundColor = color;
            Console.WriteLine(msBuildMessage);
            Console.ResetColor();
        }
    }
}