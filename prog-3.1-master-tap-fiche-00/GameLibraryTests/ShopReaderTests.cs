using GameLibrary;
using GameLibrary.Csv;

namespace GameLibraryTests
{
    public class ShopReaderTests
    {
        private static readonly string[] SHOP_HEADER = new string[] { "LocationId", "OpeningHour", "ClosingHour" };

        private List<LocationData> CreateLocations()
        {
            List<LocationData> location_datas = new List<LocationData>();

            location_datas.Add(new LocationData("WORLD", "World", "", 0));
            location_datas.Add(new LocationData("DAISY_TOWN", "Daisy Town", "WORLD", 180));
            location_datas.Add(new LocationData("GUN_SHOP", "Gun Shop", "DAISY_TOWN", 5));
            location_datas.Add(new LocationData("GENERAL_STORE", "General Store", "DAISY_TOWN", 5));

            return location_datas;
        }

        [Test]
        public void Read_ReturnsAllShopsWithoutErrorForValidFile()
        {
            ContentReport report = new ContentReport();
            CsvTable csv_table = new CsvTable("shops.csv");

            csv_table.SetHeader(SHOP_HEADER);
            csv_table.AddRow(new CsvRow(2, new string[] { "GUN_SHOP", "8", "18" }));
            csv_table.AddRow(new CsvRow(3, new string[] { "GENERAL_STORE", "9", "17" }));

            List<ShopData> shop_datas = ShopReader.Read(csv_table, CreateLocations(), report);

            Assert.That(report.GetErrorCount(), Is.EqualTo(0));
            Assert.That(shop_datas.Count, Is.EqualTo(2));
            Assert.That(shop_datas[0]._locationId, Is.EqualTo("GUN_SHOP"));
            Assert.That(shop_datas[0]._openingHour, Is.EqualTo(8));
            Assert.That(shop_datas[0]._closingHour, Is.EqualTo(18));
        }

        [Test]
        public void Read_ReportsUnknownLocationWithItsLine()
        {
            ContentReport report = new ContentReport();
            CsvTable csv_table = new CsvTable("shops.csv");

            csv_table.SetHeader(SHOP_HEADER);
            csv_table.AddRow(new CsvRow(2, new string[] { "GUN_SHOP", "8", "18" }));
            csv_table.AddRow(new CsvRow(3, new string[] { "GUN_SHOPP", "9", "17" }));

            ShopReader.Read(csv_table, CreateLocations(), report);

            Assert.That(report.GetErrorCount(), Is.EqualTo(1));
            Assert.That(report.GetErrorTextAtIndex(0), Does.Contain("line 3"));
            Assert.That(report.GetErrorTextAtIndex(0), Does.Contain("GUN_SHOPP"));
        }

        [Test]
        public void Read_ReportsLocationDeclaredTwice()
        {
            ContentReport report = new ContentReport();
            CsvTable csv_table = new CsvTable("shops.csv");

            csv_table.SetHeader(SHOP_HEADER);
            csv_table.AddRow(new CsvRow(2, new string[] { "GUN_SHOP", "8", "18" }));
            csv_table.AddRow(new CsvRow(3, new string[] { "GUN_SHOP", "9", "17" }));

            ShopReader.Read(csv_table, CreateLocations(), report);

            Assert.That(report.GetErrorCount(), Is.EqualTo(1));
            Assert.That(report.GetErrorTextAtIndex(0), Does.Contain("line 3"));
            Assert.That(report.GetErrorTextAtIndex(0), Does.Contain("GUN_SHOP"));
        }

        [Test]
        public void Read_ReportsHourThatIsNotAWholeNumber()
        {
            ContentReport report = new ContentReport();
            CsvTable csv_table = new CsvTable("shops.csv");

            csv_table.SetHeader(SHOP_HEADER);
            csv_table.AddRow(new CsvRow(2, new string[] { "GUN_SHOP", "8h", "18" }));

            ShopReader.Read(csv_table, CreateLocations(), report);

            Assert.That(report.GetErrorCount(), Is.EqualTo(1));
            Assert.That(report.GetErrorTextAtIndex(0), Does.Contain("line 2"));
            Assert.That(report.GetErrorTextAtIndex(0), Does.Contain("8h"));
        }

        [Test]
        public void Read_ReportsHourOutOfRange()
        {
            ContentReport report = new ContentReport();
            CsvTable csv_table = new CsvTable("shops.csv");

            csv_table.SetHeader(SHOP_HEADER);
            csv_table.AddRow(new CsvRow(2, new string[] { "GUN_SHOP", "8", "24" }));

            ShopReader.Read(csv_table, CreateLocations(), report);

            Assert.That(report.GetErrorCount(), Is.EqualTo(1));
            Assert.That(report.GetErrorTextAtIndex(0), Does.Contain("line 2"));
            Assert.That(report.GetErrorTextAtIndex(0), Does.Contain("24"));
        }

        [Test]
        public void Read_ReportsOpeningHourThatIsNotBeforeClosingHour()
        {
            ContentReport report = new ContentReport();
            CsvTable csv_table = new CsvTable("shops.csv");

            csv_table.SetHeader(SHOP_HEADER);
            csv_table.AddRow(new CsvRow(2, new string[] { "GUN_SHOP", "18", "8" }));

            ShopReader.Read(csv_table, CreateLocations(), report);

            Assert.That(report.GetErrorCount(), Is.EqualTo(1));
            Assert.That(report.GetErrorTextAtIndex(0), Does.Contain("line 2"));
        }

        [Test]
        public void Read_ReportsEveryErrorOfTheFile()
        {
            ContentReport report = new ContentReport();
            CsvTable csv_table = new CsvTable("shops.csv");

            csv_table.SetHeader(SHOP_HEADER);
            csv_table.AddRow(new CsvRow(2, new string[] { "GUN_SHOPP", "8", "18" }));
            csv_table.AddRow(new CsvRow(3, new string[] { "GENERAL_STORE", "9", "25" }));
            csv_table.AddRow(new CsvRow(4, new string[] { "DAISY_TOWN", "12", "10" }));

            ShopReader.Read(csv_table, CreateLocations(), report);

            Assert.That(report.GetErrorCount(), Is.EqualTo(3));
        }
    }
}
