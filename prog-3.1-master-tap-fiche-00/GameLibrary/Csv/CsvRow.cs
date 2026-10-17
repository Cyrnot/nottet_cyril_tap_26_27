using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLibrary.Csv
{
    public class CsvRow
    {
        private int _lineNumber;
        private string[] _cellTable;

        public CsvRow(int line_number, string[] cell_table)
        {
            _lineNumber = line_number;
            _cellTable = cell_table;
        }

        public string GetCellAtIndex(int cell_index)
        {
            return _cellTable[cell_index].Trim();
        }


        public int GetCellCount()
        {
            return _cellTable.Count();
        }

        public int GetLineNumber()
        {
            return _lineNumber;
        }
    }
}
