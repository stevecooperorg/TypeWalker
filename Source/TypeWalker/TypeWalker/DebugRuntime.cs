using System.Diagnostics;

namespace TypeWalker
{
    public class DebugRuntime : IRuntime
    {
        public void Error(string message, params object[] args)
        {
            Debug.WriteLine("Error: " + string.Format(message, args));
        }

        public void ErrorInFile(string file, int lineNumber, string message, params object[] args)
        {
            this.Error(string.Format("{0} {1} {2}", file, lineNumber, message), args);
        }

        public void Log(string message)
        {
            Debug.WriteLine("Info: " + message);
        }

        public void Warn(string message, params object[] args)
        {
            Debug.WriteLine("Warn: " + string.Format(message, args));
        }
    }
}