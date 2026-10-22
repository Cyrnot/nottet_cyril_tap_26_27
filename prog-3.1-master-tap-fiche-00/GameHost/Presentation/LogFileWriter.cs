using GameLibrary.Core;

namespace GameHost.Presentation
{
    public class LogFileWriter : ILogWriter
    {
        private string _logFilePath;

        public LogFileWriter(string log_file_path)
        {
            _logFilePath = log_file_path;

            string log_directory = Path.GetDirectoryName(_logFilePath);
            if (!string.IsNullOrEmpty(log_directory))
            {
                Directory.CreateDirectory(log_directory);
            }
        }

        public void WriteLine(string line)
        {
            File.AppendAllText(_logFilePath, line);
        }
    }
}
