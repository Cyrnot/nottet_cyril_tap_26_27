using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLibrary.Data
{
    public class ItemData
    {
        public string _id;
        public string _name;
        public string _shopId;
        public int _price;
        public int _quantity;

        public ItemData(string id, string name, string shopId, int price, int quantity)
        {
            _id = id;
            _name = name;
            _shopId = shopId;
            _price = price;
            _quantity = quantity;
        }
    }
}
