using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jenny.Core
{
    public class LogService
    {
        public void LogAssistant(string text)
        {
            LogWithColor(text, ConsoleColor.White);
        }

        public void LogUser(string text)
        {
            LogWithColor(text, ConsoleColor.Gray);
        }

        public void LogWithColor(string text, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(text);
        }
    }
}
