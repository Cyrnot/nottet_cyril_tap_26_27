using GameLibrary.Data;
using GameLibrary.Csv;

namespace GameLibraryTests
{
    public class ItemReaderTests
    {
        private static readonly string[] ITEM_HEADER = new string[] { "Id", "Name", "ShopId", "Price", "Quantity" };

        private List<ShopData> CreateShops()
        {
            List<ShopData> shop_datas = new List<ShopData>();

            shop_datas.Add(new ShopData("GUN_SHOP", 8, 18));

            return shop_datas;
        }

        [Test]
        public void Read_ReturnsAllItemsWithoutErrorForValidFile()
        {
            ContentReport report = new ContentReport();
            CsvTable csv_table = new CsvTable("items.csv");

            csv_table.SetHeader(ITEM_HEADER);
            csv_table.AddRow(new CsvRow(2, new string[] { "COLT", "Colt Peacemaker", "GUN_SHOP", "50", "3" }));
            csv_table.AddRow(new CsvRow(3, new string[] { "RIFLE", "Winchester Rifle", "GUN_SHOP", "120", "0" }));

            List<ItemData> item_datas = ItemReader.Read(csv_table, CreateShops(), report);

            Assert.That(report.GetErrorCount(), Is.EqualTo(0));
            Assert.That(item_datas.Count, Is.EqualTo(2));
            Assert.That(item_datas[0]._id, Is.EqualTo("COLT"));
            Assert.That(item_datas[0]._name, Is.EqualTo("Colt Peacemaker"));
            Assert.That(item_datas[0]._shopId, Is.EqualTo("GUN_SHOP"));
            Assert.That(item_datas[0]._price, Is.EqualTo(50));
            Assert.That(item_datas[0]._quantity, Is.EqualTo(3));
        }

        [Test]
        public void Read_ReportsEmptyId()
        {
            ContentReport report = new ContentReport();
            CsvTable csv_table = new CsvTable("items.csv");

            csv_table.SetHeader(ITEM_HEADER);
            csv_table.AddRow(new CsvRow(2, new string[] { "", "Colt Peacemaker", "GUN_SHOP", "50", "3" }));

            ItemReader.Read(csv_table, CreateShops(), report);

            Assert.That(report.GetErrorCount(), Is.EqualTo(1));
            Assert.That(report.GetErrorTextAtIndex(0), Does.Contain("line 2"));
        }

        [Test]
        public void Read_ReportsEmptyName()
        {
            ContentReport report = new ContentReport();
            CsvTable csv_table = new CsvTable("items.csv");

            csv_table.SetHeader(ITEM_HEADER);
            csv_table.AddRow(new CsvRow(2, new string[] { "COLT", "", "GUN_SHOP", "50", "3" }));

            ItemReader.Read(csv_table, CreateShops(), report);

            Assert.That(report.GetErrorCount(), Is.EqualTo(1));
            Assert.That(report.GetErrorTextAtIndex(0), Does.Contain("line 2"));
        }

        [Test]
        public void Read_ReportsIdDeclaredTwice()
        {
            ContentReport report = new ContentReport();
            CsvTable csv_table = new CsvTable("items.csv");

            csv_table.SetHeader(ITEM_HEADER);
            csv_table.AddRow(new CsvRow(2, new string[] { "COLT", "Colt Peacemaker", "GUN_SHOP", "50", "3" }));
            csv_table.AddRow(new CsvRow(3, new string[] { "COLT", "Colt Navy", "GUN_SHOP", "40", "1" }));

            ItemReader.Read(csv_table, CreateShops(), report);

            Assert.That(report.GetErrorCount(), Is.EqualTo(1));
            Assert.That(report.GetErrorTextAtIndex(0), Does.Contain("line 3"));
            Assert.That(report.GetErrorTextAtIndex(0), Does.Contain("COLT"));
        }

        [Test]
        public void Read_ReportsUnknownShopWithItsLine()
        {
            ContentReport report = new ContentReport();
            CsvTable csv_table = new CsvTable("items.csv");

            csv_table.SetHeader(ITEM_HEADER);
            csv_table.AddRow(new CsvRow(2, new string[] { "COLT", "Colt Peacemaker", "GUN_SHOPP", "50", "3" }));

            ItemReader.Read(csv_table, CreateShops(), report);

            Assert.That(report.GetErrorCount(), Is.EqualTo(1));
            Assert.That(report.GetErrorTextAtIndex(0), Does.Contain("line 2"));
            Assert.That(report.GetErrorTextAtIndex(0), Does.Contain("GUN_SHOPP"));
        }

        [Test]
        public void Read_ReportsLocationThatIsNotAShop()
        {
            ContentReport report = new ContentReport();
            CsvTable csv_table = new CsvTable("items.csv");

            csv_table.SetHeader(ITEM_HEADER);
            csv_table.AddRow(new CsvRow(2, new string[] { "COLT", "Colt Peacemaker", "DAISY_TOWN", "50", "3" }));

            ItemReader.Read(csv_table, CreateShops(), report);

            Assert.That(report.GetErrorCount(), Is.EqualTo(1));
            Assert.That(report.GetErrorTextAtIndex(0), Does.Contain("DAISY_TOWN"));
        }

        [Test]
        public void Read_ReportsNegativePrice()
        {
            ContentReport report = new ContentReport();
            CsvTable csv_table = new CsvTable("items.csv");

            csv_table.SetHeader(ITEM_HEADER);
            csv_table.AddRow(new CsvRow(2, new string[] { "COLT", "Colt Peacemaker", "GUN_SHOP", "-50", "3" }));

            ItemReader.Read(csv_table, CreateShops(), report);

            Assert.That(report.GetErrorCount(), Is.EqualTo(1));
            Assert.That(report.GetErrorTextAtIndex(0), Does.Contain("line 2"));
            Assert.That(report.GetErrorTextAtIndex(0), Does.Contain("-50"));
        }

        [Test]
        public void Read_ReportsQuantityThatIsNotAWholeNumber()
        {
            ContentReport report = new ContentReport();
            CsvTable csv_table = new CsvTable("items.csv");

            csv_table.SetHeader(ITEM_HEADER);
            csv_table.AddRow(new CsvRow(2, new string[] { "COLT", "Colt Peacemaker", "GUN_SHOP", "50", "2,5" }));

            ItemReader.Read(csv_table, CreateShops(), report);

            Assert.That(report.GetErrorCount(), Is.EqualTo(1));
            Assert.That(report.GetErrorTextAtIndex(0), Does.Contain("line 2"));
            Assert.That(report.GetErrorTextAtIndex(0), Does.Contain("2,5"));
        }

        [Test]
        public void Read_ReportsEveryErrorOfTheFile()
        {
            ContentReport report = new ContentReport();
            CsvTable csv_table = new CsvTable("items.csv");

            csv_table.SetHeader(ITEM_HEADER);
            csv_table.AddRow(new CsvRow(2, new string[] { "COLT", "Colt Peacemaker", "GUN_SHOPP", "50", "3" }));
            csv_table.AddRow(new CsvRow(3, new string[] { "RIFLE", "", "GUN_SHOP", "120", "1" }));
            csv_table.AddRow(new CsvRow(4, new string[] { "KNIFE", "Bowie Knife", "GUN_SHOP", "10", "-1" }));

            ItemReader.Read(csv_table, CreateShops(), report);

            Assert.That(report.GetErrorCount(), Is.EqualTo(3));
        }
    }
}
