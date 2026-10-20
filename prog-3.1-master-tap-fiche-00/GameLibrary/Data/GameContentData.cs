using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLibrary.Data
{
    public class GameContentData
    {
        public List<LocationData> _locationDataTable;
        public List<ShopData> _shopDataTable;
        public Dictionary<string, ItemData> _itemDataTable;
        public List<InventoryEntryData> _inventoryDataTable;
        public List<HeroData> _heroDataTable;
    }
}
