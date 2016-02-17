namespace TypeWalker
{
    public interface IRuntime
    {
        void Error(string message, params object[] args);
        void ErrorInFile(string file, int lineNumber, string message, params object[] args);
        void Warn(string message, params object[] args);
        void Log(string message);
    }
}