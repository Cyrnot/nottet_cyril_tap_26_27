using GameLibrary.Data;
using System.Collections.Generic;

namespace GameLibrary.Core.Components
{
    public class MineComponent : Component
    {
        private int _stock;
        private readonly int _minutesPerStrike;
        private readonly List<ItemData> _oreItemTable = new List<ItemData>();
        private readonly List<int> _oreWeightTable = new List<int>();

        public MineComponent(GameObject game_object, int stock, int minutes_per_strike) : base(game_object)
        {
            _stock = stock;
            _minutesPerStrike = minutes_per_strike;
        }

        public int GetStock()
        {
            return _stock;
        }

        public int GetMinutesPerStrike()
        {
            return _minutesPerStrike;
        }

        public void AddOre(ItemData item_data, int weight)
        {
            if (item_data != null && weight > 0)
            {
                _oreItemTable.Add(item_data);
                _oreWeightTable.Add(weight);
            }
        }

        public int GetTotalOreWeight()
        {
            int total_weight = 0;

            for (int ore_index = 0; ore_index < _oreWeightTable.Count; ore_index++)
            {
                total_weight += _oreWeightTable[ore_index];
            }

            return total_weight;
        }

        public ItemData GetOreAtWeightIndex(int weight_index)
        {
            for (int ore_index = 0; ore_index < _oreItemTable.Count; ore_index++)
            {
                weight_index -= _oreWeightTable[ore_index];

                if (weight_index < 0)
                {
                    return _oreItemTable[ore_index];
                }
            }

            return null;
        }

        public bool RemoveOneFromStock()
        {
            if (_stock <= 0)
            {
                return false;
            }

            _stock--;
            return true;
        }
    }
}
