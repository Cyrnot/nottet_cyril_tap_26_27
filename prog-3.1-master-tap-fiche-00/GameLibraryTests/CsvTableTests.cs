using GameLibrary;
using GameLibrary.Csv;

namespace GameLibraryTests
{
    public class CsvTableTests
    {
        [Test]
        public void GetFileName_ReturnsConstructionFileName()
        {
            CsvTable csv_table = new CsvTable("locations.csv");

            string file_name = csv_table.GetFileName();

            Assert.That(file_name, Is.EqualTo("locations.csv"));
        }

        [Test]
        public void GetHasHeader_ReturnsFalseBeforeSetHeader()
        {
            CsvTable csv_table = new CsvTable("locations.csv");

            bool has_header = csv_table.GetHasHeader();

            Assert.That(has_header, Is.False);
        }

        [Test]
        public void GetHasHeader_ReturnsTrueAfterSetHeader()
        {
            CsvTable csv_table = new CsvTable("locations.csv");

            csv_table.SetHeader(new string[] { "Id", "Name", "ParentId", "TravelMinutes" });

            Assert.That(csv_table.GetHasHeader(), Is.True);
        }

        [Test]
        public void GetColumnCount_ReturnsHeaderSize()
        {
            CsvTable csv_table = new CsvTable("locations.csv");

            csv_table.SetHeader(new string[] { "Id", "Name", "ParentId", "TravelMinutes" });

            Assert.That(csv_table.GetColumnCount(), Is.EqualTo(4));
        }

        [Test]
        public void GetColumnIndex_ReturnsPositionOfColumnName()
        {
            CsvTable csv_table = new CsvTable("locations.csv");
            csv_table.SetHeader(new string[] { "Id", "Name", "ParentId", "TravelMinutes" });

            int column_index = csv_table.GetColumnIndex("ParentId");

            Assert.That(column_index, Is.EqualTo(2));
        }

        [Test]
        public void GetColumnIndex_ReturnsMinusOneForUnknownColumn()
        {
            CsvTable csv_table = new CsvTable("locations.csv");
            csv_table.SetHeader(new string[] { "Id", "Name", "Parent", "TravelMinutes" });

            int column_index = csv_table.GetColumnIndex("ParentId");

            Assert.That(column_index, Is.EqualTo(-1));
        }

        [Test]
        public void GetColumnIndex_IgnoresSpacesAroundHeaderNames()
        {
            CsvTable csv_table = new CsvTable("locations.csv");
            csv_table.SetHeader(new string[] { "Id", " Name ", "ParentId", "TravelMinutes\r" });

            int name_index = csv_table.GetColumnIndex("Name");
            int travel_index = csv_table.GetColumnIndex("TravelMinutes");

            Assert.That(name_index, Is.EqualTo(1));
            Assert.That(travel_index, Is.EqualTo(3));
        }

        [Test]
        public void GetColumnNameAtIndex_ReturnsHeaderName()
        {
            CsvTable csv_table = new CsvTable("locations.csv");
            csv_table.SetHeader(new string[] { "Id", "Name", "ParentId", "TravelMinutes" });

            string column_name = csv_table.GetColumnNameAtIndex(3);

            Assert.That(column_name, Is.EqualTo("TravelMinutes"));
        }

        [Test]
        public void GetRowCount_ReturnsZeroWhenNoRowWasAdded()
        {
            CsvTable csv_table = new CsvTable("locations.csv");
            csv_table.SetHeader(new string[] { "Id", "Name", "ParentId", "TravelMinutes" });

            int row_count = csv_table.GetRowCount();

            Assert.That(row_count, Is.EqualTo(0));
        }

        [Test]
        public void GetRowAtIndex_ReturnsRowsInAddedOrder()
        {
            CsvTable csv_table = new CsvTable("locations.csv");
            CsvRow world_row = new CsvRow(2, new string[] { "WORLD", "World", "", "" });
            CsvRow town_row = new CsvRow(3, new string[] { "DAISY_TOWN", "Daisy Town", "WORLD", "180" });
            csv_table.SetHeader(new string[] { "Id", "Name", "ParentId", "TravelMinutes" });

            csv_table.AddRow(world_row);
            csv_table.AddRow(town_row);

            Assert.That(csv_table.GetRowCount(), Is.EqualTo(2));
            Assert.That(csv_table.GetRowAtIndex(0), Is.EqualTo(world_row));
            Assert.That(csv_table.GetRowAtIndex(1), Is.EqualTo(town_row));
        }
    }
}
