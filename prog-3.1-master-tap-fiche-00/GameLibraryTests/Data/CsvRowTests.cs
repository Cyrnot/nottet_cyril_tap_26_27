using GameLibrary;
using GameLibrary.Csv;

namespace GameLibraryTests.Data
{
    public class CsvRowTests
    {
        [Test]
        public void GetLineNumber_ReturnsConstructionLineNumber()
        {
            CsvRow row = new CsvRow(7, new string[] { "GUN_SHOP", "Gun Shop" });

            int line_number = row.GetLineNumber();

            Assert.That(line_number, Is.EqualTo(7));
        }

        [Test]
        public void GetCellCount_ReturnsNumberOfValues()
        {
            CsvRow row = new CsvRow(2, new string[] { "GUN_SHOP", "Gun Shop", "DAISY_TOWN", "5" });

            int cell_count = row.GetCellCount();

            Assert.That(cell_count, Is.EqualTo(4));
        }

        [Test]
        public void GetCellAtIndex_ReturnsValuesInFileOrder()
        {
            CsvRow row = new CsvRow(2, new string[] { "GUN_SHOP", "Gun Shop", "DAISY_TOWN", "5" });

            string first_cell = row.GetCellAtIndex(0);
            string last_cell = row.GetCellAtIndex(3);

            Assert.That(first_cell, Is.EqualTo("GUN_SHOP"));
            Assert.That(last_cell, Is.EqualTo("5"));
        }

        [Test]
        public void GetCellAtIndex_ReturnsValueWithoutSurroundingSpaces()
        {
            CsvRow row = new CsvRow(2, new string[] { "  GUN_SHOP ", " Gun Shop\r" });

            string id_cell = row.GetCellAtIndex(0);
            string name_cell = row.GetCellAtIndex(1);

            Assert.That(id_cell, Is.EqualTo("GUN_SHOP"));
            Assert.That(name_cell, Is.EqualTo("Gun Shop"));
        }

        [Test]
        public void GetCellAtIndex_KeepsEmptyValue()
        {
            CsvRow row = new CsvRow(2, new string[] { "WORLD", "World", "", "" });

            string parent_cell = row.GetCellAtIndex(2);

            Assert.That(parent_cell, Is.EqualTo(""));
        }
    }
}