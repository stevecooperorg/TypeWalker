using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
