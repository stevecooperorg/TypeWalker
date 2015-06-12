using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TypeWalker
{
    public class DebugRuntime: IRuntime
    {
        public void Error(string message, params object[] args)
        {
            Debug.WriteLine("Error: " + string.Format(message, args));
        }

        public void Warn(string message, params object[] args)
        {
            Debug.WriteLine("Warn: " + string.Format(message, args));
        }

        public void Log(string message)
        {
            Debug.WriteLine("Info: " + message);
        }
    }
}
