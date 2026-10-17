using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLibrary.Data
{
    public class ShopData
    {
        public string _locationId;
        public int _openingHour;
        public int _closingHour;

        public ShopData(string location_id, int opening_hour, int closing_hour)
        {
            _locationId = location_id;
            _openingHour = opening_hour;
            _closingHour = closing_hour;
        }
    }
}
