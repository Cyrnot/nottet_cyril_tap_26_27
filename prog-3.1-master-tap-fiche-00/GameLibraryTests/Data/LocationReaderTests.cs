using NUnit.Framework;
using GameLibrary.Data;
using GameLibrary.Csv;
using System.Collections.Generic;

namespace GameLibraryTests.Data
{
    public class LocationReaderTests
    {
        [Test]
        public void Read_ValidFileNoErrorsAndAllLocationsReturned()
        {
            CsvTable csv_table = new CsvTable("locations.csv");
            ContentReport report = new ContentReport();

            csv_table.AddRow(new CsvRow(1, new[] { "W", "World", "", "" }));
            csv_table.AddRow(new CsvRow(2, new[] { "DT_W", "Daisy Town", "W", "10" }));
            List<LocationData> location_data_table = LocationReader.Read(csv_table, report);

            Assert.That(location_data_table, Has.Count.EqualTo(2));
            Assert.That(report.GetErrorCount(), Is.EqualTo(0));
        }

        [Test]
        public void Read_NoIdAndNoNameReportsError()
        {
            CsvTable csv_table = new CsvTable("locations.csv");
            ContentReport report = new ContentReport();

            csv_table.AddRow(new CsvRow(1, new[] { "", "World", "", "" }));
            csv_table.AddRow(new CsvRow(2, new[] { "DT_W", "", "W", "10" }));
            List<LocationData> location_data_table = LocationReader.Read(csv_table, report);

            Assert.That(location_data_table, Is.Empty);
            Assert.That(report.GetErrorCount(), Is.EqualTo(2));
            Assert.That(report.GetErrorTextAtIndex(0), Does.Contain("line 2"));
            Assert.That(report.GetErrorTextAtIndex(0), Does.Contain("is not a valid reference"));
            Assert.That(report.GetErrorTextAtIndex(1), Does.Contain("line 3"));
            Assert.That(report.GetErrorTextAtIndex(1), Does.Contain("is not a valid name"));
        }

        [Test]
        public void Read_ValidParentAndChildReturnsBothAndNoErrors()
        {
            CsvTable csv_table = new CsvTable("locations.csv");
            ContentReport report = new ContentReport();

            csv_table.AddRow(new CsvRow(1, new[] { "W", "World", "", "" }));
            csv_table.AddRow(new CsvRow(2, new[] { "DT_W", "Daisy Town", "W", "10" }));
            List<LocationData> location_data_table = LocationReader.Read(csv_table, report);

            Assert.That(location_data_table, Has.Count.EqualTo(2));
            Assert.That(location_data_table[0]._id, Is.EqualTo("W"));
            Assert.That(location_data_table[0]._name, Is.EqualTo("World"));
            Assert.That(location_data_table[0]._parent, Is.EqualTo(""));
            Assert.That(location_data_table[1]._id, Is.EqualTo("DT_W"));
            Assert.That(location_data_table[1]._parent, Is.EqualTo("W"));
            Assert.That(location_data_table[1]._timeToTravel, Is.EqualTo(10));
            Assert.That(report.GetErrorCount(), Is.EqualTo(0));
        }

        [Test]
        public void Read_DuplicateIdReportsErrorAndSkipsDuplicate()
        {
            CsvTable csv_table = new CsvTable("locations.csv");
            ContentReport report = new ContentReport();

            csv_table.AddRow(new CsvRow(1, new[] { "a", "First", "", "" }));   
            csv_table.AddRow(new CsvRow(2, new[] { "a", "Second", "", "" }));
            List<LocationData> location_data_table = LocationReader.Read(csv_table, report);

            Assert.That(location_data_table, Has.Count.EqualTo(1));
            Assert.That(location_data_table[0]._id, Is.EqualTo("a"));
            Assert.That(report.GetErrorCount(), Is.EqualTo(1));
            Assert.That(report.GetErrorTextAtIndex(0), Does.Contain("locations.csv"));
            Assert.That(report.GetErrorTextAtIndex(0), Does.Contain("line 3"));
            Assert.That(report.GetErrorTextAtIndex(0), Does.Contain("a ID is already present."));
        }

        [Test]
        public void Read_MultipleRootsReportsError()
        {
            CsvTable csv_table = new CsvTable("locations.csv");
            ContentReport report = new ContentReport();

            csv_table.AddRow(new CsvRow(1, new[] { "W", "World", "", "" }));
            csv_table.AddRow(new CsvRow(2, new[] { "W2", "World2", "", "" }));
            List<LocationData> location_data_table = LocationReader.Read(csv_table, report);

            Assert.That(location_data_table, Has.Count.EqualTo(2));
            Assert.That(report.GetErrorCount(), Is.EqualTo(1));
            Assert.That(report.GetErrorTextAtIndex(0), Does.Contain("there is already a root node."));
            Assert.That(report.GetErrorTextAtIndex(0), Does.Contain("line 3"));
        }

        [Test]
        public void Read_MissingParentReportsParentNotFound()
        {
            CsvTable csv_table = new CsvTable("locations.csv");
            ContentReport report = new ContentReport();

            csv_table.AddRow(new CsvRow(1, new[] { "DT_W", "Daisy Town", "W", "10" }));
            List<LocationData> location_data_table = LocationReader.Read(csv_table, report);

            Assert.That(location_data_table, Has.Count.EqualTo(1));
            Assert.That(report.GetErrorCount(), Is.EqualTo(1));
            Assert.That(report.GetErrorTextAtIndex(0), Does.Contain("not found."));
            Assert.That(report.GetErrorTextAtIndex(0), Does.Contain("line 2"));
        }

        [Test]
        public void Read_NegativeTravelTimeReportsErrorAndSkipsRow()
        {
            CsvTable csv_table = new CsvTable("locations.csv");
            ContentReport report = new ContentReport();

            csv_table.AddRow(new CsvRow(1, new[] { "DT_W", "Daisy Town", "W", "-5" }));
            csv_table.AddRow(new CsvRow(2, new[] { "W", "World", "", "" }));
            List<LocationData> location_data_table = LocationReader.Read(csv_table, report);

            Assert.That(location_data_table, Has.Count.EqualTo(1));
            Assert.That(location_data_table[0]._id, Is.EqualTo("W"));
            Assert.That(report.GetErrorCount(), Is.EqualTo(1));
            Assert.That(report.GetErrorTextAtIndex(0), Does.Contain("Travel time has to be >= 0."));
            Assert.That(report.GetErrorTextAtIndex(0), Does.Contain("line 2"));
        }

        [Test]
        public void Read_FileWithThreeErrors_ReportsThreeErrors()
        {
            CsvTable csv_table = new CsvTable("locations.csv");
            ContentReport report = new ContentReport();

            csv_table.AddRow(new CsvRow(1, new[] { "", "World", "", "" }));
            csv_table.AddRow(new CsvRow(2, new[] { "DT_W", "", "", "" }));
            csv_table.AddRow(new CsvRow(3, new[] { "LHI_DT", "Laughing Horse Inn", "DT_W", "Ten" }));
            List<LocationData> location_data_table = LocationReader.Read(csv_table, report);

            Assert.That(location_data_table, Is.Empty);
            Assert.That(report.GetErrorCount(), Is.EqualTo(3));
        }
    }
}
