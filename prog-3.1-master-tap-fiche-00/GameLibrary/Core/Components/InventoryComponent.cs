using GameLibrary.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLibrary.Core.Components
{
    public class InventoryComponent : Component
    {
        private readonly List<InventoryEntry> _inventoryDataTable = new List<InventoryEntry>();

        public InventoryComponent(GameObject game_object) : base(game_object)
        {
        }

        public void Add(ItemData item_data, int quantity)
        {
            if (quantity <= 0)
            {
                throw new ArgumentException("Quantity must be greater than zero");
            }
            InventoryEntry entry = GetEntry(item_data._id);
            if (entry != null)
            {
                entry.SetQuantity(entry.GetQuantity() + quantity);
            }
            else
            {
                _inventoryDataTable.Add(new InventoryEntry(item_data, quantity));
            }

        }

        public InventoryEntry GetEntry(string id)
        {
            InventoryEntry inventory_entry = null;

            for (int item_index = 0; item_index < _inventoryDataTable.Count; item_index++)
            {
                if (_inventoryDataTable[item_index].GetItem()._id == id)
                {
                    inventory_entry = _inventoryDataTable[item_index];
                }
            }

            return inventory_entry;
        }

        public InventoryEntry GetEntryAtIndex(int inventory_index)
        {
            return _inventoryDataTable[inventory_index];
        }

        public int GetEntryCount()
        {
            return _inventoryDataTable.Count();
        }

        public int GetQuantity(string id)
        {
            int quantity = 0;
            InventoryEntry inventory_data = GetEntry(id);

            if(inventory_data != null)
            {
                quantity = inventory_data.GetQuantity();
            }

            return quantity;
        }

        public bool TryRemove(string id, int quantity)
        {
            if (quantity <= 0)
            {
                throw new ArgumentException("Quantity must be greater than zero");
            }

            bool does_remove = false;
            InventoryEntry inventory_entry = GetEntry(id);

            if ((inventory_entry != null) && (inventory_entry.GetQuantity() >= quantity))
            {
                inventory_entry.SetQuantity(Math.Max(0, inventory_entry.GetQuantity() - quantity));
                does_remove = true;
            }

            return does_remove;
        }
    }
}
