using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
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

        public void Error(string message, params object[] args)
        {
            this.WriteBuildError("Error", 1, message, args);
        }

        public void Warn(string message, params object[] args)
        {
            this.WriteBuildError("Warning", 1, message, args);
        }

        public void Log(string message)
        {
            Console.WriteLine(exeName + ": " + message);
        }
        private void WriteBuildError(string type, int lineNumber, string message, params object[] args)
        {
            // what messages comes from the client?
            var substititedMessage = string.Format(message, args);

            // what message does MSBuild recognise?
            var exePath = Assembly.GetExecutingAssembly().GetAssemblyPath();
            var msBuildMessage = string.Format(@"{0}({1}) : {2}: {3}.", exePath, lineNumber, type, substititedMessage);

            // write out the message with the appropriate colour
            ConsoleColor color = type == "Error" ? ConsoleColor.Red : ConsoleColor.Yellow;
            Console.ForegroundColor = color;
            Console.WriteLine(msBuildMessage);
            Console.ResetColor();
        }
    }
}
