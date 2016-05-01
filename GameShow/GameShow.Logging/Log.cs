using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// ReSharper disable once CheckNamespace
namespace GameShow
{
    public delegate void LogEvent(string source, string message);
    public class Logging
    {
        public static event LogEvent OnLogMessage;
        public static void LogMessage(string source, string msg)
        {
            Debug.WriteLine(msg);

            if (OnLogMessage != null)
            {
                try
                {
                    OnLogMessage(source, msg);
                }
                catch
                {
                }
            }
        }
    }
}
