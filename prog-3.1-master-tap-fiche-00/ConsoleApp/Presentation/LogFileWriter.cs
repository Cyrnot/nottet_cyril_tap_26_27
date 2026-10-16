using GameLibrary.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameConsole.Presentation
{
    public class LogFileWriter : ILogWriter
    {
        private string _logFilePath;

        public LogFileWriter(string log_file_path)
        {
            _logFilePath = log_file_path;
        }

        public void WriteLine(string line)
        {
            File.AppendAllText(_logFilePath, line);
        }
    }
}
