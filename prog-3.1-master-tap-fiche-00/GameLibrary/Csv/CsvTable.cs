using System.Reflection.Metadata.Ecma335;

namespace GameLibrary.Csv
{
    public class CsvTable
    {
        private string _fileName;
        private List<CsvRow> _rowTable = new List<CsvRow>();

        private List<string> _rowHeaderTable;

        public CsvTable(string file_name)
        {
            _fileName = file_name;
        }

        public void AddRow(CsvRow csvRow)
        {
            _rowTable.Add(csvRow);
        }

        public int GetColumnCount()
        {
            return _rowHeaderTable.Count();
        }

        public int GetColumnIndex(string column_id)
        {
            int column_index = -1;

            for (int header_column_index = 0; header_column_index < _rowHeaderTable.Count; header_column_index++)
            {
                if (_rowHeaderTable[header_column_index].Contains(column_id))
                {
                    column_index = header_column_index;
                }
            }

            return column_index;
        }

        public string GetColumnNameAtIndex(int column_index)
        {
            return _rowHeaderTable[column_index];
        }

        public string GetFileName()
        {
            return _fileName;
        }

        public bool GetHasHeader()
        {
            return _rowHeaderTable != null;
        }

        public CsvRow GetRowAtIndex(int row_index)
        {
            return _rowTable[row_index];
        }

        public int GetRowCount()
        {
            return _rowTable.Count();
        }

        public void SetHeader(string[] row_header_table)
        {
            _rowHeaderTable = new List<string>(row_header_table);
        }
    }
}
