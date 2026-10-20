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
        public int _price;

        public ItemData(string id, string name, int price)
        {
            _id = id;
            _name = name;
            _price = price;
        }
    }
}
