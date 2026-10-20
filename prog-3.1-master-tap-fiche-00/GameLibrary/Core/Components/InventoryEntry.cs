using GameLibrary.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLibrary.Core.Components
{
    public class InventoryEntry
    {
        private ItemData _itemData;
        private int _quantity;

        public InventoryEntry(ItemData itemData, int quantity)
        {
            _itemData = itemData;
            _quantity = quantity;
        }

        public ItemData GetItem()
        {
            return _itemData;
        }

        public int GetQuantity()
        {
            return _quantity;
        }

        public void SetQuantity(int quantity)
        {
            _quantity = quantity;
        }
    }
}
