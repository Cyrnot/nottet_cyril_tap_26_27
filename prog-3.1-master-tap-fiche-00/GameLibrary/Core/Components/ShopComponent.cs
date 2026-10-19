using GameLibrary.Data;

namespace GameLibrary.Core.Components
{
    public class ShopComponent : Component
    {
        private float GAME_MINUTES_PER_SECOND = 10.0f;
        private ShopData _shopData;
        private List<ItemData> _itemDatas;
        private float _timeInMinutes;


        public ShopComponent(ShopData shop_data, List<ItemData> item_datas, GameObject game_object) : base(game_object)
        {
            _shopData = shop_data;
            _itemDatas = item_datas;
            _timeInMinutes = 8 * 60;
        }

        public override void FixedUpdate(float elapsed_fixed_time)
        {
            _timeInMinutes += elapsed_fixed_time * GAME_MINUTES_PER_SECOND;
            _timeInMinutes %= 24 * 60;
        }

        public int GetHour()
        {
            // Cheat
            return (int)MathF.Floor(_timeInMinutes / 60.0f);
        }

        public bool GetIsOpen()
        {
            return GetHour() >= _shopData._openingHour && GetHour() < _shopData._closingHour;
        }

        public ItemData GetItemAtIndex(int item_index)
        {
            return _itemDatas[item_index];
        }

        public int GetItemCount()
        {
            return _itemDatas.Count();
        }

        public int GetMinute()
        {
            // Cheat
            return (int)MathF.Floor(_timeInMinutes % 60);
        }

        public int GetQuantityAtIndex(int item_index)
        {
            return _itemDatas[item_index]._quantity;
        }

        public bool TryRemoveOneAtIndex(int item_index)
        {
            if (_itemDatas[item_index]._quantity > 0)
            {
                _itemDatas[item_index]._quantity -= 1;

                return true;
            }
            return false;
        }

        public ShopData GetShopData()
        {
            return _shopData;

        }
    }
}
