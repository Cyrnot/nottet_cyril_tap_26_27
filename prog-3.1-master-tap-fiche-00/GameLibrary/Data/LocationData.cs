using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLibrary.Data
{
    public class LocationData
    {
        public string _id;
        public string _name;
        public string _parent;
        public int _timeToTravel;

        public LocationData(string id, string name, string parent, int time_to_travel)
        {
            _id = id;
            _name = name;
            _parent = parent;
            _timeToTravel = time_to_travel;
        }
    }
}
