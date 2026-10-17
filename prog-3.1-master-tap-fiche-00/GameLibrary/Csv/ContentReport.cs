using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLibrary.Csv
{
    public class ContentReport
    {
        private class ErrorData
        {
            public string _fileName;
            public int _fileLine;
            public string _message;
        }

        private readonly List<ErrorData> _errorTable = new List<ErrorData>();

        public void AddError(string file_name, int file_line, string message)
        {
            _errorTable.Add(new ErrorData { _fileName = file_name, _fileLine = file_line, _message = message });
        }

        public string GetErrorTextAtIndex(int error_index)
        {
            ErrorData error_data = _errorTable[error_index];

            return $"{error_data._fileName}, line {error_data._fileLine}: {error_data._message}";
        }

        public int GetErrorCount()
        {
            return _errorTable.Count;
        }
    }
}
